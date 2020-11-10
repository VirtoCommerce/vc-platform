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
            // Todo update to switch from C# 9.0 with or feature
            switch (EnumUtility.SafeParse(ValueType, SettingValueType.LongText))
            {
                case SettingValueType.Boolean:
                    return BooleanValue;

                case SettingValueType.DateTime:
                    return DateTimeValue;

                case SettingValueType.Decimal:
                    return DecimalValue;

                case SettingValueType.Integer:
                case SettingValueType.PositiveInteger:
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

            // Todo update to switch from C# 9.0 with or feature
            if (valueType == SettingValueType.Boolean)
            {
                BooleanValue = Convert.ToBoolean(value);
            }
            else if (valueType == SettingValueType.DateTime)
            {
                DateTimeValue = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
            }
            else if (valueType == SettingValueType.Decimal)
            {
                DecimalValue = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
            }
            else if (valueType == SettingValueType.Integer)
            {
                IntegerValue = Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            else if (valueType == SettingValueType.PositiveInteger)
            {
                IntegerValue = Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            else if (valueType == SettingValueType.LongText)
            {
                LongTextValue = Convert.ToString(value);
            }
            else if (valueType == SettingValueType.Json)
            {
                LongTextValue = Convert.ToString(value);
            }
            else if (valueType == SettingValueType.SecureString)
            {
                ShortTextValue = Convert.ToString(value);
            }
            else
            {
                ShortTextValue = Convert.ToString(value);
            }

            ShortTextValue = string.IsNullOrWhiteSpace(ShortTextValue) ? null : ShortTextValue;
            LongTextValue = string.IsNullOrWhiteSpace(LongTextValue) ? null : LongTextValue;

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
                case SettingValueType.PositiveInteger:
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
