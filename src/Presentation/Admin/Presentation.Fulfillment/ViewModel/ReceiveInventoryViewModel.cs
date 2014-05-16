using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Inventories.Repositories;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;


namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel
{
	public class ReceiveInventoryViewModel : ViewModelBase, IReceiveInventoryViewModel
	{
		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }
		public DelegateCommand<Foundation.Inventories.Model.Inventory> SearchSkuCommand { get; private set; }

		public Foundation.Inventories.Model.Inventory CurrentItem { get; set; }
		public List<FulfillmentCenter> AvailableFulfillments { get; private set; }
		private FulfillmentCenter _selectedFulfillmentCenter;
		public FulfillmentCenter SelectedFulfillmentCenter
		{
			get { return _selectedFulfillmentCenter; }
			set
			{
				_selectedFulfillmentCenter = value;
				OnSpecifiedPropertyChanged("AvailableFulfillments");
				OnPropertyChanged();
			}
		}

		private readonly IRepositoryFactory<IFulfillmentCenterRepository> _fulfillmentRepositoryFactory;
		private readonly IRepositoryFactory<IInventoryRepository> _inventoryRepositoryFactory;
		private readonly IViewModelsFactory<ISearchItemViewModel> _searchItemVmFactory;


		public ReceiveInventoryViewModel(
			IRepositoryFactory<IFulfillmentCenterRepository> fulfillmentRepositoryFactory,
			IRepositoryFactory<IInventoryRepository> inventoryRepositoryFactory,
			IViewModelsFactory<ISearchItemViewModel> searchItemVmFactory
		)
		{
			_fulfillmentRepositoryFactory = fulfillmentRepositoryFactory;
			_inventoryRepositoryFactory = inventoryRepositoryFactory;
			_searchItemVmFactory = searchItemVmFactory;

			Initialize();

			CommonConfirmRequest = new InteractionRequest<Confirmation>();
			SearchSkuCommand = new DelegateCommand<Foundation.Inventories.Model.Inventory>(DoSearchSku);
		}

		private void DoSearchSku(Foundation.Inventories.Model.Inventory sku)
		{
			// rp: Inventory should contain not only products but variations...
			var searchItemVm =
				_searchItemVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("catalogInfo", string.Empty), new KeyValuePair<string, object>("searchType", "Product..."));

			CommonConfirmRequest.Raise(new ConditionalConfirmation
			{
				Content = searchItemVm,
				Title = "Search for item sku".Localize()
			},
			x =>
			{
				if (x.Confirmed && searchItemVm.SelectedItem != null)
				{
					CurrentItem.Sku = searchItemVm.SelectedItem.ItemId;
					OnSpecifiedPropertyChanged("CurrentItem");
				}
			});
		}

		#region IReceiveInventoryViewModel

		public ObservableCollection<Foundation.Inventories.Model.Inventory> InventoryItems
		{
			get;
			private set;
		}

		#endregion

		#region private members
		private void Initialize()
		{
			InventoryItems = new ObservableCollection<Foundation.Inventories.Model.Inventory>();
			using (var fulfillmentRepository = _fulfillmentRepositoryFactory.GetRepositoryInstance())
			{
				AvailableFulfillments = fulfillmentRepository.FulfillmentCenters.OrderBy(x => x.Name).ToList();
			}


			foreach (var item in InventoryItems)
			{
				item.PropertyChanged += Item_PropertyChanged;
			}

			InventoryItems.CollectionChanged -= InventoryItems_CollectionChanged;
			InventoryItems.CollectionChanged += InventoryItems_CollectionChanged;
		}

		private void InventoryItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				var item = e.NewItems[0];
				((Foundation.Inventories.Model.Inventory)item).PropertyChanged += Item_PropertyChanged;
			}
		}

		private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnSpecifiedPropertyChanged("IsInInventoryList");
			OnSpecifiedPropertyChanged("AnyNotInInventoryList");
		}
		#endregion


		public bool IsInInventoryList
		{
			get
			{
				var retVal = false;
				if (InventoryItems.Count > 0 && CurrentItem != null && !String.IsNullOrEmpty(CurrentItem.Sku))
				{
					using (var inventoryRepository = _inventoryRepositoryFactory.GetRepositoryInstance())
					{
						var sku = inventoryRepository.Inventories.ToList().
															  SingleOrDefault(item => item.Sku == CurrentItem.Sku &&
																					  item.FulfillmentCenterId ==
																					  SelectedFulfillmentCenter.FulfillmentCenterId);
						if (sku == null)
							retVal = true;
					}

				}
				return retVal;
			}
		}

		public bool AnyNotInInventoryList
		{
			get
			{
				var retVal = false;
				if (InventoryItems.Count > 0)
					retVal = InventoryItems.Any(x => NotInList(x.Sku));
				return retVal;
			}
		}

		private bool NotInList(string sku)
		{
			bool returnValue;
			using (var inventoryRepository = _inventoryRepositoryFactory.GetRepositoryInstance())
			{
				returnValue = !inventoryRepository.Inventories.ToList()
				.Any(x => x.Sku == sku && x.FulfillmentCenterId == SelectedFulfillmentCenter.FulfillmentCenterId);
			}
			return returnValue;
		}
	}
}
