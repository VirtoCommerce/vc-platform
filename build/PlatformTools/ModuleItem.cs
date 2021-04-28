using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformTools
{
    class ModuleItem
    {
        public ModuleItem(string id, string version)
        {
            Id = id;
            Version = version;
        }
        public string Id { get; set; }
        public string Version { get; set; }
    }
}
