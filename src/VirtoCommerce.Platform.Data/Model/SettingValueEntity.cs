using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Model
{
    public class SettingValueEntity : AuditableEntity
    {
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

        public string SettingId { get; set; }
        public virtual SettingEntity Setting { get; set; }

        public object GetValue()
        {
            switch (EnumUtility.SafeParse(ValueType, SettingValueType.LongText))
            {
                case SettingValueType.Boolean:
                    return BooleanValue;
                case SettingValueType.DateTime:
                    return DateTimeValue;
                case SettingValueType.Decimal:
                    return DecimalValue;
                case SettingValueType.Integer:
                    return IntegerValue;
                case SettingValueType.ShortText:
                case SettingValueType.SecureString:
                    return ShortTextValue;
                default:
                    return LongTextValue;
            }
        }

        public SettingValueEntity SetValue(SettingValueType valueType, object value)
        {
            ValueType = valueType.ToString();

            switch (valueType)
            {
                case SettingValueType.Boolean:
                    BooleanValue = Convert.ToBoolean(value);
                    break;
                case SettingValueType.DateTime:
                    DateTimeValue = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                    break;
                case SettingValueType.Decimal:
                    DecimalValue = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                    break;
                case SettingValueType.Integer:
                    IntegerValue = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                    break;
                case SettingValueType.LongText:
                case SettingValueType.Json:
                    LongTextValue = Convert.ToString(value);
                    break;
                default:
                    // SettingValueType.ShortText
                    // SettingValueType.SecureString
                    ShortTextValue = Convert.ToString(value);
                    break;
            }

            return this;
        }

        public string ToString(SettingValueType valueType, IFormatProvider formatProvider)
        {
            switch (valueType)
            {
                case SettingValueType.Boolean:
                    return BooleanValue.ToString();
                case SettingValueType.DateTime:
                    return DateTimeValue == null ? null : DateTimeValue.Value.ToString(formatProvider);
                case SettingValueType.Decimal:
                    return DecimalValue.ToString(formatProvider);
                case SettingValueType.Integer:
                    return IntegerValue.ToString(formatProvider);
                case SettingValueType.LongText:
                case SettingValueType.Json:
                    return LongTextValue;
                case SettingValueType.ShortText:
                case SettingValueType.SecureString:
                    return ShortTextValue;
                default:
                    return base.ToString();
            }
        }
    }
}
