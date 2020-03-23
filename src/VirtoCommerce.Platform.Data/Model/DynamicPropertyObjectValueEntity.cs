using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Platform.Data.Model
{
    public abstract class DynamicPropertyObjectValueEntity : AuditableEntity
    {
        [StringLength(256)]
        public string ObjectType { get; set; }

        [StringLength(128)]
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

        [StringLength(128)]
        public string PropertyId { get; set; }
        [StringLength(128)]
        public string DictionaryItemId { get; set; }
        [StringLength(256)]
        public string PropertyName { get; set; }

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
            propValue.PropertyId = PropertyId;
            propValue.PropertyName = PropertyName;

            if (!string.IsNullOrEmpty(DictionaryItemId))
            {
                propValue.ValueId = DictionaryItemId;
                propValue.Value = ShortTextValue;
            }
            else
            {
                propValue.Value = GetValue(propValue.ValueType);
            }
            return propValue;
        }

        public virtual DynamicPropertyObjectValueEntity FromModel(DynamicPropertyObjectValue propValue, IHasDynamicProperties owner, DynamicProperty dynamicProperty)
        {
            if (propValue == null)
            {
                throw new ArgumentNullException(nameof(propValue));
            }

            Locale = propValue.Locale;
            ObjectId = propValue.ObjectId ?? owner.Id;
            ObjectType = propValue.ObjectType ?? owner.ObjectType ?? owner.GetType().FullName;
            var dynamicPropertyValueType = propValue.ValueType == DynamicPropertyValueType.Undefined ? dynamicProperty.ValueType : propValue.ValueType;
            ValueType = dynamicPropertyValueType.ToString();
            PropertyId = propValue.PropertyId ?? dynamicProperty.Id;
            PropertyName = propValue.PropertyName ?? dynamicProperty.Name;
            DictionaryItemId = propValue.ValueId;

            var dictItem = propValue.Value as DynamicPropertyDictionaryItem;
            if (dictItem == null && propValue.Value is JObject jObject)
            {
                dictItem = jObject.ToObject<DynamicPropertyDictionaryItem>();
            }

            if (dictItem != null)
            {
                DictionaryItemId = dictItem.Id;
                PropertyId = dictItem.PropertyId;
                ShortTextValue = dictItem.Name;
            }
            else
            {
                SetValue(dynamicPropertyValueType, propValue.Value);
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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (ReferenceEquals(null, obj))
                return false;
            if (GetType() != obj.GetType())
                return false;
            var other = obj as DynamicPropertyObjectValueEntity;
            return other != null && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return GetEqualityComponents().Aggregate(17, (current, obj) => current * 23 + (obj?.GetHashCode() ?? 0));
            }
        }

        protected virtual IEnumerable<object> GetEqualityComponents()
        {
            //Do not use Id for equality calculation because of most of values are passed without Id and this will caused a duplications and necessary deletion and db fragmentation
            yield return PropertyName;
            yield return Locale;
            yield return ObjectId;
            yield return ObjectType;
            yield return ValueType;
            yield return ShortTextValue;
            yield return LongTextValue;
            yield return DecimalValue;
            yield return IntegerValue;
            yield return BooleanValue;
            yield return DateTimeValue;
            yield return DictionaryItemId;

        }

    }
}
