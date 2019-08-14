using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Platform.Data.Model
{
    public class DynamicPropertyObjectValueEntity : AuditableEntity
    {
        [StringLength(256)]
        [Index("IX_ObjectType_ObjectId", 1)]
        public string ObjectType { get; set; }

        [StringLength(128)]
        [Index("IX_ObjectType_ObjectId", 2)]
        public string ObjectId { get; set; }

        [StringLength(64)]
        public string Locale { get; set; }

        [Required]
        [StringLength(64)]
        public string ValueType { get; set; }

        [StringLength(512)]
        public string ShortTextValue { get; set; }
        public string LongTextValue { get; set; }
        public decimal? DecimalValue { get; set; }
        public int? IntegerValue { get; set; }
        public bool? BooleanValue { get; set; }
        public DateTime? DateTimeValue { get; set; }

        public string PropertyId { get; set; }
        public virtual DynamicPropertyEntity Property { get; set; }

        public string DictionaryItemId { get; set; }
        public virtual DynamicPropertyDictionaryItemEntity DictionaryItem { get; set; }

        public virtual DynamicPropertyObjectValue ToModel(DynamicPropertyObjectValue propValue)
        {
            if (propValue == null)
            {
                throw new ArgumentNullException(nameof(propValue));
            }

            propValue.Locale = Locale;
            propValue.ObjectId = ObjectId;
            propValue.ObjectType = ObjectType;
            propValue.ValueType = EnumUtility.SafeParse(ValueType, DynamicPropertyValueType.LongText);

            if (DictionaryItem != null)
            {
                propValue.ValueId = DictionaryItem.Id;
                propValue.Value = DictionaryItem.ToModel(AbstractTypeFactory<DynamicPropertyDictionaryItem>.TryCreateInstance());
            }
            else
            {
                propValue.Value = GetValue(propValue.ValueType);
            }
            return propValue;
        }

        public virtual DynamicPropertyObjectValueEntity FromModel(DynamicPropertyObjectValue propValue)
        {
            if (propValue == null)
            {
                throw new ArgumentNullException(nameof(propValue));
            }

            Locale = propValue.Locale;
            ObjectId = propValue.ObjectId;
            ObjectType = propValue.ObjectType;
            ValueType = propValue.ValueType.ToString();
            DictionaryItemId = propValue.ValueId;

            var dictItem = propValue.Value as DynamicPropertyDictionaryItem;
            if (dictItem == null)
            {
                if (propValue.Value is JObject jObject)
                {
                    dictItem = jObject.ToObject<DynamicPropertyDictionaryItem>();
                }
            }

            if (dictItem != null)
            {
                DictionaryItemId = dictItem.Id;
            }
            else
            {
                SetValue(propValue.ValueType, propValue.Value);
            }

            return this;
        }

        public virtual void Patch(DynamicPropertyObjectValueEntity target)
        {
            target.Locale = Locale;
            target.LongTextValue = LongTextValue;
            target.BooleanValue = BooleanValue;
            target.DateTimeValue = DateTimeValue;
            target.DecimalValue = DecimalValue;
            target.DictionaryItemId = DictionaryItemId;
            target.IntegerValue = IntegerValue;
            target.ShortTextValue = ShortTextValue;
        }

        public virtual object GetValue(DynamicPropertyValueType valueType)
        {
            if (DictionaryItemId != null)
                return DictionaryItemId;

            switch (valueType)
            {
                case DynamicPropertyValueType.Boolean:
                    return BooleanValue;
                case DynamicPropertyValueType.DateTime:
                    return DateTimeValue;
                case DynamicPropertyValueType.Decimal:
                    return DecimalValue;
                case DynamicPropertyValueType.Integer:
                    return IntegerValue;
                case DynamicPropertyValueType.ShortText:
                    return ShortTextValue;
                default:
                    return LongTextValue;
            }
        }

        public virtual void SetValue(DynamicPropertyValueType valueType, object value)
        {
            switch (valueType)
            {
                case DynamicPropertyValueType.ShortText:
                    //Need to implicit cast to string because there is may be null values
                    ShortTextValue = (string)value;
                    break;
                case DynamicPropertyValueType.Decimal:
                    DecimalValue = value.ToNullable<decimal>();
                    break;
                case DynamicPropertyValueType.DateTime:
                    DateTimeValue = value.ToNullable<DateTime>();
                    break;
                case DynamicPropertyValueType.Boolean:
                    BooleanValue = value.ToNullable<bool>();
                    break;
                case DynamicPropertyValueType.Integer:
                    IntegerValue = value.ToNullable<int>();
                    break;
                default:
                    LongTextValue = (string)value;
                    break;
            }
        }
    }
}
