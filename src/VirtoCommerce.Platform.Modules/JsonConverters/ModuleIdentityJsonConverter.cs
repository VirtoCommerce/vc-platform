using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules
{
    public class ModuleIdentityJsonConverter : JsonConverter
    {
        private static readonly Type[] _knowTypes = { typeof(ModuleIdentity) };

        public override bool CanWrite => false;
        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return _knowTypes.Any(x => x.IsAssignableFrom(objectType));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            var id = obj.GetValue("id", StringComparison.InvariantCultureIgnoreCase)?.Value<string>();
            var version = obj.GetValue("version", StringComparison.InvariantCultureIgnoreCase)?.ToObject<Version>();
            if (id != null && version != null)
            {
                var result = new ModuleIdentity(id, new SemanticVersion(version));
                return result;
            }
            throw new JsonReaderException("id or version is required");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
