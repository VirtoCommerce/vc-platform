using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

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
        public const string TypeSecureString = "SecureString";
        public const string TypeJson = "Json";

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


        public virtual SettingValueEntity FromString(string value, SettingValueType valueType)
        {
            this.ValueType = valueType.ToString();

            if (valueType == SettingValueType.Boolean)
            {
                this.BooleanValue = Convert.ToBoolean(value);
            }
            else if (valueType == SettingValueType.DateTime)
            {
                this.DateTimeValue = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
            }
            else if (valueType == SettingValueType.Decimal)
            {
                this.DecimalValue = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
            }
            else if (valueType == SettingValueType.Integer)
            {
                this.IntegerValue = Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            else if (valueType == SettingValueType.LongText)
            {
                this.LongTextValue = value;
            }
            else if (valueType == SettingValueType.Json)
            {
                this.LongTextValue = value;
            }
            else if (valueType == SettingValueType.SecureString)
            {
                this.ShortTextValue = value;
            }
            else
            {
                this.ShortTextValue = value;
            }
            return this;
        }


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
                case TypeJson:
                    return LongTextValue;
                case TypeShortText:
                case TypeSecureString:
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
                case TypeJson:
                    return LongTextValue;
                case TypeShortText:
                case TypeSecureString:
                    return ShortTextValue;
                default:
                    return base.ToString();
            }
        }
    }
}
