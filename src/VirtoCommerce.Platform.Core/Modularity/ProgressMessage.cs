using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ProgressMessage
    {
        public string Message { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ProgressMessageLevel Level { get; set; }
    }
}
