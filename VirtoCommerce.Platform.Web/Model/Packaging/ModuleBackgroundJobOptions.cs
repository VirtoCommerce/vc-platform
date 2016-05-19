using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.Model.Packaging
{
    public class ModuleBackgroundJobOptions
    {
        public ModuleAction Action { get; set; }
        public ManifestModuleInfo[] Modules { get; set; }
    }
}
