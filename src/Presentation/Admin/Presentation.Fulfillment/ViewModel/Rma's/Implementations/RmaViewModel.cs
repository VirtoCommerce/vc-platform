using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.EventAggregation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Rmas.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Rmas.Implementations
{
	public class RmaViewModel : ViewModelDetailBase<RmaRequest>, IRmaViewModel
	{
		const string SettingNameReturnConditions = "ReturnConditions";

		private readonly IRepositoryFactory<IAppConfigRepository> _settingsRepositoryFactory;
		private readonly IRepositoryFactory<IOrderRepository> _repositoryFactory;
		private readonly IAuthenticationContext _authenticationContext;
		private readonly INavigationManager _navManager;
		private readonly string _defaultCondition;

		#region Constructor
		public RmaViewModel(
			IRepositoryFactory<IOrderRepository> repositoryFactory,
			IRepositoryFactory<IAppConfigRepository> settingsRepositoryFactory,
			IAuthenticationContext authenticationContext,
			INavigationManager navManager,
			RmaRequest item)
			: base(null, item)
		{
			_authenticationContext = authenticationContext;
			_repositoryFactory = repositoryFactory;
			_settingsRepositoryFactory = settingsRepositoryFactory;
			_navManager = navManager;

			ViewTitle = new ViewTitleBase
			{
                Title = "Rma request",
				SubTitle = item != null
				? (string.IsNullOrEmpty(item.AuthorizationCode) ? item.Order.TrackingNumber : item.AuthorizationCode)
				: ""
			};

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));

			AvailableConditions = Task.Run(() =>
				{
					string[] result = null;
					using (var settingsRepository = _settingsRepositoryFactory.GetRepositoryInstance())
					{
						var setting =
							settingsRepository.Settings.Where(x => x.Name == SettingNameReturnConditions)
											   .ExpandAll()
											   .SingleOrDefault();
						if (setting != null)
							result = setting.SettingValues.Select(x => x.ShortTextValue).ToArray();
					}
					return result;
				}).Result;

			_defaultCondition = AvailableConditions.Contains("Defect") ? "Defect" : null;

			GoToSelectedOrderCommand = new DelegateCommand(() =>
				{
					if (InnerItem.OrderId != null)
					{
						var mes = new GoToOrderEvent { OrderId = InnerItem.OrderId };
						EventSystem.Publish(mes);
					}
				});

			//CompleatedRmaLines = new bool[item.RmaReturnItems.Count()];
			//var i = 0;
			//foreach (var l in item.RmaReturnItems)
			//{
			//	CompleatedRmaLines[i++] = (l.ItemState == RmaLineItemState.Received.ToString()); 
			//}
		}

		#endregion

		#region ViewModel properties: AvailableConditions,CompleatedRmaLines
		//public bool[] CompleatedRmaLines
		//{
		//	get;set;
		//}

		public string[] AvailableConditions
		{
			get;
			private set;
		}

		#endregion

		#region Commands
		public DelegateCommand GoToSelectedOrderCommand { get; private set; }
		#endregion

		#region ViewModelBase: DisplayName, ShellDetailItemMenuBrush

		public override sealed string DisplayName
		{
			get
			{
				return OriginalItem == null ? ToString() : OriginalItem.Order.TrackingNumber;
			}
		}

		public override Brush ShellDetailItemMenuBrush
		{
			get
			{
				var result =
					(SolidColorBrush)Application.Current.TryFindResource("RmaDetailItemMenuBrush");
				return result ?? base.ShellDetailItemMenuBrush;
			}
		}

		private NavigationItem _navigationData;
		public override NavigationItem NavigationData
		{
			get
			{
				return _navigationData ??
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.RmaRequestId),
															NavigationNames.HomeName, NavigationNames.RmaMenuName, this));
			}
		}

		#endregion

		#region ViewModelDetailBase

		public override string ExceptionContextIdentity
		{
			get
			{
				return string.Format("Return/Exchange ({0})", DisplayName);
			}
		}

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override void LoadInnerItem()
		{
			var repository = (IOrderRepository)Repository;
			var item = repository.RmaRequests.Where(x => x.RmaRequestId == OriginalItem.RmaRequestId)
				.Expand("RmaReturnItems/RmaLineItems/LineItem,Order").SingleOrDefault();
			OnUIThread(() => { InnerItem = item; });
		}

		//public List<>

		protected override bool IsValidForSave()
		{
			InnerItem.Validate();
			return !InnerItem.Errors.Any();
		}

		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to rma request '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}
		#endregion

		#region Subscriptions
		protected override void DoSaveChanges()
		{
			InnerItem.RmaReturnItems.Where(
				rItem => rItem.RmaLineItems[0].Quantity == rItem.RmaLineItems[0].ReturnQuantity).ToList()
				.ForEach(y => y.ItemState = Enum.GetName(typeof(RmaLineItemState), RmaLineItemState.Received));
			base.DoSaveChanges();
		}
		protected override void CloseSubscriptionUI()
		{
			foreach (var i in InnerItem.RmaReturnItems)
			{
				i.PropertyChanged -= ViewModel_PropertyChanged;
				foreach (var j in i.RmaLineItems)
				{
					j.PropertyChanged -= ViewModel_PropertyChanged;
					j.PropertyChanged -= Quantity_PropertyChanged;
				}
			}
		}

		protected override void SetSubscriptionUI()
		{
			foreach (var i in InnerItem.RmaReturnItems)
			{
				i.PropertyChanged += ViewModel_PropertyChanged;
				foreach (var j in i.RmaLineItems)
				{
					j.PropertyChanged += ViewModel_PropertyChanged;
					j.PropertyChanged += Quantity_PropertyChanged;
				}
			}
		}

		private void Quantity_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Quantity")
			{
				OnUIThread(() =>
				{
					InnerItem.AgentId = _authenticationContext.CurrentUserId;
					var line = (RmaLineItem)sender;
					// find parent
					var returnItem = InnerItem.RmaReturnItems.First(i => i.RmaLineItems.Contains(line));

					// default value (some help for user provided from UI)
					if (line.ReturnQuantity > 0 && string.IsNullOrEmpty(returnItem.ReturnCondition))
					{
						returnItem.ReturnCondition = _defaultCondition;
					}
					//if all lineItems physically returned to stock need change status 
					//to AwaitingCompletion
					var allReturned = InnerItem.RmaReturnItems.All(i => i.RmaLineItems.All(x => x.ReturnQuantity == x.Quantity));
					InnerItem.Status = allReturned
						? RmaRequestStatus.AwaitingCompletion.ToString()
						: RmaRequestStatus.AwaitingStockReturn.ToString();
				});
			}
		}
		#endregion
	}
}
