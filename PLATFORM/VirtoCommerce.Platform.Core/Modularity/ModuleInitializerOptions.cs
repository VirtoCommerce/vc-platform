using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ModuleInitializerOptions : IModuleInitializerOptions
    {
        public IDictionary<string, string> ModuleDirectories { get; private set; }

        public ModuleInitializerOptions()
        {
            ModuleDirectories = new Dictionary<string, string>();
        }

        public virtual string GetModuleDirectoryPath(string moduleId)
        {
            return ModuleDirectories[moduleId];
        }
    }
}
