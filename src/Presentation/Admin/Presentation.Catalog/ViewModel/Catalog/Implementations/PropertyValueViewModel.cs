using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using catalogModel = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class PropertyValueViewModel : ViewModelBase, IPropertyValueViewModel
	{
		public PropertyValueViewModel(PropertyValue item, IPropertyViewModel parent)
		{
			InnerItem = item;
			ParentCatalog = parent.ParentCatalog;
			IsMultiLanguage = parent.InnerItem.IsLocaleDependant;

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

		// public Property ParentProperty { get; private set; }
		public bool IsMultiLanguage { get; private set; }
		public catalogModel.Catalog ParentCatalog { get; private set; }

		#region IPropertyValueViewModel

		public PropertyValue InnerItem { get; private set; }

		public bool Validate()
		{
			InnerItem.Validate();

			var itemValueType = (PropertyValueType)InnerItem.ValueType;
			switch (itemValueType)
			{
				case PropertyValueType.ShortString:
					if (string.IsNullOrEmpty(InnerItem.ShortTextValue))
						InnerItem.SetError("ShortTextValue", "Value is required".Localize(), true);
					// check for duplicates
					//else if (_parent.InnerItem.Values.Any(x => x.ValueType == InnerItem.ValueType))
					//    InnerItem.SetError("ShortTextValue", "Value is required", true);
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
				//case PropertyValueType.DictionaryKey:
				//default:
				//    break;
			}

		    if (IsMultiLanguage)
		    {
                if (string.IsNullOrEmpty(InnerItem.Locale))
                    InnerItem.SetError("Locale", "Value is required".Localize(), true);
                else
                    InnerItem.ClearError("Locale");
		    }

			//if (string.IsNullOrEmpty(InnerItem.PropertyValueId) || _parent.InnerItem.Values.Any(x => x.PropertyValueId == InnerItem.PropertyValueId))
			//    InnerItem.SetError("PropertyValueId", "Unique value required", true);
			//else
			//    InnerItem.ClearError("PropertyValueId");

			return InnerItem.Errors.Count == 0;
		}

		#endregion

	}
}
