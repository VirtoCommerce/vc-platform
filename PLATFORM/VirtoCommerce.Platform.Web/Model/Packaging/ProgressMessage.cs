using Newtonsoft.Json;

namespace VirtoCommerce.Platform.Web.Model.Packaging
{
    public class ProgressMessage
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("level")]
        public string Level { get; set; }
    }
}
