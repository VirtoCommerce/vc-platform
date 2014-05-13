using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class CategoryItemRelationViewModel : ViewModelBase, ICategoryItemRelationViewModel
	{
		private readonly IViewModelsFactory<ISearchCategoryViewModel> _vmFactory;

		public CategoryItemRelationViewModel(IViewModelsFactory<ISearchCategoryViewModel> vmFactory, CategoryItemRelation item)
		{
			_vmFactory = vmFactory;
			InnerItem = item;

			CategorySearchCommand = new DelegateCommand(RaiseItemPickInteractionRequest);
			CommonConfirmRequest = new InteractionRequest<Confirmation>();
		}

		public DelegateCommand CategorySearchCommand { get; private set; }
		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }

		#region IPropertyAttributeViewModel

		public CategoryItemRelation InnerItem { get; private set; }

		public bool Validate()
		{
			return InnerItem.Validate();
		}

		#endregion

		#region ViewModelBase overrides
		public override string DisplayName
		{
			get
			{
				return InnerItem.Category == null ? string.Empty : ((Category)InnerItem.Category).Name;
			}
		}
		#endregion


		private void RaiseItemPickInteractionRequest()
		{
			var itemVM = _vmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("catalogInfo", (object)InnerItem.Catalog ?? InnerItem.CatalogId)
				);
			// itemVM.SearchModifier = none;
			itemVM.InitializeForOpen();
			CommonConfirmRequest.Raise(
				new ConditionalConfirmation(() => itemVM.SelectedItem != null) { Content = itemVM, Title = "Select a Category".Localize() },
				(x) =>
				{
					if (x.Confirmed)
					{
						var item = itemVM.SelectedItem;
						InnerItem.Category = item;
						InnerItem.CategoryId = item.CategoryId;
						InnerItem.CatalogId = item.CatalogId;

						OnPropertyChanged("DisplayName");
					}
				});
		}
	}
}
