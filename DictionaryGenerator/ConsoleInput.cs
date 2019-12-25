using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DictionaryGenerator
{
    internal static class ConsoleInput
    {
        public static T ReadLine<T>(string text)
        {
            string input;
            Console.Write(text);
            input = Console.ReadLine();

            while (!Is(input, typeof(T)))
            {
                Console.Write(text);
                input = Console.ReadLine();
            }

            return (T)Convert.ChangeType(input, typeof(T));
        }

        private static bool Is(string input, Type targetType)
        {
            try
            {
                TypeDescriptor.GetConverter(targetType).ConvertFromString(input);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
