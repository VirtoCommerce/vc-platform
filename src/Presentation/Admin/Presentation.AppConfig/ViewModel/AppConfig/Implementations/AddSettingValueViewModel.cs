using System;
using VirtoCommerce.ManagementClient.AppConfig.Infrastructure.Enumerations;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Implementations
{
	public class AddSettingValueViewModel : ViewModelBase, IAddSettingValueViewModel
    {

        #region Fields

        private Infrastructure.Enumerations.ValueType _valueType;
        private OperationType _operationType;

        #endregion

        #region Constructor

        public AddSettingValueViewModel(VirtoCommerce.ManagementClient.AppConfig.Infrastructure.Enumerations.ValueType valueType, OperationType operationType)
        {
            _valueType = valueType;
            _operationType = operationType;


            switch (_valueType)
            {
                case Infrastructure.Enumerations.ValueType.Boolean:
                    IsBooleanValue = true;
                    OnPropertyChanged("IsBooleanValue");
                    break;

                case Infrastructure.Enumerations.ValueType.DataTime:
                    IsDateTimeValue = true;
                    OnPropertyChanged("IsDateTimeValue");
                    break;

                case Infrastructure.Enumerations.ValueType.Decimal:
                    IsDecimalValue = true;
                    OnPropertyChanged("IsDecimalValue");
                    break;

                case Infrastructure.Enumerations.ValueType.Integer:
                    IsIntegerValue = true;
                    OnPropertyChanged("IsIntegerValue");
                    break;

                case Infrastructure.Enumerations.ValueType.LongText:
                    IsLongStringValue = true;
                    OnPropertyChanged("IsLongStringValue");
                    break;

                case Infrastructure.Enumerations.ValueType.ShortText:
                    IsShortStringValue = true;
                    OnPropertyChanged("IsShortStringValue");
                    break;
            }

        }

        #endregion

        #region Properties

        public Infrastructure.Enumerations.ValueType ValueType
        {
            get { return _valueType; }
            set { _valueType = value; }
        }

        public bool IsShortStringValue { get; set; }

        public bool IsLongStringValue { get; set; }

        public bool IsDecimalValue { get; set; }

        public bool IsIntegerValue { get; set; }

        public bool IsBooleanValue { get; set; }

        public bool IsDateTimeValue { get; set; }


        private string _longTextValue;
        public string LongTextValue
        {
            get { return _longTextValue; }
            set
            {
                _longTextValue = value;
                OnPropertyChanged();
                OnPropertyChanged("IsValid");
            }
        }

        private string _shortTextValue;
        public string ShortTextValue
        {
            get { return _shortTextValue; }
            set
            {
                _shortTextValue = value;
                OnPropertyChanged();
                OnPropertyChanged("IsValid");
            }
        }

        private decimal _decimalValue;
        public decimal DecimalValue
        {
            get { return _decimalValue; }
            set
            {
                _decimalValue = value;
                OnPropertyChanged();
                OnPropertyChanged("IsValid");
            }
        }

        private int _integerValue;
        public int IntegerValue
        {
            get { return _integerValue; }
            set
            {
                _integerValue = value;
                OnPropertyChanged();
                OnPropertyChanged("IsValid");
            }
        }

        private bool _booleanValue;
        public bool BooleanValue
        {
            get { return _booleanValue; }
            set
            {
                _booleanValue = value;
                OnPropertyChanged();
                OnPropertyChanged("IsValid");
            }
        }

        private DateTime? _datetimeValue;
        public DateTime? DateTimeValue
        {
            get { return _datetimeValue; }
            set
            {
                _datetimeValue = value;
                OnPropertyChanged();
                OnPropertyChanged("IsValid");
            }
        }


        public bool IsValid
        {
            get { return IsInputValueValid(); }
        }


        #endregion


        #region Private Methods

        private bool IsInputValueValid()
        {
            bool result = false;

            switch (ValueType)
            {
                    case Infrastructure.Enumerations.ValueType.Boolean:
                    result = true;
                    break;

                    case Infrastructure.Enumerations.ValueType.DataTime:
                    result = true;
                    break;

                    case Infrastructure.Enumerations.ValueType.Decimal:
                    result = true;
                    break;

                    case Infrastructure.Enumerations.ValueType.Integer:
                    result = true;
                    break;

                    case Infrastructure.Enumerations.ValueType.LongText:
                    result = !String.IsNullOrEmpty(LongTextValue);
                    break;

                    case Infrastructure.Enumerations.ValueType.ShortText:
                    result = !string.IsNullOrEmpty(ShortTextValue);
                    break;
            }

            return result;
        }

        #endregion

    }
}
