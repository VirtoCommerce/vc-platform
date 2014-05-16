using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Fulfillment.Model;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Localization;
using Picklist = VirtoCommerce.Foundation.Orders.Model.Fulfillment.Picklist;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Wizard.Implementations
{
	public class CreatePicklistStepViewModel : ViewModelDetailAndWizardBase<Picklist>, ICreatePicklistStepViewModel
	{
		#region Dependencies

		private readonly IRepositoryFactory<IFulfillmentRepository> _repositoryFactory;
		private readonly IRepositoryFactory<IOrderRepository> _orderRepositoryFactory;
		private readonly IAuthenticationContext _authenticationContext;

		#endregion

		#region Constructor

		public CreatePicklistStepViewModel(
			IRepositoryFactory<IFulfillmentCenterRepository> fulfillmentCenterRepositoryFactory,
			IRepositoryFactory<IFulfillmentRepository> repositoryFactory,
			IRepositoryFactory<IOrderRepository> orderRepositoryFactory,
			IAuthenticationContext authenticationContext,
			Picklist item)
			: base(null, item, true)
		{
			_repositoryFactory = repositoryFactory;
			_orderRepositoryFactory = orderRepositoryFactory;
			_authenticationContext = authenticationContext;

			Initialize(fulfillmentCenterRepositoryFactory);

			UpdateSelectedItemsChangedCommand = new DelegateCommand<Shipment>(shipment => OnIsValidChanged());
		}

		protected override void DoSaveChanges()
		{
			var shipments = new List<Foundation.Orders.Model.Shipment>();

			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				var newPickList = InnerItem;
				newPickList.MemberId = _authenticationContext.CurrentUserId;
				newPickList.FulfillmentCenterId = SelectedFulfillmentCenter.FulfillmentCenterId;
				repository.Add(newPickList);
				repository.UnitOfWork.Commit();
			}

			using (var repository = _orderRepositoryFactory.GetRepositoryInstance())
			{
				Shipments.Where(x => x.IsSelected).ToList().ForEach(s =>
				{
					var shipment = repository.Shipments.Where(o => o.ShipmentId == s.ShipmentId).First();
					shipment.PicklistId = InnerItem.PicklistId;
					shipment.FulfillmentCenterId = SelectedFulfillmentCenter.FulfillmentCenterId;
					shipment.Status = Foundation.Orders.Model.ShipmentStatus.Packing.ToString();
					shipments.Add(shipment);
					repository.UnitOfWork.Commit();
				});
			}
		}

		private async void Initialize(IRepositoryFactory<IFulfillmentCenterRepository> fulfillmentCenterRepositoryFactory)
		{
			IsInitializing = true;

			AvailableFulfillments = await Task.Run(() =>
					{
						using (var fulfillmentCenterRepository = fulfillmentCenterRepositoryFactory.GetRepositoryInstance())
						{
							var l = new List<FulfillmentCenter>();
							if (fulfillmentCenterRepository.FulfillmentCenters != null)
							{
								l = fulfillmentCenterRepository.FulfillmentCenters.OrderBy(x => x.Name).ToList();
							}
							return l;
						}
					});
			var res = await Task.Run(() =>
				{
					using (var orderRepository = _orderRepositoryFactory.GetRepositoryInstance())
					{
						var l = new List<Foundation.Orders.Model.Shipment>();
						if (orderRepository.Shipments != null)
						{
							l = orderRepository.Shipments.Expand(
								"OrderForm/OrderGroup/OrderAddresses,ShipmentItems/LineItem")
								.Where(ship =>
									 ship.PicklistId == null &&
									 (ship.Status == null ||
									  ship.OrderForm.OrderGroup.Status ==
									  Foundation.Orders.Model.OrderStatus.InProgress.ToString()) &&
									 ship.Status ==
									 Foundation.Orders.Model.ShipmentStatus.Released.ToString()).OrderByDescending(item => item.LastModified).ToList();

						}
						return l;
					}
				});

			SelectedFulfillmentCenter = AvailableFulfillments.Any() ? AvailableFulfillments.First() : null;

			res.ForEach(shipment => Shipments.Add(new Shipment(shipment)));
			OnSpecifiedPropertyChanged("Shipments");
			OnIsValidChanged();
			IsInitializing = false;
		}

		#endregion

		#region ViewModelDetailAndWizardBase

		public override string ExceptionContextIdentity
		{
			get
			{
				return string.Format("Create new Picklist ({0})", DisplayName);
			}
		}

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override void LoadInnerItem()
		{
			throw new ApplicationException("Can't be called from there");
		}

		protected override bool IsValidForSave()
		{
			InnerItem.FulfillmentCenterId = SelectedFulfillmentCenter.FulfillmentCenterId;
			InnerItem.Validate();
			return !InnerItem.Errors.Any();
		}

		protected override RefusedConfirmation CancelConfirm()
		{
			var value = new RefusedConfirmation
				{
					Content = string.Format("Create picklist '{0}'?".Localize(), DisplayName),
					Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
				};
			return value;
		}

		public override NavigationItem NavigationData
		{
			get
			{
				return null;
			}
		}


		#endregion

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				return Shipments != null && Shipments.Any(x => x.IsSelected);
			}
		}


		public override bool IsLast
		{
			get
			{
				return true; // only one step in wizard
			}
		}

		public override string Comment
		{
			get
			{
				return string.Empty;
			}
		}

		public override string Description
		{
			get
			{
				return string.Format("Enter picklist details.".Localize());
			}
		}

		#endregion

		#region Commands

		public DelegateCommand<Shipment> UpdateSelectedItemsChangedCommand { get; private set; }

		#endregion

		#region Fields and Properties : Shipments, AvailableFulfillments, SelectedFulfillmentCenter
		private ObservableCollection<Shipment> _shipments;
		public ObservableCollection<Shipment> Shipments
		{
			get { return _shipments ?? (_shipments = new ObservableCollection<Shipment>()); }
			set
			{
				_shipments = value;
				OnPropertyChanged();
			}
		}

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

		#endregion
	}
}