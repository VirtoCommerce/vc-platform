using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
namespace VirtoCommerce.Platform.Data.DynamicProperties.Converters
{
    public static class DynamicPropertyObjectValueConverter
    {
        public static DynamicPropertyObjectValue ToModel(this DynamicPropertyObjectValueEntity entity)
        {
			var retVal = new DynamicPropertyObjectValue();
			retVal.Locale = entity.Locale;
			if(entity.DictionaryItem != null)
			{
				retVal.Value = entity.DictionaryItem.ToModel();
			}
			else
			{
				retVal.Value = entity.RawValue();
			}
			return retVal;
        }
    
        public static DynamicPropertyObjectValueEntity ToEntity(this DynamicPropertyObjectValue propertyValue, DynamicObjectProperty property)
        {
            var result = new DynamicPropertyObjectValueEntity
            {
                ObjectType = property.ObjectType,
				ObjectId = property.ObjectId,
                ValueType = property.ValueType.ToString()
            };

			result.InjectFrom(propertyValue);

            if (property.IsDictionary)
            {
				var item = propertyValue.Value as DynamicPropertyDictionaryItem;

                if (item == null)
                {
					var jObject = propertyValue.Value as JObject;
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
                        result.BooleanValue = Convert.ToBoolean(propertyValue.Value);
                        break;
                    case DynamicPropertyValueType.DateTime:
						result.DateTimeValue = Convert.ToDateTime(propertyValue.Value, CultureInfo.InvariantCulture);
                        break;
                    case DynamicPropertyValueType.Decimal:
						result.DecimalValue = Convert.ToDecimal(propertyValue.Value, CultureInfo.InvariantCulture);
                        break;
                    case DynamicPropertyValueType.Integer:
						result.IntegerValue = Convert.ToInt32(propertyValue.Value, CultureInfo.InvariantCulture);
                        break;
                    case DynamicPropertyValueType.LongText:
						result.LongTextValue = (string)propertyValue.Value;
                        break;
                    default:
						result.ShortTextValue = (string)propertyValue.Value;
                        break;
                }
            }

            return result;
        }

		public static void Patch(this DynamicPropertyObjectValueEntity source, DynamicPropertyObjectValueEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");


			var patchInjectionPolicy = new PatchInjection<DynamicPropertyObjectValueEntity>(x => x.Locale, x => x.LongTextValue, x => x.BooleanValue, x => x.DateTimeValue,
																							x => x.DecimalValue, x => x.DictionaryItemId, x => x.IntegerValue, x => x.ShortTextValue);
			target.InjectFrom(patchInjectionPolicy, source);

		}

		
    }

	public class DynamicPropertyObjectValueComparer : IEqualityComparer<DynamicPropertyObjectValueEntity>
	{
		#region IEqualityComparer<DynamicPropertyObjectValueEntity> Members

		public bool Equals(DynamicPropertyObjectValueEntity x, DynamicPropertyObjectValueEntity y)
		{
			return GetHashCode(x) == GetHashCode(y);
		}

		public int GetHashCode(DynamicPropertyObjectValueEntity obj)
		{
			var result = obj.RawValue();
			return result.GetHashCode();
		}


		#endregion
	}
}
