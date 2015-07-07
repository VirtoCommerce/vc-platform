using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.DynamicProperties.Converters
{
    public static class DynamicPropertyValueConverter
    {
        public static DynamicPropertyValueEntity[] ToEntity(this DynamicPropertyValue value, DynamicProperty property)
        {
            var result = new List<DynamicPropertyValueEntity>();

            if (value.ArrayValues != null)
            {
                result.AddRange(value.ArrayValues.Select(v => v.ToEntity(property, value.Locale)));
            }
            else if (value.Value != null)
            {
                result.Add(value.Value.ToEntity(property, value.Locale));
            }

            return result.ToArray();
        }

        public static DynamicPropertyValueEntity ToEntity(this string value, DynamicProperty property, string locale)
        {
            var result = new DynamicPropertyValueEntity
            {
                SearchKey = string.Join("-", property.ObjectType, property.Name, property.ObjectId),
                ObjectType = property.ObjectType,
                ObjectId = property.ObjectId,
                ValueType = property.ValueType.ToString(),
                Locale = locale,
            };

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
