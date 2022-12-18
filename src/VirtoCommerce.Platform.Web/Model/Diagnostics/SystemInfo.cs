using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Web.Licensing;
using VirtoCommerce.Platform.Web.Modularity;

namespace VirtoCommerce.Platform.Web.Model.Diagnostics
{
    public class SystemInfo
    {
        public string PlatformVersion { get; set; }
        public License License { get; set; }
        public ModuleDescriptor[] InstalledModules { get; set; }
        public string Version { get; set; }
        public bool Is64BitOperatingSystem { get; set; }
        public bool Is64BitProcess { get; set; }
        public string DatabaseProvider { get; set; }
    }
}
