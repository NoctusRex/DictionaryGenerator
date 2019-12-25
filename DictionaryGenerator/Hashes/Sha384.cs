using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryGenerator.Hashes
{
    public class Sha384 : IHash
    {
        public string Name => "SHA384";

        public string Generate(string text)
        {
            using (System.Security.Cryptography.SHA384 sha384 = System.Security.Cryptography.SHA384.Create())
            {
                byte[] hashBytes = sha384.ComputeHash(Encoding.ASCII.GetBytes(text));

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
