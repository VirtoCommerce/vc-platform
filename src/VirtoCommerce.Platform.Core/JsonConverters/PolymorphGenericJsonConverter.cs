using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.JsonConverters
{
    public class PolymorphGenericJsonConverter<T> : JsonConverter
    {
        readonly HashSet<Type> _knownTypes;

        public PolymorphGenericJsonConverter()
        {
            var types = AbstractTypeFactory<T>
                            .AllTypeInfos
                            .Select(x => x.Type)
                            .Union(new[] { typeof(T) });
            _knownTypes = new HashSet<Type>(types);
        }

        protected virtual T Create(Type objectType, JObject jObject)
        {
            return AbstractTypeFactory<T>.TryCreateInstance();
        }

        public override bool CanWrite => false;
        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return _knownTypes.Contains(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            T target = Create(objectType, jObject);

            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
