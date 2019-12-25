using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryGenerator.Hashes
{
    public class Md5 : IHash
    {
        public string Name { get => "MD5";}

        public string Generate(string text)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(text));

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
