using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Platform.Web.Model.Packaging
{
    public class ModuleBackgroundJobOptions
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ModuleAction Action { get; set; }
        public string PackageId { get; set; }
        public string PackageFilePath { get; set; }
    }
}
