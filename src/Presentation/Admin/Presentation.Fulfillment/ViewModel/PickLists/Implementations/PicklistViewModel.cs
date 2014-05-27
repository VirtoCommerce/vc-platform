using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Model.Fulfillment;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Fulfillment.Report;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.PickLists.Interfaces;
using VirtoCommerce.ManagementClient.Localization;
using vm = VirtoCommerce.ManagementClient.Fulfillment.Model;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.PickLists.Implementations
{
	public class PicklistViewModel : ViewModelDetailBase<Picklist>, IPicklistViewModel
	{
		#region Dependencies
		private readonly IAuthenticationContext _authContext;
		private readonly IRepositoryFactory<IFulfillmentRepository> _repositoryFactory;
		private readonly IRepositoryFactory<IFulfillmentCenterRepository> _fulfillmentCenterRepositoryFactory;
		private readonly IRepositoryFactory<IOrderRepository> _orderRepositoryFactory;
		private readonly INavigationManager _navManager;
		#endregion

		#region Constructor

		public PicklistViewModel(
			IRepositoryFactory<IFulfillmentRepository> repositoryFactory,
			IRepositoryFactory<IOrderRepository> orderRepositoryFactory,
			IRepositoryFactory<IFulfillmentCenterRepository> fulfillmentCenterRepositoryFactory,
			INavigationManager navManager,
			IAuthenticationContext authContext,
			Picklist item)
			: base(null, item)
		{
			_repositoryFactory = repositoryFactory;
			_orderRepositoryFactory = orderRepositoryFactory;
			_fulfillmentCenterRepositoryFactory = fulfillmentCenterRepositoryFactory;
			_authContext = authContext;
			_navManager = navManager;

			ViewTitle = new ViewTitleBase
				{
                    Title = "Picklist",
					SubTitle = (item != null && item.Shipments != null)
							? string.Format("Overall items {0}".Localize(), item.Shipments.Sum(x => x.ShipmentItems.Sum(y => y.Quantity))).ToUpper()
							: ""
				};

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));

			PrintPicklistCommand = new DelegateCommand(PrintPicklistRequest);
			PrintSlipsCommand = new DelegateCommand(PrintSlipRequest);
			RemoveSelectedShipmentsCommand = new DelegateCommand(OnRemoveSelectedShipmentsCommand, CanRemoveShipments);
		}

		private bool CanRemoveShipments()
		{
			return Shipments != null && Shipments.Any(x => x.IsSelected);
		}

		protected override void InitializePropertiesForViewing()
		{
			Debug.Assert(InnerItem != null);
			using (var repository = _fulfillmentCenterRepositoryFactory.GetRepositoryInstance())
			{
				var q = repository.FulfillmentCenters.Where(fc => fc.FulfillmentCenterId == InnerItem.FulfillmentCenterId);
				var center = q.FirstOrDefault();
				FulfillmentCenter = center == null ? InnerItem.FulfillmentCenterId : center.Name;
			}
			OnSpecifiedPropertyChanged("FulfillmentCenter");
		}

		#endregion

		#region ViewModelBase : DisplayName, ShellDetailItemMenuBrush

		public override string DisplayName
		{
			get
			{
				return OriginalItem.Created == null ? string.Empty : OriginalItem.Created.ToString();
			}
		}

		public override Brush ShellDetailItemMenuBrush
		{
			get
			{
				var result =
					(SolidColorBrush)Application.Current.TryFindResource("PickListDetailItemMenuBrush");

				return result ?? base.ShellDetailItemMenuBrush;
			}
		}

		private NavigationItem _navigationData;
		public override NavigationItem NavigationData
		{
			get
			{
				return _navigationData ??
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.PicklistId),
															NavigationNames.HomeName, NavigationNames.PicklistMenuName, this));
			}
		}

		#endregion

		#region ViewModelDetailBase
		public override string ExceptionContextIdentity
		{
			get { return string.Format("Edit PickList ({0})".Localize(), DisplayName); }
		}

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override void LoadInnerItem()
		{
			var picklist = ((IFulfillmentRepository)Repository).Picklists.Where(x => x.PicklistId == OriginalItem.PicklistId)
																	.Expand("Shipments/OrderForm/OrderGroup/OrderAddresses,Shipments/ShipmentItems/LineItem,").SingleOrDefault();

			OnUIThread(() =>
			{
				InnerItem = picklist;
				ReloadShipments();
			});
		}

		private void ReloadShipments()
		{
			if (InnerItem != null && InnerItem.Shipments.Any())
			{
				Shipments = new List<vm.Shipment>();
				InnerItem.Shipments.ToList().ForEach(shipment => Shipments.Add(new vm.Shipment(shipment)));
			}
			OnPropertyChanged("Shipments");
		}

		protected override bool IsValidForSave()
		{
			InnerItem.Validate();
			return !InnerItem.Errors.Any();
		}

		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to PickList '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}
		#endregion

		#region Print
		public DelegateCommand PrintPicklistCommand
		{
			get;
			protected set;
		}
		public DelegateCommand PrintSlipsCommand
		{
			get;
			protected set;
		}

		private void PrintPicklistRequest()
		{
			var pdf = new PicklistPdfReport().CreatePdfReport(InnerItem.Shipments, _authContext.CurrentUserName);
			Process.Start(pdf.FileName);
		}

		private void PrintSlipRequest()
		{
			InnerItem.Shipments.ToList().ForEach(x => x.ShipmentItems.ToList().ForEach(shipItem => shipItem.Shipment = x));
			var pdf = new SlipPdfReport().CreatePdfReport(InnerItem.Shipments, _authContext.CurrentUserName);
			Process.Start(pdf.FileName);
		}
		#endregion

		#region RemoveSelectedShipmentsCommand and realisation
		public DelegateCommand RemoveSelectedShipmentsCommand
		{
			get;
			protected set;
		}

		private void OnRemoveSelectedShipmentsCommand()
		{
			using (var orderRepository = _orderRepositoryFactory.GetRepositoryInstance())
			{
				Shipments.Where(x => x.IsSelected).ToList().ForEach(s =>
					{
						var shipment = orderRepository.Shipments.Where(o => o.ShipmentId == s.ShipmentId).First();
						shipment.PicklistId = null;
						shipment.FulfillmentCenterId = null;
						shipment.Status = Foundation.Orders.Model.ShipmentStatus.Released.ToString();
						var temp = Shipments.First(x => x.ShipmentId == shipment.ShipmentId);
						Shipments.Remove(temp);
						orderRepository.UnitOfWork.Commit();
						InnerItem.Shipments.Remove(shipment);
					});
			}

			OnPropertyChanged("InnerItem");
			ReloadShipments();
			RemoveSelectedShipmentsCommand.RaiseCanExecuteChanged();
		}
		#endregion

		#region Fields and properties

		public string FulfillmentCenter { get; set; }

		private bool _setAll;
		public bool SetAll
		{
			get
			{
				return _setAll;
			}
			set
			{
				_setAll = value;
				SetItemsStatus();
				OnPropertyChanged();
			}
		}

		private List<vm.Shipment> _shipments;
		public List<vm.Shipment> Shipments
		{
			get { return _shipments ?? (_shipments = new List<vm.Shipment>()); }
			set { _shipments = value; OnPropertyChanged(); }
		}

		private void SetItemsStatus()
		{
			Shipments.ForEach(shipment => shipment.IsSelected = _setAll);
			RemoveSelectedShipmentsCommand.RaiseCanExecuteChanged();
		}

		#endregion

		#region IPicklistViewModel
		public Picklist Picklist
		{
			get { return InnerItem; }
		}
		#endregion
	}
}
