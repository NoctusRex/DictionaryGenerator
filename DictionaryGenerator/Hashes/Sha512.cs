using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryGenerator.Hashes
{
    public class Sha512 : IHash
    {
        public string Name => "SHA512";

        public string Generate(string text)
        {
            using (System.Security.Cryptography.SHA512 sha512 = System.Security.Cryptography.SHA512.Create())
            {
                byte[] hashBytes = sha512.ComputeHash(Encoding.ASCII.GetBytes(text));

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
