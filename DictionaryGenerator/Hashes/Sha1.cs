using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryGenerator.Hashes
{
    public class Sha1 : IHash
    {
        public string Name => "SHA1";

        public string Generate(string text)
        {
            using (System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.ASCII.GetBytes(text));

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
