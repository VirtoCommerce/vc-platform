using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.DynamicProperties.Converters
{
    public static class DynamicPropertyObjectValueConverter
    {
        public static object ToModel(this DynamicPropertyObjectValueEntity entity)
        {
            return entity.DictionaryItem != null ? entity.DictionaryItem.ToModel() : entity.RawValue();
        }

        public static DynamicPropertyObjectValueEntity[] ToEntity(this IEnumerable<DynamicPropertyObjectValue> models, List<DynamicProperty> properties)
        {
            var result = new List<DynamicPropertyObjectValueEntity>();

            var propertyGroups = models.GroupBy(v => v.Property.Id).ToList();

            foreach (var propertyGroup in propertyGroups)
            {
                var propertyValues = new List<DynamicPropertyObjectValueEntity>();

                var property = properties.First(p => p.Id == propertyGroup.Key);

                foreach (var model in propertyGroup)
                {
                    var entities = (model.Values ?? new object[0]).Select(v => v.ToEntity(property, model.ObjectId, model.Locale)).ToList();
                    propertyValues.AddRange(entities);
                }

                // Keep only one value for each locale if property is not array
                if (!property.IsArray)
                {
                    propertyValues = propertyValues.GroupBy(v => new { v.ObjectId, v.Locale }).Select(g => g.Last()).ToList();
                }

                result.AddRange(propertyValues);
            }

            return result.ToArray();
        }

        public static DynamicPropertyObjectValueEntity ToEntity(this object value, DynamicProperty property, string objectId, string locale)
        {
            var result = new DynamicPropertyObjectValueEntity
            {
                PropertyId = property.Id,
                ObjectType = property.ObjectType,
                ObjectId = objectId,
                ValueType = property.ValueType.ToString(),
                Locale = property.IsMultilingual ? locale : null,
            };

            if (property.IsDictionary)
            {
                var item = value as DynamicPropertyDictionaryItem;

                if (item == null)
                {
                    var jObject = value as JObject;
                    if (jObject != null)
                    {
                        item = jObject.ToObject<DynamicPropertyDictionaryItem>();
                    }
                }

                if (item != null)
                    result.DictionaryItemId = item.Id;
            }
            else
            {
                switch (property.ValueType)
                {
                    case DynamicPropertyValueType.Boolean:
                        result.BooleanValue = Convert.ToBoolean(value);
                        break;
                    case DynamicPropertyValueType.DateTime:
                        result.DateTimeValue = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                        break;
                    case DynamicPropertyValueType.Decimal:
                        result.DecimalValue = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                        break;
                    case DynamicPropertyValueType.Integer:
                        result.IntegerValue = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                        break;
                    case DynamicPropertyValueType.LongText:
                        result.LongTextValue = (string)value;
                        break;
                    default:
                        result.ShortTextValue = (string)value;
                        break;
                }
            }

            return result;
        }
    }
}
