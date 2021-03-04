using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.JsonConverters
{
    /// <summary>
    /// Generic converter to instantiate objects of type T from JSON
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PolymorphGenericJsonConverter<T> : JsonConverter
    {
        private readonly string _typePropertyNameLower;
        private readonly string _typePropertyNameUpper;

        readonly HashSet<Type> _knownTypes;

        /// <summary>
        /// Instantiate for every (base) entity type.
        /// </summary>
        /// <param name="typePropertyName">The property name where more specific type name is stored (optional)</param>
        public PolymorphGenericJsonConverter(string typePropertyName = default)
        {
            if (!typePropertyName.IsNullOrEmpty())
            {
                _typePropertyNameLower = char.IsLower(typePropertyName, 0) ? typePropertyName : char.ToLowerInvariant(typePropertyName[0]) + typePropertyName.Substring(1);
                _typePropertyNameUpper = char.IsUpper(typePropertyName, 0) ? typePropertyName : char.ToUpperInvariant(typePropertyName[0]) + typePropertyName.Substring(1);
            }

            var types = AbstractTypeFactory<T>
                            .AllTypeInfos
                            .Select(x => x.Type)
                            .Union(new[] { typeof(T) });
            _knownTypes = new HashSet<Type>(types);
        }

        public override bool CanWrite => false;
        public override bool CanRead => true;

        protected virtual T Create(Type objectType, JObject jObject)
        {
            var typeName = objectType.Name;

            if (_typePropertyNameLower != null)
            {
                var pt = jObject[_typePropertyNameLower] ?? jObject[_typePropertyNameUpper];
                if (pt != null)
                {
                    typeName = pt.Value<string>();
                }
            }

            return AbstractTypeFactory<T>.TryCreateInstance(typeName);
        }

        public override bool CanConvert(Type objectType)
        {
            return _knownTypes.Contains(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var target = Create(objectType, jObject);

            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
