using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.Modularity
{
    public class ModuleBackgroundJobOptions
    {
        public ModuleAction Action { get; set; }
        public ModuleDescriptor[] Modules { get; set; }
    }
}
