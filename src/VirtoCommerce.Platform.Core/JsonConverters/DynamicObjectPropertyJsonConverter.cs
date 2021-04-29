using System;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Platform.Core.JsonConverters
{
    public class DynamicObjectPropertyJsonConverter : PolymorphJsonConverter
    {
        private readonly IDynamicPropertyMetaDataResolver _metadataResolver;
        public DynamicObjectPropertyJsonConverter(IDynamicPropertyMetaDataResolver metadataResolver)
        {
            _metadataResolver = metadataResolver;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IHasDynamicProperties).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = base.ReadJson(reader, objectType, existingValue, serializer);
            if (result is IHasDynamicProperties hasDynProp)
            {
                hasDynProp.ResolveMetaDataAsync(_metadataResolver).GetAwaiter().GetResult();
            }
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
