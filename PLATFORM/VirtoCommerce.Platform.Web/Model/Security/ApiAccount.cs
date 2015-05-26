using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class ApiAccount
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ApiAccountType ApiAccountType { get; set; }
        public bool? IsActive { get; set; }
        public string AppId { get; set; }
        public string SecretKey { get; set; }
    }
}
