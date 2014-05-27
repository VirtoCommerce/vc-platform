using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Implementations
{
	public class PriceListViewModel : ViewModelDetailAndWizardBase<Pricelist>, IPriceListViewModel
	{
		#region Fields

		private const string SettingName_Currencies = "Currencies";
		public string[] AllAvailableCurrencies { get; private set; }

		#endregion

		#region Dependencies

		private readonly IAuthenticationContext _authContext;
		private readonly INavigationManager _navManager;
		private readonly IRepositoryFactory<IPricelistRepository> _repositoryFactory;
		private readonly IRepositoryFactory<IAppConfigRepository> _appConfigRepositoryFactory;
		private readonly IViewModelsFactory<IPriceViewModel> _priceVmFactory;

		#endregion

		#region Constructor

		/// <summary>
		/// public. For viewing
		/// </summary>
		public PriceListViewModel(IRepositoryFactory<IPricelistRepository> repositoryFactory,
			IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, IViewModelsFactory<IPriceViewModel> priceVmFactory,
			ICatalogEntityFactory entityFactory, INavigationManager navManager,
			IAuthenticationContext authContext, Pricelist item)
			: base(entityFactory, item, false)
		{
			ViewTitle = new ViewTitleBase()
			{
                Title = "Price List",
				SubTitle = (item != null && !string.IsNullOrEmpty(item.Name)) ? item.Name.ToUpper(CultureInfo.InvariantCulture) : ""
			};
			_repositoryFactory = repositoryFactory;
			_appConfigRepositoryFactory = appConfigRepositoryFactory;
			_priceVmFactory = priceVmFactory;
			_navManager = navManager;
			_authContext = authContext;
			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
			CommandsInit();
		}

		/// <summary>
		/// protected. For a step
		/// </summary>
		protected PriceListViewModel(IRepositoryFactory<IPricelistRepository> repositoryFactory,
			IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
			ICatalogEntityFactory entityFactory,
			IAuthenticationContext authContext, Pricelist item)
			: base(entityFactory, item, true)
		{
			_repositoryFactory = repositoryFactory;
			_appConfigRepositoryFactory = appConfigRepositoryFactory;
			_authContext = authContext;
		}

		#endregion

		#region ViewModelBase members

		public override string IconSource
		{
			get { return "Icon_Module_PriceLists"; }
		}

		public override string DisplayName
		{
			get { return OriginalItem == null ? this.GetHashCode().ToString() : OriginalItem.Name; }
		}


		public override Brush ShellDetailItemMenuBrush
		{
			get
			{
				var result =
					(SolidColorBrush)Application.Current.TryFindResource("PriceListDetailItemMenuBrush");

				return result ?? base.ShellDetailItemMenuBrush;
			}
		}

		private NavigationItem _navigationData;
		public override NavigationItem NavigationData
		{
			get
			{
				return _navigationData ??
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.PricelistId),
														NavigationNames.HomeNamePriceList,
														NavigationNames.MenuNamePriceList, this));
			}
		}

		#endregion

		#region ViewModelDetailAndWizardBase Members

		public override string ExceptionContextIdentity { get { return string.Format("Price list ({0})", DisplayName); } }

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override bool HasPermission()
		{
			return _authContext.CheckPermission(PredefinedPermissions.PricingPrice_ListsManage);
		}

		protected override bool IsValidForSave()
		{
			return InnerItem.Validate();
		}

		/// <summary>
		/// Return RefusedConfirmation for Cancel Confirm dialog
		/// </summary>
		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to price list '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			try
			{
				var priceRepository = (IPricelistRepository)Repository;
				var item = priceRepository.Pricelists.Where(x => x.PricelistId == OriginalItem.PricelistId)
					// .Expand("Prices/CatalogItem")
						.SingleOrDefault();

				// load only top 200 prices
				var queryPrices = priceRepository.Prices
								.Where(x => x.PricelistId == OriginalItem.PricelistId)
								.Expand(x => x.CatalogItem)
								.Take(200);
				item.Prices.SetItems(queryPrices.ToArray());
				OnUIThread(() =>
				{
					InnerItem = item;
				});

			}
			catch (Exception ex)
			{
				ShowErrorDialog(ex, string.Format("An error occurred when trying to load {0}",
					ExceptionContextIdentity));
			}
		}

		protected override void InitializePropertiesForViewing()
		{
			if (!IsWizardMode)
			{
				InitializeAvailableCurrencies();
				InitializePrices();
			}
		}

		protected override void BeforeSaveChanges()
		{
			if (!IsWizardMode)
			{
				OnUIThread(UpdatePrices);
			}
		}

		protected override void AfterSaveChangesUI()
		{
			// OriginalItem.InjectFrom<CloneInjection>(InnerItem); //too slowly UI operation
			OriginalItem.InjectFrom(InnerItem); // quick
		}

		protected override void SetSubscriptionUI()
		{
			if (Prices != null)
			{
				Prices.CollectionChanged += ViewModel_PropertyChanged;
			}
		}

		protected override void CloseSubscriptionUI()
		{
			if (Prices != null)
			{
				Prices.CollectionChanged -= ViewModel_PropertyChanged;
			}
		}

		protected override bool BeforeDelete()
		{
			InnerItem.Prices.DeleteCollectionItems(Repository as IPricelistRepository);
			return true;
		}

		#endregion

		#region IPriceListViewModel

		public DelegateCommand PriceAddCommand { get; private set; }
		public DelegateCommand<Price> PriceEditCommand { get; private set; }
		public DelegateCommand<Price> PriceDeleteCommand { get; private set; }

		public ObservableCollection<Price> Prices { set; get; }
		public Dictionary<Price, Price> PricesMap { set; get; }

		#endregion

		#region Pricing tab

		private void RaisePriceAddInteractionRequest()
		{
			var item = EntityFactory.CreateEntity<Price>();
			item.MinQuantity = 1;
			item.PricelistId = InnerItem.PricelistId; // required for validation
			if (RaisePriceEditInteractionRequest(item, "Add Price".Localize()))
			{
				Prices.Add(item);
			}
		}

		private void RaisePriceEditInteractionRequest(Price originalItem)
		{
			var item = originalItem.DeepClone(EntityFactory as IKnownSerializationTypes);
			if (RaisePriceEditInteractionRequest(item, "Edit Price".Localize()))
			{
				// copy all values to original:
				OnUIThread(() => originalItem.InjectFrom<CloneInjection>(item));
				OnViewModelPropertyChangedUI(null, null);
			}
		}

		private bool RaisePriceEditInteractionRequest(Price item, string title)
		{
			var result = false;
			var parameters = new List<KeyValuePair<string, object>>
				{
					new KeyValuePair<string, object>("item", item),
					new KeyValuePair<string, object>("preloadedItems", Prices),
					new KeyValuePair<string, object>("isAllFieldsVisible", true)
				};
			var itemVM = _priceVmFactory.GetViewModelInstance(parameters.ToArray());

			var confirmation = new ConditionalConfirmation(itemVM.Validate) { Content = itemVM, Title = title };
			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				result = x.Confirmed;
			});

			return result;
		}

		private void RaisePriceDeleteInteractionRequest(Price item)
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure you want to delete price '{0}'?".Localize(), item.PriceId),
				Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					//                    item.PropertyChanged -= ViewModel_PropertyChanged;
					Prices.Remove(item);
				}
			});
		}

		#endregion

		#region Private Methods

		private void CommandsInit()
		{
			PriceAddCommand = new DelegateCommand(RaisePriceAddInteractionRequest, HasPermission);
			PriceEditCommand = new DelegateCommand<Price>(RaisePriceEditInteractionRequest,
				x => HasPermission() && x != null);
			PriceDeleteCommand = new DelegateCommand<Price>(RaisePriceDeleteInteractionRequest,
				x => HasPermission() && x != null);
		}

		#endregion

		#region Initialize and Update

		protected void InitializeAvailableCurrencies()
		{
			if (AllAvailableCurrencies == null)
			{
				try
				{
					using (var appConfigRepository = _appConfigRepositoryFactory.GetRepositoryInstance())
					{
						var setting =
							appConfigRepository.Settings.Where(x => x.Name == SettingName_Currencies)
											   .ExpandAll()
											   .SingleOrDefault();
						if (setting != null)
						{
							OnUIThread(() =>
							{
								AllAvailableCurrencies = setting.SettingValues.Select(x => x.ShortTextValue).ToArray();
								OnPropertyChanged("AllAvailableCurrencies");
							});
						}
					}
				}
				catch (Exception ex)
				{
					ShowErrorDialog(ex, string.Format("An error occurred when trying to load currencies for : {0}",
													  ExceptionContextIdentity));
				}
			}
		}

		protected void InitializePrices()
		{
			var tmpPrices = new List<Price>();
			PricesMap = new Dictionary<Price, Price>();

			if (InnerItem != null)
			{
				foreach (var price in InnerItem.Prices)
				{
					// rp: this provoke a lot of unnecessary validation activities
					// proxyPrice.InjectFrom<CloneInjection>(price);
					// var proxyPrice = new Price();

					var proxyPrice = Price.Clone(price);

					tmpPrices.Add(proxyPrice);
					PricesMap.Add(proxyPrice, price);
				}

				OnUIThread(() =>
					{
						Prices = new ObservableCollection<Price>(tmpPrices);
						OnSpecifiedPropertyChanged("Prices");
					});
			}
		}

		protected void UpdatePrices()
		{
			if (Prices != null && InnerItem != null && InnerItem.Prices != null)
			{
				var priceForDelete = new List<Price>();
				foreach (var pair in PricesMap)
				{
					Price originalPrice = pair.Value;
					Price proxyPrice = pair.Key;
					if (!Prices.Contains(proxyPrice))
					{
						priceForDelete.Add(originalPrice);
					}
					else
					{
						if (!originalPrice.Equals(proxyPrice))
						{
							originalPrice.List = proxyPrice.List;
							originalPrice.Sale = proxyPrice.Sale;
							originalPrice.MinQuantity = proxyPrice.MinQuantity;
						}
					}
				}
				foreach (var price in priceForDelete)
				{
					InnerItem.Prices.Remove(price);
				}
				foreach (var proxyPrice in Prices)
				{
					if (!PricesMap.ContainsKey(proxyPrice))
					{
						proxyPrice.CatalogItem = null;
						InnerItem.Prices.Add(proxyPrice);
					}

					//if (InnerItem.Prices.All(x => !(x.ItemId == price.ItemId && x.MinQuantity==price.MinQuantity)))
					//{
					//}
				}
			}
		}

		#endregion
	}
}