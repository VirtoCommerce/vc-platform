using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Core.JsonConverters
{
    public class PolymorphJsonConverter : JsonConverter
    {
        private static readonly Type[] _knowTypes = { typeof(ObjectSettingEntry), typeof(DynamicProperty), typeof(ApplicationUser), typeof(Role), typeof(PermissionScope), typeof(PushNotification) };

        public override bool CanWrite => false;
        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return _knowTypes.Any(x => x.IsAssignableFrom(objectType));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object result;
            if (typeof(PermissionScope).IsAssignableFrom(objectType))
            {
                var obj = JObject.Load(reader);
                var scopeType = objectType.Name;
                var pt = obj["type"] ?? obj["Type"];
                if (pt != null)
                {
                    scopeType = pt.Value<string>();
                }
                result = AbstractTypeFactory<PermissionScope>.TryCreateInstance(scopeType);
                if (result == null)
                {
                    throw new NotSupportedException("Unknown scopeType: " + scopeType);
                }
                serializer.Populate(obj.CreateReader(), result);
            }
            else if (typeof(PushNotification).IsAssignableFrom(objectType))
            {
                var typeNameHandling = serializer.TypeNameHandling;
                var typeNameAssemblyFormatHandling = serializer.TypeNameAssemblyFormatHandling;

                serializer.TypeNameHandling = TypeNameHandling.Auto;
                serializer.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full;

                result = serializer.Deserialize(reader);

                serializer.TypeNameHandling = typeNameHandling;
                serializer.TypeNameAssemblyFormatHandling = typeNameAssemblyFormatHandling;
            }
            else
            {
                var obj = JObject.Load(reader);
                var tryCreateInstance = typeof(AbstractTypeFactory<>).MakeGenericType(objectType).GetMethods().FirstOrDefault(x => x.Name.EqualsInvariant("TryCreateInstance") && x.GetParameters().Length == 0);
                result = tryCreateInstance?.Invoke(null, null);
                serializer.Populate(obj.CreateReader(), result);
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
