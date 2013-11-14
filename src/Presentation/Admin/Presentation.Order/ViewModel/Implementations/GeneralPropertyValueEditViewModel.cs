using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Order.Model.Settings;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Implementations
{
    public class GeneralPropertyValueEditViewModel : ViewModelBase
    {
        public GeneralPropertyValueEditViewModel(GeneralPropertyValue item)
        {
            InnerItem = item;

            var itemValueType = (GeneralPropertyValueType)InnerItem.ValueType;
            IsShortStringValue = itemValueType == GeneralPropertyValueType.ShortString;
            IsBooleanValue = itemValueType == GeneralPropertyValueType.Boolean;
            IsDictionary = itemValueType == GeneralPropertyValueType.DictionaryKey;
            //IsLongStringValue = itemValueType == GeneralPropertyValueType.LongString;
            //IsDecimalValue = itemValueType == GeneralPropertyValueType.Decimal;
            //IsIntegerValue = itemValueType == GeneralPropertyValueType.Integer;
            //IsDateTimeValue = itemValueType == GeneralPropertyValueType.DateTime;
        }

        public bool IsShortStringValue { get; private set; }
        public bool IsBooleanValue { get; private set; }
        public bool IsDictionary { get; private set; }
        //public bool IsLongStringValue { get; private set; }
        //public bool IsDecimalValue { get; private set; }
        //public bool IsIntegerValue { get; private set; }
        //public bool IsDateTimeValue { get; private set; }

        public GeneralPropertyValue InnerItem { get; private set; }

        public bool Validate()
        {
            // InnerItem.Validate(true);
            return true;
        }
    }
}
