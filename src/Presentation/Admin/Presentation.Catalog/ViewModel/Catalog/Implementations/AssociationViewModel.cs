using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class AssociationViewModel : ViewModelBase, IAssociationViewModel
	{
		#region Dependencies

		private readonly IViewModelsFactory<ISearchItemViewModel> _searchVmFactory;

		#endregion

		public AssociationViewModel(IViewModelsFactory<ISearchItemViewModel> searchVmFactory, Association item, IItemViewModel parent)
		{
			InnerItem = item;
			_searchVmFactory = searchVmFactory;

			AvailableAssociationTypes = new[] { AssociationTypes.optional.ToString(), AssociationTypes.required.ToString()};

			ItemPickCommand = new DelegateCommand(RaiseItemPickInteractionRequest);
			CommonConfirmRequest = new InteractionRequest<Confirmation>();
		}

		public string[] AvailableAssociationTypes { get; private set; }

		public DelegateCommand ItemPickCommand { get; private set; }
		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }

		#region IPropertyAttributeViewModel

		public Association InnerItem { get; private set; }

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
				return InnerItem.CatalogItem == null ? string.Empty : InnerItem.CatalogItem.Name;
			}
		}
		#endregion

		private void RaiseItemPickInteractionRequest()
		{
			var itemVM = _searchVmFactory.GetViewModelInstance(
				new System.Collections.Generic.KeyValuePair<string, object>("catalogInfo", string.Empty)
				, new System.Collections.Generic.KeyValuePair<string, object>("searchType", string.Empty)
				);
			CommonConfirmRequest.Raise(
				new ConditionalConfirmation(() => itemVM.SelectedItem != null) { Content = itemVM, Title = "Select an item".Localize() },
				(x) =>
				{
					if (x.Confirmed)
					{
						InnerItem.CatalogItem = itemVM.SelectedItem;
						InnerItem.ItemId = itemVM.SelectedItem.ItemId;
						OnPropertyChanged("DisplayName");
					}
				});
		}
	}
}
