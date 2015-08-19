using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class DynamicPropertyObjectValueEntity : AuditableEntity
    {
        public const string TypeShortText = "ShortText";
        public const string TypeLongText = "LongText";
        public const string TypeInteger = "Integer";
        public const string TypeDecimal = "Decimal";
        public const string TypeBoolean = "Boolean";
        public const string TypeDateTime = "DateTime";
        public const string TypeHtml = "Html";

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

        public string PropertyId { get; set; }
        public virtual DynamicPropertyEntity Property { get; set; }

        public string DictionaryItemId { get; set; }
        public virtual DynamicPropertyDictionaryItemEntity DictionaryItem { get; set; }


        public object RawValue()
        {
            if (DictionaryItemId != null)
                return DictionaryItemId;

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
                case TypeHtml:
                    return LongTextValue;
                case TypeShortText:
                    return ShortTextValue;
                default:
                    return null;
            }
        }

        public string ToString(IFormatProvider formatProvider)
        {
            if (DictionaryItemId != null)
                return DictionaryItemId;

            switch (ValueType)
            {
                case TypeBoolean:
                    return BooleanValue == null ? null : BooleanValue.Value.ToString();
                case TypeDateTime:
                    return DateTimeValue == null ? null : DateTimeValue.Value.ToString("O", formatProvider);
                case TypeDecimal:
                    return DecimalValue == null ? null : DecimalValue.Value.ToString(formatProvider);
                case TypeInteger:
                    return IntegerValue == null ? null : IntegerValue.Value.ToString(formatProvider);
                case TypeLongText:
                case TypeHtml:
                    return LongTextValue;
                case TypeShortText:
                    return ShortTextValue;
                default:
                    return base.ToString();
            }
        }
    }
}
