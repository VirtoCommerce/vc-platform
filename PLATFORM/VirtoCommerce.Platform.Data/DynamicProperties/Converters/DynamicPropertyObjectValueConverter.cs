using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.DynamicProperties.Converters
{
    public static class DynamicPropertyObjectValueConverter
    {
        public static DynamicPropertyObjectValue ToModel(this DynamicPropertyObjectValueEntity entity)
        {
            var retVal = new DynamicPropertyObjectValue();
            retVal.Locale = entity.Locale;
            if (entity.DictionaryItem != null)
            {
                retVal.Value = entity.DictionaryItem.ToModel();
            }
            else
            {
                retVal.Value = entity.RawValue();
            }
            return retVal;
        }

        public static DynamicPropertyObjectValueEntity ToEntity(this DynamicPropertyObjectValue propertyValue, DynamicObjectProperty property, string objectId, string objectType)
        {
            var result = new DynamicPropertyObjectValueEntity
            {
                ObjectType = property.ObjectType,
                ObjectId = property.ObjectId,
                ValueType = property.ValueType.ToString()
            };

            result.InjectFrom(propertyValue);

            if (!string.IsNullOrEmpty(objectId))
                result.ObjectId = objectId;
            if (!string.IsNullOrEmpty(objectType))
                result.ObjectType = objectType;

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
                        result.BooleanValue = propertyValue.Value.ToNullable<Boolean>();
                        break;
                    case DynamicPropertyValueType.DateTime:
                        result.DateTimeValue = propertyValue.Value.ToNullable<DateTime>();
                        break;
                    case DynamicPropertyValueType.Decimal:
                        result.DecimalValue = propertyValue.Value.ToNullable<Decimal>();
                        break;
                    case DynamicPropertyValueType.Integer:
                        result.IntegerValue = propertyValue.Value.ToNullable<Int32>();
                        break;
                    case DynamicPropertyValueType.ShortText:
                        result.ShortTextValue = (string)propertyValue.Value;
                        break;
                    default:
                        result.LongTextValue = (string)propertyValue.Value;
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
            if (result != null)
            {
                return result.GetHashCode();
            }
            return 0;
        }


        #endregion
    }
}
