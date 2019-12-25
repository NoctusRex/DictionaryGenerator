using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryGenerator
{
    public interface IHash
    {
        public string Name { get; }
        public string Generate(string text);
    }
}
