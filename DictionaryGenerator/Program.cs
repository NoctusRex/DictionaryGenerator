using NoRe.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DictionaryGenerator
{
    class Program
    {
        static readonly HashLoader hashLoader = new HashLoader();
        static readonly List<string> combinations = new List<string>();
        static readonly string tableName = "table_" + DateTime.Now.ToString("ddMMyyyyHHmmss");
        static char[] chars;
        static UInt64 possibleCombinations = 0;
        static UInt64 generatedCombinations = 0;

        static void Main()
        {

            try
            {
                Start();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.BackgroundColor = ConsoleColor.White;

                Console.WriteLine(ex.ToString());
            }

        }

        static void Start()
        {
            int minCombinationLength = ConsoleInput.ReadLine<int>("Min combination length: ");
            int maxCombinationLength = ConsoleInput.ReadLine<int>("Max combination length: ");
            int insertInterval = ConsoleInput.ReadLine<int>("Insert interval: ");

            hashLoader.Load();
            hashLoader.Hashes.ForEach(h => { Console.WriteLine($"Loaded hash {h}"); });

            LoadChars();
            for (int i = 1; i < maxCombinationLength + 1; i++)
            {
                possibleCombinations += (UInt64)Math.Pow(chars.Length, i);
            }
            Console.WriteLine($"Loaded {chars.Length} chars. Possible combinations: {possibleCombinations}");

            CreateTable();
            Console.WriteLine($"Created table {tableName}");

            Console.WriteLine("Starting ...");
            System.Threading.Thread.Sleep(5000);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;

            if (minCombinationLength <= 0) minCombinationLength = 1;
            for (int i = minCombinationLength; i <= maxCombinationLength; i++)
            {
                Generate(chars, i, "", chars.Length, insertInterval);
            }

            if (combinations.Count > 0)
            {
                InsertCombinations();
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Generation finished");

        }

        static void LoadChars()
        {
            List<char> printableChars = new List<char>();
            for (int i = char.MinValue; i <= char.MaxValue; i++)
            {
                char c = Convert.ToChar(i);
                if (char.IsControl(c)) continue;
                if (!IsRepresentable(c, WinApi.GetKeyboardLayout(WinApi.GetCurrentThreadId()))) continue;

                printableChars.Add(c);
            }
            chars = printableChars.ToArray();
        }

        static bool IsRepresentable(char c, IntPtr keyboardLayout)
        {
            var x = WinApi.VkKeyScanEx(c, keyboardLayout);
            return x != -1;
        }

        static void Generate(char[] chars, int i, string @string, int len, int insertInterval)
        {
            // base case 
            if (i == 0) // when len has been reached 
            {
                // print it out 
                PrintCombination(@string);
                combinations.Add(@string);

                if (combinations.Count >= insertInterval)
                {
                    InsertCombinations();
                }

                // cnt++; 
                return;
            }

            // iterate through the array 
            for (int j = 0; j < len; j++)
            {

                // Create new string with next character 
                // Call generate again until string has 
                // reached its len 
                String appended = @string + chars[j];
                Generate(chars, i - 1, appended, len, insertInterval);
            }

            return;
        }

        static void CreateTable()
        {
            string statement = $"CREATE TABLE {tableName} (id BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT, combination TEXT NOT NULL";

            hashLoader.Hashes.ForEach(h =>
           {
               statement += $", {h.Name} TEXT";
           });

            statement += ");";

            new Wrapper(DatabaseType.MySql).ExecuteNonQuery(statement);
        }

        static void InsertCombinations()
        {
            int counter = 0;

            Console.Clear();
            CheckConnection();

            Console.WriteLine($"{generatedCombinations} of {possibleCombinations} combinations");
            Console.WriteLine($"Inserting {combinations.Count} combinations");
            generatedCombinations += (UInt64)combinations.Count;

            combinations.ForEach(c =>
            {
                string statement = $"INSERT INTO {tableName} (combination,";
                hashLoader.Hashes.ForEach(h =>
                {
                    statement += $"{h.Name},";
                });
                statement = statement.TrimEnd(',');
                statement += ") VALUES (@0,";

                GenerateHashes(c).ForEach(h =>
               {
                   statement += $"'{h}',";
               });
                statement = statement.TrimEnd(',');
                statement += ")";

                try
                {
                    CheckConnection();

                    using (Wrapper wrapper = new Wrapper())
                    {
                        wrapper.ExecuteNonQuery(statement, c);
                    }
                    counter++;
                    Console.Write($"\r{counter} / {combinations.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(statement);
                    Console.WriteLine(ex);
                }

            });

            Console.WriteLine();
            combinations.Clear();
        }

        static List<string> GenerateHashes(string combination) => hashLoader.Hashes.Select(h => h.Generate(combination)).ToList();

        static void PrintCombination(string combination)
        {
            switch (Console.ForegroundColor)
            {
                case ConsoleColor.Magenta:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case ConsoleColor.Blue:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;

                case ConsoleColor.Cyan:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;

                case ConsoleColor.Green:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

                case ConsoleColor.Yellow:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case ConsoleColor.DarkYellow:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case ConsoleColor.Red:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
            }

            Console.Write("\r" + combination);

        }

        static void CheckConnection()
        {
            while (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                Console.WriteLine("No network connection available. Waiting 5 seconds ...");
                System.Threading.Thread.Sleep(5000);
            }

            while (true)
            {
                try
                {
                    using Wrapper wrapper = new Wrapper();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Can not connect to database. '" + ex.Message + "' Waiting 5 seconds ...");
                    System.Threading.Thread.Sleep(5000);
                }
            }

        }

    }

}
