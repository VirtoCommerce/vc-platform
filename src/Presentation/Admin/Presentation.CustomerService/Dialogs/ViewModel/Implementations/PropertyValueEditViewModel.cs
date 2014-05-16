using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Implementations
{
	public class PropertyValueEditViewModel : ViewModelBase
	{
		public PropertyValueEditViewModel(PropertyValueBase item)
		{
			InnerItem = item;

			var itemValueType = (PropertyValueType)InnerItem.ValueType;
			IsShortStringValue = itemValueType == PropertyValueType.ShortString;
			IsLongStringValue = itemValueType == PropertyValueType.LongString;
			IsDecimalValue = itemValueType == PropertyValueType.Decimal;
			IsIntegerValue = itemValueType == PropertyValueType.Integer;
			IsBooleanValue = itemValueType == PropertyValueType.Boolean;
			IsDateTimeValue = itemValueType == PropertyValueType.DateTime;
		}

		public bool IsShortStringValue { get; private set; }
		public bool IsLongStringValue { get; private set; }
		public bool IsDecimalValue { get; private set; }
		public bool IsIntegerValue { get; private set; }
		public bool IsBooleanValue { get; private set; }
		public bool IsDateTimeValue { get; private set; }

		public PropertyValueBase InnerItem { get; private set; }

		public bool Validate()
		{
			InnerItem.Validate(true);

			var itemValueType = (PropertyValueType)InnerItem.ValueType;
			switch (itemValueType)
			{
				case PropertyValueType.ShortString:
					if (string.IsNullOrEmpty(InnerItem.ShortTextValue))
						InnerItem.SetError("ShortTextValue", "Value is required".Localize(), true);
					else
						InnerItem.ClearError("ShortTextValue");
					break;
				case PropertyValueType.LongString:
					if (string.IsNullOrEmpty(InnerItem.LongTextValue))
						InnerItem.SetError("LongTextValue", "Value is required".Localize(), true);
					else
						InnerItem.ClearError("LongTextValue");
					break;
				case PropertyValueType.Decimal:
					break;
				case PropertyValueType.Integer:
					break;
				case PropertyValueType.Boolean:
					break;
				case PropertyValueType.DateTime:
					break;
			}

			return InnerItem.Errors.Count == 0;
		}
	}


	public class PropertyValueWithFieldNameEditViewModel : PropertyValueEditViewModel
	{
		public PropertyValueWithFieldNameEditViewModel(PropertyValueBase item)
			: base(item)
		{
			Priority = item.Priority;
		}

		public string FieldName { get; set; }
		public int Priority { get; private set; }
	}
}
