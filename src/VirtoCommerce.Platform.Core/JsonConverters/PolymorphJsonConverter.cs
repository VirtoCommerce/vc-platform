using System;
using System.Collections.Concurrent;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.JsonConverters
{
    public class PolymorphJsonConverter : JsonConverter
    {
        private static readonly ConcurrentDictionary<Type, Func<JObject, object>> _convertFactories = new ConcurrentDictionary<Type, Func<JObject, object>>();
        private static readonly ConcurrentDictionary<Type, bool> _canConvertCache = new ConcurrentDictionary<Type, bool>();

        public override bool CanWrite => false;
        public override bool CanRead => true;


        public static void RegisterTypeForDiscriminator(Type type, string discriminator)
        {

            RegisterType(type, obj =>
            {
                var typeName = type.Name;
                var pt = obj[discriminator] ?? obj[discriminator.FirstCharToUpper()];
                if (pt != null)
                {
                    typeName = pt.Value<string>();
                }
                var tryCreateInstance = typeof(AbstractTypeFactory<>).MakeGenericType(type).GetMethods().FirstOrDefault(x => x.Name.Equals("TryCreateInstance") && x.GetParameters().Length == 1);
                var result = tryCreateInstance?.Invoke(null, new[] { typeName });
                if (result == null)
                {
                    throw new NotSupportedException("Unknown scopeType: " + typeName);
                }
                return result;
            });

        }
        public static void RegisterType(Type type, Func<JObject, object> factory)
        {
            _canConvertCache[type] = true;
            _convertFactories[type] = factory;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType.IsPrimitive || objectType.Equals(typeof(string)) || objectType.IsArray)
            {
                return false;
            }
            var result = _canConvertCache.GetOrAdd(objectType, _ =>
            {
                return (bool)typeof(AbstractTypeFactory<>).MakeGenericType(objectType).GetProperty("HasOverrides").GetValue(null, null);
            });
            return result;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object result;
            var obj = JObject.Load(reader);

            object Factory(JObject obj2)
            {
                //TODO: Optmimize reflection
                var tryCreateInstance = typeof(AbstractTypeFactory<>).MakeGenericType(objectType).GetMethods().FirstOrDefault(x => x.Name.Equals("TryCreateInstance") && x.GetParameters().Length == 0);
                return tryCreateInstance?.Invoke(null, null);
            };
            var factory = _convertFactories.GetOrAdd(objectType, _ => Factory);

            result = factory(obj);

            serializer.Populate(obj.CreateReader(), result);
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
