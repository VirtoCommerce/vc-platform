using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.DynamicProperties.Converters
{
    public static class DynamicPropertyObjectValueConverter
    {
        public static DynamicPropertyObjectValueEntity[] ToEntity(this DynamicPropertyObjectValue model, DynamicProperty property)
        {
            var result = new List<DynamicPropertyObjectValueEntity>();

            if (model.DictionaryItemId != null)
            {
                var entity = ToEntity(null, property, null);
                entity.DictionaryItemId = model.DictionaryItemId;
                result.Add(entity);
            }
            else if (model.ArrayValues != null)
            {
                result.AddRange(model.ArrayValues.Select(v => v.ToEntity(property, model.Locale)));
            }
            else if (model.Value != null)
            {
                result.Add(model.Value.ToEntity(property, model.Locale));
            }

            return result.ToArray();
        }

        public static DynamicPropertyObjectValueEntity ToEntity(this string value, DynamicProperty model, string locale)
        {
            var result = new DynamicPropertyObjectValueEntity
            {
                ObjectType = model.ObjectType,
                ObjectId = model.ObjectId,
                ValueType = model.ValueType.ToString(),
                Locale = locale,
            };

            switch (model.ValueType)
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
                    result.LongTextValue = value;
                    break;
                case DynamicPropertyValueType.SecureString:
                    result.ShortTextValue = value;
                    break;
                default:
                    result.ShortTextValue = value;
                    break;
            }

            return result;
        }
    }
}
