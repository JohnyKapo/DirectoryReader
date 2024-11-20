using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.model
{
    public class AbstractDirectoryObject(string name, string path) : IDirectoryObject
    {
        public string Name { get; set; } = name;
        public string Path { get; set; } = path;

        public string GetName()
        {
            return Name;
        }

        public string GetPath()
        {
            return Path;
        }
    }
}
