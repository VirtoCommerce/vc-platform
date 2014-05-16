using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.Fulfillment;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.FulfillmentCenters.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.PickLists.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.PickLists.Implementations
{
	public class PicklistHomeViewModel :
		 ViewModelHomeEditableBase<Picklist>,
		 IPicklistHomeViewModel, IVirtualListLoader<IPicklistViewModel>, ISupportDelayInitialization
	{
		#region Filters

		public string SearchFilterShipmentId { get; set; }

		/// <summary>
		/// For future purpose. Filter values to filter by agents. can be viewed currently logged in agent picklists or if permission is set - view all agents picklists
		/// </summary>
		public enum AgentFilterValues
		{
			My,
			All
		}

		#endregion

		#region Dependencies

		private readonly IAuthenticationContext _authenticationContext;
		private readonly IOrderEntityFactory _entityFactory;
		private readonly IRepositoryFactory<IFulfillmentCenterRepository> _fulfillmentCenterRepositoryFactory;
		private readonly IRepositoryFactory<IFulfillmentRepository> _fulfillmentRepositoryFactory;
		private readonly IViewModelsFactory<IPicklistViewModel> _itemVmFactory;
		private readonly IViewModelsFactory<ICreatePicklistWizardViewModel> _wizardVmFactory;
		#endregion

		#region Constructor

		public PicklistHomeViewModel(
			IViewModelsFactory<IPicklistViewModel> itemVmFactory,
			IViewModelsFactory<ICreatePicklistWizardViewModel> wizardVmFactory,
			IRepositoryFactory<IFulfillmentCenterRepository> fulfillmentCenterRepositoryFactory,
			IRepositoryFactory<IFulfillmentRepository> fulfillmentRepositoryFactory,
			IAuthenticationContext authenticationContext,
			IOrderEntityFactory entityFactory)
		{
			_authenticationContext = authenticationContext;
			_entityFactory = entityFactory;
			_itemVmFactory = itemVmFactory;
			_wizardVmFactory = wizardVmFactory;
			_fulfillmentCenterRepositoryFactory = fulfillmentCenterRepositoryFactory;
			_fulfillmentRepositoryFactory = fulfillmentRepositoryFactory;

			ClearFiltersCommand = new DelegateCommand(() =>
				{
					SearchFilterShipmentId = null;

					OnSpecifiedPropertyChanged("SearchFilterShipmentId");
					OnSpecifiedPropertyChanged("SearchFilterFulfillment");
				});

			CommonNotifyRequest = new InteractionRequest<Notification>();
			SearchFilterFulfillments = new ObservableCollection<FulfillmentCenter>();
		}

		#endregion

		#region Filters

		public FulfillmentCenter SearchFilterFulfillment { get; set; }
		public ObservableCollection<FulfillmentCenter> SearchFilterFulfillments { get; set; }

		#endregion

		#region Commands

		public DelegateCommand ClearFiltersCommand { get; private set; }

		#endregion

		#region ViewModelHomeEditableBase

		protected override bool CanItemAddExecute()
		{
			return _authenticationContext.CheckPermission(PredefinedPermissions.FulfillmentPicklistsManage);
		}

		public VirtualListItem<IPicklistViewModel> SelectedItem { get; set; }

		protected override bool CanItemDeleteExecute(IList x)
		{
			return (_authenticationContext.CheckPermission(PredefinedPermissions.FulfillmentPicklistsManage)) && x != null && x.Count > 0;
		}

		protected override void RaiseItemAddInteractionRequest()
		{
			var item = _entityFactory.CreateEntity<Picklist>();
			var itemVm = _wizardVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item));
			var confirmation = new Confirmation { Content = itemVm, Title = "Create Pick List".Localize() };
			ItemAdd(confirmation);
		}

		protected override void RaiseItemDeleteInteractionRequest(IList selectedItemsList)
		{
		}

		public InteractionRequest<Notification> CommonNotifyRequest { get; private set; }

		private void UpdateFulfillmentCenterFilter()
		{
			//Get all fulfillment centers
			Action getAllFulfillmentsAction = () =>
			{
				using (var repository = _fulfillmentCenterRepositoryFactory.GetRepositoryInstance())
				{
					var fulfillments = repository.FulfillmentCenters.ToList();
					OnUIThread(() =>
						{
							SearchFilterFulfillments.SetItems(fulfillments);
							if (SearchFilterFulfillments.Count > 0)
								SearchFilterFulfillment = SearchFilterFulfillments[0];
							OnSpecifiedPropertyChanged("SearchFilterFulfillment");
						});
				}
			};
			getAllFulfillmentsAction.BeginInvoke(null, null);
		}

		#endregion

		#region IPicklistHomeViewModel members

		public IMainFulfillmentViewModel ParentViewModel { get; set; }

		#endregion

		#region IVirtualListLoader<IPicklistViewModel> Members

		public bool CanSort
		{
			get { return false; }
		}

		public IList<IPicklistViewModel> LoadRange(int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount)
		{
			var retVal = new List<IPicklistViewModel>();

			using (var repository = _fulfillmentRepositoryFactory.GetRepositoryInstance())
			{
				var canQuery = false;
				var query = repository.Picklists
					.Where(item =>
							item.Shipments.Any(shipment => shipment.Status != ShipmentStatus.Shipped.ToString()));

				if (SearchFilterFulfillment != null && !string.IsNullOrEmpty(SearchFilterFulfillment.FulfillmentCenterId))
				{
					canQuery = true;
					query = query.Where(x => x.FulfillmentCenterId == SearchFilterFulfillment.FulfillmentCenterId);
				}

				if (SearchFilterShipmentId != null)
				{
					query = query.Where(x => x.Shipments.Any(ship => ship.ShipmentId.Contains(SearchFilterShipmentId)));
				}

				if (canQuery)
				{
					overallCount = query.Count();
					var l = query.OrderByDescending(x => x.Created)
						.Skip(startIndex).Take(count)
						.Expand("Shipments/OrderForm/OrderGroup/OrderAddresses,Shipments/ShipmentItems/LineItem")
						.ToList();

					retVal.AddRange(l.Select(i => _itemVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", i))));
				}
				else
				{
					overallCount = 0;
				}
			}

			return retVal;
		}

		#endregion

		#region ISupportDelayInitialization Members

		public void InitializeForOpen()
		{
			if (SearchFilterFulfillments.Count == 0)
			{
				UpdateFulfillmentCenterFilter();
			}

			if (ListItemsSource == null)
			{
				OnUIThread(() => ListItemsSource = new VirtualList<IPicklistViewModel>(this, 25, SynchronizationContext.Current));
			}
		}

		#endregion

	}
}
