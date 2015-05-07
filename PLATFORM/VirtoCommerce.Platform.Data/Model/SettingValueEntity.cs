using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class SettingValueEntity : AuditableEntity
    {
        public const string TypeShortText = "ShortText";
        public const string TypeLongText = "LongText";
        public const string TypeInteger = "Integer";
        public const string TypeDecimal = "Decimal";
        public const string TypeBoolean = "Boolean";
        public const string TypeDateTime = "DateTime";

        [Required]
        [StringLength(64)]
        public string ValueType { get; set; }

        [StringLength(512)]
        public string ShortTextValue { get; set; }

        public string LongTextValue { get; set; }
        public decimal DecimalValue { get; set; }
        public int IntegerValue { get; set; }
        public bool BooleanValue { get; set; }

        public DateTime? DateTimeValue { get; set; }


        [StringLength(64)]
        public string Locale { get; set; }

        public string SettingId { get; set; }
        public virtual SettingEntity Setting { get; set; }


        public object RawValue()
        {
            switch (ValueType)
            {
                case TypeBoolean:
                    return BooleanValue;
                case TypeDateTime:
                    return DateTimeValue;
                case TypeDecimal:
                    return DecimalValue;
                case TypeInteger:
                    return IntegerValue;
                case TypeLongText:
                    return LongTextValue;
                case TypeShortText:
                    return ShortTextValue;
                default:
                    return null;
            }
        }

        public string ToString(IFormatProvider formatProvider)
        {
            switch (ValueType)
            {
                case TypeBoolean:
                    return BooleanValue.ToString();
                case TypeDateTime:
                    return DateTimeValue == null ? null : DateTimeValue.Value.ToString(formatProvider);
                case TypeDecimal:
                    return DecimalValue.ToString(formatProvider);
                case TypeInteger:
                    return IntegerValue.ToString(formatProvider);
                case TypeLongText:
                    return LongTextValue;
                case TypeShortText:
                    return ShortTextValue;
                default:
                    return base.ToString();
            }
        }
    }
}
