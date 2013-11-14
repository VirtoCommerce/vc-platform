using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.AppConfig.Model
{
    public class SettingValueModel : NotifyPropertyChanged
    {
        public SettingValueModel(object item = null)
        {
            if (item != null)
            {
                this.InjectFrom(item);

                var propInfo = item.GetType().FindPropertiesWithAttribute(typeof(KeyAttribute)).First();
                SettingValueId = propInfo.GetValue(item) as string ?? Guid.NewGuid().ToString();
            }
        }

        private string _settingValueId;
        [Key]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string SettingValueId
        {
            get
            {
                return _settingValueId;
            }
            set
            {
                _settingValueId = value;
                OnPropertyChanged();
            }
        }

        private string _valueType;
        [StringLength(64)]
        public string ValueType
        {
            get
            {
                return _valueType;
            }
            set
            {
                _valueType= value;
                OnPropertyChanged();
            }
        }

        private string _shortTextValue;
        public string ShortTextValue
        {
            get
            {
                return _shortTextValue;
            }
            set
            {
                _shortTextValue= value;
                OnPropertyChanged();
            }
        }

        private string _longTextValue;
        public string LongTextValue
        {
            get
            {
                return _longTextValue;
            }
            set
            {
                _longTextValue= value;
                OnPropertyChanged();
            }
        }

        private decimal _decimalValue;
        public decimal DecimalValue
        {
            get
            {
                return _decimalValue;
            }
            set
            {
                _decimalValue = value;
                OnPropertyChanged();
            }
        }

        private int _integerValue;
        public int IntegerValue
        {
            get
            {
                return _integerValue;
            }
            set
            {
                _integerValue = value;
                OnPropertyChanged();
            }
        }

        private bool _booleanValue;
        public bool BooleanValue
        {
            get
            {
                return _booleanValue;
            }
            set
            {
                _booleanValue = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _dateTimeValue;
        public DateTime? DateTimeValue
        {
            get
            {
                return _dateTimeValue;
            }
            set
            {
                _dateTimeValue = value;
                OnPropertyChanged();
            }
        }

        private string _locale;
        [StringLength(64)]
        public string Locale
        {
            get
            {
                return _locale;
            }
            set
            {
                _locale = value;
                OnPropertyChanged();
            }
        }

        private string _settingId;

        [StringLength(128)]
        public string SettingId
        {
            get
            {
                return _settingId;
            }
            set
            {
                _settingId = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            switch (this.ValueType)
            {
                case textBoolean:
                    return this.BooleanValue.ToString();
                case textDateTime:
                    return this.DateTimeValue.ToString();
                case textDecimal:
                    return this.DecimalValue.ToString();
                case textInteger:
                    return this.IntegerValue.ToString();
                case textLongText:
                    return this.LongTextValue;
                case textShortText:
                    return this.ShortTextValue;
            }
            return base.ToString();
        }

        private const string textShortText = "ShortText",
            textLongText = "LongText",
            textInteger = "Integer",
            textDecimal = "Decimal",
            textBoolean = "Boolean",
            textDateTime = "DateTime";

    }
}
