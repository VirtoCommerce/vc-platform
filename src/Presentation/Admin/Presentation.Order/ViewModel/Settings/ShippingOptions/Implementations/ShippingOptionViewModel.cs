using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model.Gateways;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Order.Model.Settings;
using VirtoCommerce.ManagementClient.Order.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingOptions.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingOptions.Implementations
{
	public class ShippingOptionViewModel : ViewModelDetailAndWizardBase<ShippingOption>, IShippingOptionViewModel
	{
		#region Dependencies

		private readonly INavigationManager _navManager;
		private readonly IHomeSettingsViewModel _parent;
		private readonly IRepositoryFactory<IShippingRepository> _repositoryFactory;
		private readonly IViewModelsFactory<IShippingOptionAddShippingPackageViewModel> _addPackageVmFactory;
		private readonly ICatalogRepository _catalogRepository;
		#endregion

		#region constructor

		public ShippingOptionViewModel(
			IViewModelsFactory<IShippingOptionAddShippingPackageViewModel> addPackageVmFactory,
			IRepositoryFactory<IShippingRepository> repositoryFactory,
			IOrderEntityFactory entityFactory,
			IHomeSettingsViewModel parent,
			INavigationManager navManager, ShippingOption item, ICatalogRepository catalogRepository)
			: base(entityFactory, item, false)
		{
            ViewTitle = new ViewTitleBase { SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory), Title = "Shipping Option" };
			_repositoryFactory = repositoryFactory;
			_navManager = navManager;
			_parent = parent;
			_addPackageVmFactory = addPackageVmFactory;
			_catalogRepository = catalogRepository;

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));

			CommandInit();
		}

		protected ShippingOptionViewModel(
			IViewModelsFactory<IShippingOptionAddShippingPackageViewModel> addPackageVmFactory,
			IRepositoryFactory<IShippingRepository> repositoryFactory,
			IOrderEntityFactory entityFactory,
			ICatalogRepository catalogRepository,
			ShippingOption item)
			: base(entityFactory, item, true)
		{
			_repositoryFactory = repositoryFactory;
			_addPackageVmFactory = addPackageVmFactory;
			_catalogRepository = catalogRepository;
			CommandInit();
		}

		#endregion

		#region ViewModelBase members

		public override string DisplayName
		{
			get
			{
				return InnerItem.ShippingOptionId;
			}
		}

		public override string IconSource
		{
			get
			{
				return "shippingOption";
			}
		}

		public override Brush ShellDetailItemMenuBrush
		{
			get
			{
				var result =
				  (SolidColorBrush)Application.Current.TryFindResource("SettingsDetailItemMenuBrush");

				return result ?? base.ShellDetailItemMenuBrush;
			}
		}

		private NavigationItem _navigationData;
		public override NavigationItem NavigationData
		{
			get
			{
				return _navigationData ??
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.ShippingOptionId),
															Configuration.NavigationNames.HomeName,
															NavigationNames.MenuName, this));
			}
		}

		#endregion

		#region ViewModelDetailAndWizardBase members

		public override string ExceptionContextIdentity
		{
			get { return string.Format("Shipping option ({0})", DisplayName); }
		}

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override bool IsValidForSave()
		{
			return InnerItem.Validate();
		}

		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to Shipping option '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			var item = (Repository as IShippingRepository).ShippingOptions.Expand(so => so.ShippingPackages)
				.Expand(so => so.ShippingGatewayPropertyValues)
				.Where(so => so.ShippingOptionId == OriginalItem.ShippingOptionId)
				.SingleOrDefault();

			OnUIThread(() => InnerItem = item);
		}

		protected override void InitializePropertiesForViewing()
		{

			OnUIThread(() =>
			{
				if (Gateways == null)
				{
					Gateways = (Repository as IShippingRepository).ShippingGateways.ExpandAll().ToList();

				}
			});

		}

		protected override void SetSubscriptionUI()
		{
			InnerItem.ShippingPackages.CollectionChanged += ViewModel_PropertyChanged;
		}

		protected override void CloseSubscriptionUI()
		{
			InnerItem.ShippingPackages.CollectionChanged -= ViewModel_PropertyChanged;
		}


		protected override void BeforeSaveChanges()
		{
			InitializeGatewayValuesForSave();
		}

		protected override void AfterSaveChangesUI()
		{
			if (_parent != null)
			{
				OriginalItem.InjectFrom(InnerItem);
				_parent.RefreshItem(OriginalItem);
			}
		}

		#endregion

		#region IWizardStep Members

		public override string Description
		{
			get { return "Enter Shipping Option information.".Localize(); }
		}

		public override bool IsValid
		{
			get
			{
				var retval = true;
				if (this is IShippingOptionOverviewStepViewModel)
				{
					bool doNotifyChanges = false;
					InnerItem.Validate(doNotifyChanges);
					retval = InnerItem.Errors.Count == 0 && !string.IsNullOrEmpty(InnerItem.Description) &&
							 !string.IsNullOrEmpty(InnerItem.Name);
					InnerItem.Errors.Clear();
				}

				return retval;
			}
		}

		public override bool IsLast
		{
			get { return this is IShippingOptionPackagesStepViewModel; }
		}

		#endregion

		#region Commands and Request

		public DelegateCommand AddShippingPackageCommand { get; private set; }
		public DelegateCommand<ShippingPackage> RemoveShippingPackageCommand { get; private set; }
		public DelegateCommand<ShippingPackage> EditShippingPackageCommand { get; private set; }


		public InteractionRequest<ConditionalConfirmation> AddEditShippingPackageRequest { get; private set; }

		public InteractionRequest<ConditionalConfirmation> RemoveShippingPackageRequest { get; private set; }

		#endregion

		#region Command Implementation

		private void CommandInit()
		{
			AddShippingPackageCommand = new DelegateCommand(AddShippingPackage);
			EditShippingPackageCommand = new DelegateCommand<ShippingPackage>(EditShippingPackage,
																			  (x) => SelectedShippingPackage != null);
			RemoveShippingPackageCommand = new DelegateCommand<ShippingPackage>(RemoveShippingPackage,
																				(x) => SelectedShippingPackage != null);

			AddEditShippingPackageRequest = new InteractionRequest<ConditionalConfirmation>();
			RemoveShippingPackageRequest = new InteractionRequest<ConditionalConfirmation>();

			AllPackages = _catalogRepository != null ? _catalogRepository.Packagings.ToList() : null;
		}

		private void AddShippingPackage()
		{

			var parameters = new[]
				{
					new KeyValuePair<string, object>("item", new ShippingPackage()),
					new KeyValuePair<string, object>("selectedPackaging", InnerItem.ShippingPackages.Select(sp => sp.MappedPackagingId).ToList())
				};

			var itemVm = _addPackageVmFactory.GetViewModelInstance(parameters);

			var confirmation = new ConditionalConfirmation
			{
				Title = "Add Payment package".Localize(),
				Content = itemVm
			};

			if (AddEditShippingPackageRequest != null)
			{
				AddEditShippingPackageRequest.Raise(confirmation,
					(x) =>
					{
						if (x.Confirmed)
						{
							var itemToAdd = (x.Content as IShippingOptionAddShippingPackageViewModel).InnerItem;

							InnerItem.ShippingPackages.Add(itemToAdd);
						}
					});
			}

		}

		private void EditShippingPackage(ShippingPackage selectedItem)
		{
			var itemToUpdate = selectedItem;

			var selectedItems = InnerItem.ShippingPackages.Select(sp => sp.MappedPackagingId).ToList();
			selectedItems.Remove(itemToUpdate.MappedPackagingId);

			var parameters = new[]
            {
                new KeyValuePair<string, object> ("item", itemToUpdate),
                new KeyValuePair<string, object> ("selectedPackaging", selectedItems)
            };

			var itemVm = _addPackageVmFactory.GetViewModelInstance(parameters);

			var confirmation = new ConditionalConfirmation
			{
				Title = "Add Payment package".Localize(),
				Content = itemVm
			};

			if (AddEditShippingPackageRequest != null)
			{
				AddEditShippingPackageRequest.Raise(confirmation,
					(x) =>
					{
						if (x.Confirmed)
						{
							var itemFromDialog = (x.Content as IShippingOptionAddShippingPackageViewModel).InnerItem;

							OnUIThread(() => itemToUpdate.InjectFrom<CloneInjection>(itemFromDialog));

							IsModified = true;
						}
					});
			}

		}

		private void RemoveShippingPackage(ShippingPackage selectedItem)
		{
			var itemToRemove = selectedItem;

			var confirmation = new ConditionalConfirmation
				{
					Content = string.Format("Remove shipping package '{0}?".Localize(), itemToRemove.ShippingOptionPackaging),
					Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
				};

			if (RemoveShippingPackageRequest != null)
			{
				RemoveShippingPackageRequest.Raise(confirmation,
				(x) =>
				{
					if (x.Confirmed)
					{

						var item =
							InnerItem.ShippingPackages.SingleOrDefault(
								sp => sp.ShippingPackageId == itemToRemove.ShippingPackageId);

						if (item != null)
						{
							InnerItem.ShippingPackages.Remove(item);
						}
					}
				});
			}
		}

		#endregion

		#region Properties


		private ShippingPackage _selectedShippingPackage;
		public ShippingPackage SelectedShippingPackage
		{
			get { return _selectedShippingPackage; }
			set
			{
				_selectedShippingPackage = value;
				OnPropertyChanged();
				EditShippingPackageCommand.RaiseCanExecuteChanged();
				RemoveShippingPackageCommand.RaiseCanExecuteChanged();
			}
		}

		private List<Packaging> _allPackages;
		public List<Packaging> AllPackages
		{
			get { return _allPackages; }
			set
			{
				_allPackages = value;
				OnPropertyChanged();
			}
		}

		protected List<ShippingGateway> _Gateways;
		public List<ShippingGateway> Gateways
		{
			get { return _Gateways; }
			set
			{
				_Gateways = value;
				OnPropertyChanged();
				if (!string.IsNullOrEmpty(InnerItem.ShippingGatewayId))
				{
					var itemGateway = Gateways.FirstOrDefault(x => x.GatewayId == InnerItem.ShippingGatewayId);
					if (itemGateway != null)
						SelectedGateway = itemGateway;
				}
			}
		}

		protected ShippingGateway _SelectedGateway;
		public virtual ShippingGateway SelectedGateway
		{
			get { return _SelectedGateway; }
			set
			{
				_SelectedGateway = value;
				OnPropertyChanged();

				if (_SelectedGateway != null)
				{
					if (_SelectedGateway.GatewayId == "None")
					{
						InnerItem.ShippingGatewayId = null;
						PropertyValues = new List<GeneralPropertyValueEditViewModel>();
					}
					else
					{
						if (!IsInitializing)
						{
							InnerItem.ShippingGatewayId = _SelectedGateway.GatewayId;
							InnerItem.ShippingGateway = _SelectedGateway;
						}

						var newValuesList = new List<GeneralPropertyValueEditViewModel>();
						// create property values placeholders
						_SelectedGateway.GatewayProperties.ToList().ForEach(x =>
						{
							var item = new GeneralPropertyValue(x);
							switch ((GatewayProperty.ValueTypes)x.ValueType)
							{
								case GatewayProperty.ValueTypes.ShortString:
									item.ValueType = (int)GeneralPropertyValueType.ShortString;
									break;
								case GatewayProperty.ValueTypes.Boolean:
									item.ValueType = (int)GeneralPropertyValueType.Boolean;
									break;
								case GatewayProperty.ValueTypes.DictionaryKey:
									item.ValueType = (int)GeneralPropertyValueType.DictionaryKey;
									item.DictionaryValues = new System.Collections.Specialized.StringDictionary();
									x.GatewayPropertyDictionaryValues.ToList()
										.ForEach(x1 => item.DictionaryValues.Add(x1.Value, x1.DisplayName));
									break;
							}

							if (_SelectedGateway.GatewayId == OriginalItem.ShippingGatewayId)
							{
								// load saved property values
								var oldValue =
									InnerItem.ShippingGatewayPropertyValues.FirstOrDefault(x1 => x1.Name == x.Name);
								if (oldValue != null)
									item.InjectFrom(oldValue);
							}

							item.PropertyChanged += ViewModel_PropertyChanged;
							newValuesList.Add(new GeneralPropertyValueEditViewModel(item));
						});

						PropertyValues = newValuesList;
					}
				}
			}
		}

		private List<GeneralPropertyValueEditViewModel> _PropertyValues;
		public List<GeneralPropertyValueEditViewModel> PropertyValues
		{
			get { return _PropertyValues; }
			private set { _PropertyValues = value; OnPropertyChanged(); OnPropertyChanged("IsPropertyValuesVisible"); }
		}

		public bool IsPropertyValuesVisible
		{
			get { return PropertyValues != null && PropertyValues.Count > 0; }
		}

		public int SelectedTabIndex { get; set; }

		#endregion

		#region private members

		private void InitializeGatewayValuesForSave()
		{
			var RemovedItems = InnerItem.ShippingGatewayPropertyValues.Where(
					x => PropertyValues.All(x1 => x1.InnerItem.Name != x.Name)).ToList();
			foreach (var removedItem in RemovedItems)
			{
				InnerItem.ShippingGatewayPropertyValues.Remove(removedItem);
			}

			if (PropertyValues == null)
				return;

			foreach (var currentItem in PropertyValues)
			{
				var item = InnerItem.ShippingGatewayPropertyValues.FirstOrDefault(x => x.Name == currentItem.InnerItem.Name);
				if (item == null)
				{
					item = EntityFactory.CreateEntity<ShippingGatewayPropertyValue>();
					InnerItem.ShippingGatewayPropertyValues.Add(item);
				}
				item.InjectFrom(currentItem.InnerItem);
			}

			if (!IsWizardMode)
				Repository.UnitOfWork.Commit();
		}


		#endregion
	}
}
