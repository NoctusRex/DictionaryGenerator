using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DictionaryGenerator
{
    internal class HashLoader
    {
        public List<IHash> Hashes { get; private set; }

        public HashLoader() { Hashes = new List<IHash>(); }

        public void Load()
        {
            GetDlls().ForEach(d =>
            {
                try
                {
                    foreach (Type t in Assembly.LoadFrom(d).GetTypes())
                    {
                        if (typeof(IHash) == t) { continue; }
                        if (t.GetInterface(typeof(IHash).FullName) is null) { continue; }

                        Hashes.Add((IHash)Activator.CreateInstance(t));
                    }

                }
                catch { }

            });
        }

        private List<string> GetDlls() => Directory.GetFiles(NoRe.Database.General.Pathmanager.StartupDirectory).Where(x => x.EndsWith(".dll")).ToList();

    }
}
