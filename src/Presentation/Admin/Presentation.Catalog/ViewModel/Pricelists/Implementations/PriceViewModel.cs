using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Implementations
{
	public class PriceViewModel : ViewModelBase, IPriceViewModel
	{
		#region Dependencies

		private readonly IRepositoryFactory<IPricelistRepository> _repositoryFactory;
		private readonly IEnumerable _preloadedItems;
		private readonly IViewModelsFactory<ISearchItemViewModel> _searchVmFactory;

		#endregion

		/// <param name="searchVmFactory"></param>
		/// <param name="item"></param>
		/// <param name="preloadedItems">preloaded prices against which the validation will be done.</param>
		/// <param name="isAllFieldsVisible"></param>
		/// <param name="repositoryFactory"></param>
		public PriceViewModel(Price item,
			bool isAllFieldsVisible,
			IEnumerable preloadedItems,
			IViewModelsFactory<ISearchItemViewModel> searchVmFactory,
			IRepositoryFactory<IPricelistRepository> repositoryFactory)
		{
			_innerItem = item;
			_preloadedItems = preloadedItems;
			IsAllFieldsVisible = isAllFieldsVisible;
			_searchVmFactory = searchVmFactory;
			_repositoryFactory = repositoryFactory;

			IsItemChangeable = string.IsNullOrEmpty(item.ItemId);

			ItemPickCommand = new DelegateCommand(RaiseItemPickInteractionRequest);
			CommonConfirmRequest = new InteractionRequest<Confirmation>();
		}

		public bool IsAllFieldsVisible { get; private set; }
		public bool IsItemChangeable { get; private set; }

		public DelegateCommand ItemPickCommand { get; private set; }
		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }

		#region IPropertyAttributeViewModel

		private readonly Price _innerItem;
		public Price InnerItem
		{
			get
			{
				return _innerItem;
			}
		}

		/// <summary>
		/// Validates this instance. Special validation case: item/quantity combination should be unique.
		/// </summary>
		/// <returns></returns>
		public bool Validate()
		{
			bool result;

			InnerItem.Validate();
			if (InnerItem.List <= 0)
			{
				InnerItem.SetError("List", "List Price must be greater than 0".Localize(), true);
			}
			else
			{
				InnerItem.ClearError("List");
			}

			// validate duplicates in preloaded prices
			var preloadedPrices = (ObservableCollection<Price>)_preloadedItems;
			result = !preloadedPrices.Any(x => x.PriceId != InnerItem.PriceId && x.ItemId == InnerItem.ItemId && x.MinQuantity == InnerItem.MinQuantity);

			if (result)
			{
				using (var repository = _repositoryFactory.GetRepositoryInstance())
				{
					// query for price duplicate in repository
					var priceFromRepository = repository.Prices.Where(x => x.PricelistId == InnerItem.PricelistId && x.PriceId != InnerItem.PriceId && x.ItemId == InnerItem.ItemId && x.MinQuantity == InnerItem.MinQuantity).FirstOrDefault();
					if (priceFromRepository != null)
					{
						// check if duplicate price in repository was updated locally
						var localPriceExistsAndItsValid = preloadedPrices.Any(x => x.PriceId == priceFromRepository.PriceId);
						if (!localPriceExistsAndItsValid)
						{
							// duplicate price exists in repository but not locally
							result = false;
						}
					}
				}
			}

			if (result)
			{
				InnerItem.ClearError("MinQuantity");
			}
			else
			{
				InnerItem.SetError("MinQuantity", "This pricelist already contains this item with the same quantity".Localize(), true);
			}

			result = InnerItem.Errors.Count == 0;
			return result;
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
			var parameters = new List<KeyValuePair<string, object>>
		        {
			        new KeyValuePair<string, object>("catalogInfo", string.Empty),
			        new KeyValuePair<string, object>("searchType", string.Empty)
		        };
			var itemVM = _searchVmFactory.GetViewModelInstance(parameters.ToArray());

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
