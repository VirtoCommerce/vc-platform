using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using System.Threading;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.FulfillmentCenters.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Rmas.Interfaces;
using domainModel = VirtoCommerce.Foundation.Orders.Model.Fulfillment;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using System.Threading.Tasks;
using PropertyChanged;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Rmas.Implementations
{
	[ImplementPropertyChanged]
    public class RmaHomeViewModel :
		ViewModelHomeEditableBase<RmaRequest>, 
		IRmaHomeViewModel, IVirtualListLoader<IRmaViewModel>, ISupportDelayInitialization
	{
		#region const
		const string SettingNameReturnReasons = "ReturnReasons";
		#endregion 

		#region SearchFields
		public string SearchFilterAuthorizationCode { get; set; }
		public string SearchFilterItemCode { get; set; }
		public string SearchFilterItemName { get; set; }
		public string SearchFilterReason { get; set; }
		public string SearchFilterOrderTrackingNumber { get; set; }
		public string SearchFilterCustomerName { get; set; }
		public object SearchFilterStatus { get; set; }
		#endregion

		#region Properties
		
		public string[] AvailableReasons
		{
			get;
			private set;
		}

		#endregion

		#region Dependencies

		private readonly IRepositoryFactory<IAppConfigRepository> _appConfigRepositoryFactory;
	    private readonly IRepositoryFactory<IOrderRepository> _orderRepositoryFactory;
	    private readonly IViewModelsFactory<IRmaViewModel> _rmaVmFactory;

		#endregion

		#region Constructor
		public RmaHomeViewModel(
			IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
			IRepositoryFactory<IOrderRepository> orderRepositoryFactory,
			IViewModelsFactory<IRmaViewModel> rmaVmFactory
			)
		{
			_rmaVmFactory = rmaVmFactory;
			_orderRepositoryFactory = orderRepositoryFactory;
			_appConfigRepositoryFactory = appConfigRepositoryFactory;
			CommonNotifyRequest = new InteractionRequest<Notification>();

			ClearFiltersCommand = new DelegateCommand(DoClearFilters);
			Initialize();

			// rp: commented before me
			//Get all fulfillment centers
			//Action getAllFulfillmentsAction = () =>
			//{
			//	using (var repository = _fulfillmentCenterRepository)
			//	{
			//		var fulfillments = repository.FulfillmentCenters.ToList();
			//		OnUIThread(() => { EndSearchFulfillments(fulfillments); return true; });
			//	}
			//};
			//getAllFulfillmentsAction.BeginInvoke(null, null);
		}

		private async void Initialize()
		{
			AvailableReasons = await Task.Run(() =>
			{
				string[] result = null;
				using (var settingsRepository = _appConfigRepositoryFactory.GetRepositoryInstance())
				{
				    var setting = settingsRepository.Settings.Where(x => x.Name == SettingNameReturnReasons).ExpandAll().SingleOrDefault();
				    if (setting != null)
				   {
					   result = setting.SettingValues.Select(x => x.ShortTextValue).ToArray();
				   }
				}
				return result;
			});

			SearchFilterStatus = RmaRequestStatus.AwaitingStockReturn;
		}
		#endregion
		
		#region Commands

		public DelegateCommand ClearFiltersCommand { get; private set; }

		#endregion

		#region Requests
		public InteractionRequest<Notification> CommonNotifyRequest { get; private set; }
		#endregion

		#region IRmaHomeViewModel members

		public IMainFulfillmentViewModel ParentViewModel { get; set; }

		#endregion

		#region IVirtualListLoader<IRmaViewModel> Members

		public bool CanSort
		{
			get { return false; }
		}

		public IList<IRmaViewModel> LoadRange(int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount)
		{
			var retVal = new List<IRmaViewModel>();

			using (var repository = _orderRepositoryFactory.GetRepositoryInstance())
			{
				var query = repository.RmaRequests.Expand("RmaReturnItems/RmaLineItems/LineItem,Order");
					//.Where(rma => rma.RmaReturnItems.Any(item => item.ItemState == "AwaitingReturn"));// == Enum.GetName(typeof(RmaRequestStatus), RmaRequestStatus.AwaitingStockReturn));

				if (!string.IsNullOrEmpty(SearchFilterAuthorizationCode))
				{
					query = query.Where(x => x.AuthorizationCode.Contains(SearchFilterAuthorizationCode));
				}

				if (!string.IsNullOrEmpty(SearchFilterItemName))
				{
					query =
						query.Where(
							x =>
							x.RmaReturnItems.Any(
								rItem => rItem.RmaLineItems.Any(lItem => lItem.LineItem.DisplayName.Contains(SearchFilterItemName))));
				}

				if (SearchFilterStatus is RmaRequestStatus)
				{
					query = query.Where(x => x.Status == Enum.GetName(typeof (RmaRequestStatus), SearchFilterStatus));
				}

				if (!string.IsNullOrEmpty(SearchFilterCustomerName))
				{
					query = query.Where(x => x.Order.CustomerName.Contains(SearchFilterCustomerName));
				}

				if (!string.IsNullOrEmpty(SearchFilterOrderTrackingNumber))
				{
					query = query.Where(x => x.Order.TrackingNumber.Contains(SearchFilterOrderTrackingNumber));
				}

				if (!string.IsNullOrEmpty(SearchFilterItemCode))
				{
					query =
						query.Where(
							x =>
							x.RmaReturnItems.Any(
								rItem => rItem.RmaLineItems.Any(lItem => lItem.LineItem.CatalogItemCode.Contains(SearchFilterItemCode))));
				}

				if (!string.IsNullOrEmpty(SearchFilterReason) &&
				    AvailableReasons.Any(reason => string.Equals(SearchFilterReason, reason)))
				{
					query = query.Where(x => x.RmaReturnItems.Any(item => item.ReturnReason == SearchFilterReason));
				}

				overallCount = query.Count();
				var l = query.OrderByDescending(x => x.Created).Skip(startIndex).Take(count).ToList();

				retVal.AddRange(l.Select(i => _rmaVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", i))));
			}
			return retVal;
		}

		#endregion

        #region ISupportDelayInitialization Members

        public void InitializeForOpen()
        {
			if (ListItemsSource == null)
            {
				OnUIThread(() => ListItemsSource = new VirtualList<IRmaViewModel>(this, 25, SynchronizationContext.Current));
            }
        }

        #endregion

		#region private members
		
		private void DoClearFilters()
		{
			SearchFilterAuthorizationCode = null;
			SearchFilterItemCode = null;
			SearchFilterItemName = null;
			SearchFilterReason = null;
			SearchFilterOrderTrackingNumber = null;
			SearchFilterCustomerName = null;
		}

		#endregion

		#region ViewModelHomeEditableBase overrides
		
		protected override bool CanItemAddExecute()
	    {
		    return false;
	    }

	    protected override bool CanItemDeleteExecute(IList x)
	    {
		    return false;

	    }

	    protected override void RaiseItemAddInteractionRequest()
	    {
			throw new ApplicationException("Can't create RmaRequest this way");
	    }

	    protected override void RaiseItemDeleteInteractionRequest(IList selectedItemsList)
	    {
			throw new ApplicationException("Can't delete RmaRequest this way");
		    //throw new NotImplementedException();
		}

		#endregion
	}
}
