using System.Linq;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Interfaces;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Implementations
{
    public class StoreSettingViewModel : ViewModelBase, IStoreSettingViewModel
    {
        public const string textShortText = "ShortText",
            textLongText = "LongText",
            textInteger = "Integer",
            textDecimal = "Decimal",
            textBoolean = "Boolean",
            textDateTime = "DateTime",
            textXML = "xml";

        public StoreSettingViewModel(StoreSetting item)
        {
            InnerItem = item;

            AvailableValueTypes = new string[] { textShortText, textLongText, textInteger, textDecimal, textBoolean, textDateTime, textXML };
            IsValueTypeChangeable = AvailableValueTypes.All(x => x != InnerItem.ValueType);

            UpdateInputControlsVisibility();
        }

        public bool IsValueTypeChangeable { get; private set; }
        public bool CanEnterValue { get { return AvailableValueTypes.Any(x => x == InnerItem.ValueType); } }
        public string[] AvailableValueTypes { get; private set; }

        bool _IsShortStringValue;
        public bool IsShortStringValue
        {
            get { return _IsShortStringValue; }
            set { _IsShortStringValue = value; OnPropertyChanged(); }
        }
        bool _IsLongStringValue;
        public bool IsLongStringValue
        {
            get { return _IsLongStringValue; }
            set { _IsLongStringValue = value; OnPropertyChanged(); }
        }
        bool _IsDecimalValue;
        public bool IsDecimalValue
        {
            get { return _IsDecimalValue; }
            set { _IsDecimalValue = value; OnPropertyChanged(); }
        }
        bool _IsIntegerValue;

        public bool IsIntegerValue
        {
            get { return _IsIntegerValue; }
            set { _IsIntegerValue = value; OnPropertyChanged(); }
        }
        bool _IsBooleanValue;
        public bool IsBooleanValue
        {
            get { return _IsBooleanValue; }
            set { _IsBooleanValue = value; OnPropertyChanged(); }
        }
        bool _IsDateTimeValue;
        public bool IsDateTimeValue
        {
            get { return _IsDateTimeValue; }
            set { _IsDateTimeValue = value; OnPropertyChanged(); }
        }

        #region IStoreSettingViewModel

        public StoreSetting InnerItem { get; set; }

        #endregion

        public void UpdateInputControlsVisibility()
        {
            IsShortStringValue = InnerItem.ValueType == textShortText;
            IsLongStringValue = InnerItem.ValueType == textLongText || InnerItem.ValueType == textXML;
            IsDecimalValue = InnerItem.ValueType == textDecimal;
            IsIntegerValue = InnerItem.ValueType == textInteger;
            IsBooleanValue = InnerItem.ValueType == textBoolean;
            IsDateTimeValue = InnerItem.ValueType == textDateTime;
            OnPropertyChanged("CanEnterValue");
        }
    }
}
