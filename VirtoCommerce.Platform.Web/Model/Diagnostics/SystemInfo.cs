using VirtoCommerce.Platform.Web.Licensing;
using VirtoCommerce.Platform.Web.Model.Modularity;

namespace VirtoCommerce.Platform.Web.Model.Diagnostics
{
    public class SystemInfo
    {
        public string PlatformVersion { get; set; }
        public License License { get; set; }
        public ModuleDescriptor[] InstalledModules { get; set; }
    }
}
