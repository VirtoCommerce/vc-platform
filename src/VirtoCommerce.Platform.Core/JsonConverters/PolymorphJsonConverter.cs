using System;
using System.Collections.Concurrent;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.JsonConverters
{
    public class PolymorphJsonConverter : JsonConverter
    {
        
        public class CreateInstanceCacheKey
        {
            public CreateInstanceCacheKey(Type type, bool withDiscriminator)
            {
                _type = type;
                _withDiscriminator = withDiscriminator;
            }

            private readonly Type _type;
            private readonly bool _withDiscriminator;

            public override bool Equals(object obj)
            {
                return (obj is CreateInstanceCacheKey objToCompare) && (_type.Equals(objToCompare._type) && _withDiscriminator.Equals(objToCompare._withDiscriminator));
            }
            public override int GetHashCode()
            {
                return HashCode.Combine(_type, _withDiscriminator);
            }
        }
        
        /// <summary>
        /// Factory methods for create instances of proper classes during deserialization
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Func<JObject, object>> _convertFactories = new ConcurrentDictionary<Type, Func<JObject, object>>();
        /// <summary>
        /// Cache for conversion possibility (to reduce AbstractTypeFactory calls thru reflection)
        /// </summary>
        private static readonly ConcurrentDictionary<Type, bool> _canConvertCache = new ConcurrentDictionary<Type, bool>();
        /// <summary>
        /// Cache for instance creation method infos (to reduce AbstractTypeFactory calls thru reflection)
        /// </summary>
        private static readonly ConcurrentDictionary<CreateInstanceCacheKey, MethodInfo> _createInstanceMethodsCache = new ConcurrentDictionary<CreateInstanceCacheKey, MethodInfo>();

        public override bool CanWrite => false;
        public override bool CanRead => true;


        public static void RegisterTypeForDiscriminator(Type type, string discriminator)
        {

            RegisterType(type, obj =>
            {
                var typeName = type.Name;
                var pt = obj[discriminator] ?? obj[discriminator.FirstCharToLower()];
                if (pt != null)
                {
                    typeName = pt.Value<string>();
                }

                var tryCreateInstance = _createInstanceMethodsCache.GetOrAdd(new CreateInstanceCacheKey(type, true), _ =>
                    typeof(AbstractTypeFactory<>).MakeGenericType(type).GetMethod("TryCreateInstance", new Type[] {typeof(string) }));
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
                var tryCreateInstance = _createInstanceMethodsCache.GetOrAdd(new CreateInstanceCacheKey(objectType, false), _ =>
                    typeof(AbstractTypeFactory<>).MakeGenericType(objectType).GetMethod("TryCreateInstance", new Type[] {}));
                return tryCreateInstance?.Invoke(null, null);
            };
            var factory = _convertFactories.GetOrAdd(objectType, _ => Factory); // Fall-back instance creation for discriminator-less cases

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
