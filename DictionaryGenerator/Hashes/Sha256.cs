using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryGenerator.Hashes
{
    public class Sha256 : IHash
    {
        public string Name => "SHA256";

        public string Generate(string text)
        {
            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(text));

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
