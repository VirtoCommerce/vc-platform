using VirtoCommerce.Platform.Web.Licensing;

namespace VirtoCommerce.Platform.Web.Model.Diagnostics
{
    public class SystemInfo
    {
        public PlatformInfo Platform { get; set; }

        public License License { get; set; }

        public InstalledModuleInfo[] InstalledModules { get; set; }
    }
}
