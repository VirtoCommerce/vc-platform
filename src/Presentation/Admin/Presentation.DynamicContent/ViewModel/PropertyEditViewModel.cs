using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using catalogModel = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel
{
	public class PropertyEditViewModel : ViewModelBase, IPropertyEditViewModel
	{
		#region Dependencies

		private readonly IViewModelsFactory<IPickAssetViewModel> _pickAssetVmFactory;
		private readonly IViewModelsFactory<ISearchCategoryViewModel> _searchCategoryVmFactory;

		#endregion

		public PropertyEditViewModel(
			IViewModelsFactory<IPickAssetViewModel> pickAssetVmFactory,
			IViewModelsFactory<ISearchCategoryViewModel> searchCategoryVmFactory,
			DynamicContentItemProperty item)
		{
			_pickAssetVmFactory = pickAssetVmFactory;
			_searchCategoryVmFactory = searchCategoryVmFactory;

			InnerItem = item;

			var itemValueType = (PropertyValueType)InnerItem.ValueType;
			IsShortStringValue = itemValueType == PropertyValueType.ShortString;
			IsLongStringValue = itemValueType == PropertyValueType.LongString;
			IsDecimalValue = itemValueType == PropertyValueType.Decimal;
			IsIntegerValue = itemValueType == PropertyValueType.Integer;
			IsBooleanValue = itemValueType == PropertyValueType.Boolean;
			IsDateTimeValue = itemValueType == PropertyValueType.DateTime;
			IsAssetValue = itemValueType == PropertyValueType.Image;
			IsCategoryValue = itemValueType == PropertyValueType.Category;

			if (IsAssetValue)
				SelectedAssetDisplayName = InnerItem.LongTextValue;

			if (IsCategoryValue)
				SelectedCategoryName = InnerItem.Alias;

			AssetPickCommand = new DelegateCommand(RaiseItemPickInteractionRequest);
			CategoryPickCommand = new DelegateCommand(RaiseCategoryPickInteractionRequest);
			CommonConfirmRequest = new InteractionRequest<Confirmation>();
		}

		public bool IsShortStringValue { get; private set; }
		public bool IsLongStringValue { get; private set; }
		public bool IsDecimalValue { get; private set; }
		public bool IsIntegerValue { get; private set; }
		public bool IsBooleanValue { get; private set; }
		public bool IsDateTimeValue { get; private set; }
		public bool IsAssetValue { get; private set; }
		public bool IsCategoryValue { get; private set; }

		public DelegateCommand AssetPickCommand { get; private set; }
		public DelegateCommand CategoryPickCommand { get; private set; }
		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }

		#region IPropertyAttributeViewModel

		public DynamicContentItemProperty InnerItem { get; private set; }

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
				case PropertyValueType.Image:
					break;
				case PropertyValueType.Category:
					break;
			}

			return InnerItem.Errors.Count == 0;
		}

		#endregion

		#region ViewModelBase overrides
		public override string DisplayName
		{
			get
			{
				return InnerItem.LongTextValue;
			}
		}
		#endregion

		public string SelectedCategoryName { get; set; }
		public string SelectedAssetDisplayName { get; set; }

		private void RaiseItemPickInteractionRequest()
		{
			var itemVM = _pickAssetVmFactory.GetViewModelInstance();
            itemVM.AssetPickMode = true;
            itemVM.RootItemId = null;

			CommonConfirmRequest.Raise(
                new ConditionalConfirmation(() => itemVM.SelectedAsset != null) { Content = itemVM, Title = "Select an asset".Localize() },
				(x) =>
				{
					if (x.Confirmed)
					{
						var file = itemVM.SelectedAsset;
						InnerItem.LongTextValue = file.FolderItemId;
						SelectedAssetDisplayName = file.Name;
						SelectedAssetImageSource = file.SmallData;
						OnPropertyChanged("SelectedAssetDisplayName");
					}
				});
		}

		object _SelectedAssetImageSource;
		public object SelectedAssetImageSource
		{
			get { return _SelectedAssetImageSource; }
			set { _SelectedAssetImageSource = value; OnPropertyChanged(); }
		}

		private void RaiseCategoryPickInteractionRequest()
		{
			var itemVM = _searchCategoryVmFactory.GetViewModelInstance();

			itemVM.SearchModifier = SearchCategoryModifier.UserCanChangeSearchCatalog;
			itemVM.InitializeForOpen();
			CommonConfirmRequest.Raise(
                new ConditionalConfirmation(() => itemVM.SelectedItem != null) { Content = itemVM, Title = "Select Category".Localize() },
				(x) =>
				{
					if (x.Confirmed)
					{
						var category = itemVM.SelectedItem;
						InnerItem.LongTextValue = category.Code;
						InnerItem.Alias = category.Name;
						SelectedCategoryName = InnerItem.Alias;
						OnPropertyChanged("SelectedCategoryName");
					}
				});
		}
	}
}
