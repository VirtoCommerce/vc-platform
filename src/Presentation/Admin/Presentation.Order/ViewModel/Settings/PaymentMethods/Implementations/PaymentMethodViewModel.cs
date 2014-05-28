using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
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
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.PaymentMethods.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.PaymentMethods.Implementations
{
	public class PaymentMethodViewModel : ViewModelDetailAndWizardBase<PaymentMethod>, IPaymentMethodViewModel
	{
		#region Dependencies

		private readonly INavigationManager _navManager;
		private readonly IHomeSettingsViewModel _parent;
		private readonly IRepositoryFactory<IPaymentMethodRepository> _repositoryFactory;
		private readonly IRepositoryFactory<IShippingRepository> _shippingRepositoryFactory;
		private readonly IViewModelsFactory<IGeneralLanguagesStepViewModel> _langVmFactory;

		#endregion

		#region constructor

		public PaymentMethodViewModel(IRepositoryFactory<IPaymentMethodRepository> repositoryFactory,
			IRepositoryFactory<IShippingRepository> shippingRepositoryFactory,
			IOrderEntityFactory entityFactory, INavigationManager navManager,
			IViewModelsFactory<IGeneralLanguagesStepViewModel> langVmFactory,
			IHomeSettingsViewModel parent, PaymentMethod item)
			: base(entityFactory, item, false)
		{
            ViewTitle = new ViewTitleBase { SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory), Title = "Payment Method" };
			_navManager = navManager;
			_parent = parent;
			_repositoryFactory = repositoryFactory;
			_shippingRepositoryFactory = shippingRepositoryFactory;
			_langVmFactory = langVmFactory;

			PropertyValues = new ObservableCollection<GeneralPropertyValueEditViewModel>();

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));

			InitCommand();
		}

		public PaymentMethodViewModel(IRepositoryFactory<IPaymentMethodRepository> repositoryFactory,
			IRepositoryFactory<IShippingRepository> shippingRepositoryFactory,
			IViewModelsFactory<IGeneralLanguagesStepViewModel> langVmFactory,
			IOrderEntityFactory entityFactory, PaymentMethod item)
			: base(entityFactory, item, true)
		{
			_repositoryFactory = repositoryFactory;
			_langVmFactory = langVmFactory;
			_shippingRepositoryFactory = shippingRepositoryFactory;
			InitCommand();
		}

		private void InitCommand()
		{

			EditPaymentPropertyCommand = new DelegateCommand<PaymentMethodPropertyValue>(EditPaymentProperty, x => x != null);
			RemovePaymentPropertyCommand = new DelegateCommand<PaymentMethodPropertyValue>(RemovePaymentProperty, x => x != null);

			AddEditPaymentPropertyRequest = new InteractionRequest<ConditionalConfirmation>();
			RemovePaymentPropertyRequest = new InteractionRequest<ConditionalConfirmation>();

		}

		#endregion

		#region ViewModelBase members

		public override string DisplayName
		{
			get
			{
				return InnerItem.Name;
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
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.PaymentMethodId),
						Configuration.NavigationNames.HomeName,
						NavigationNames.MenuName, this));
			}
		}


		#endregion

		#region ViewModelDetailAndWizardBase members

		public override string ExceptionContextIdentity
		{
			get { return string.Format("Payment method ({0})", DisplayName); }
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
				Content = string.Format("Save changes to Payment method '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			var item =
				(Repository as IPaymentMethodRepository).PaymentMethods.Where(
					pm => pm.PaymentMethodId == OriginalItem.PaymentMethodId)
					.Expand(pm => pm.PaymentMethodShippingMethods).Expand(pm => pm.PaymentMethodLanguages)
					.Expand(pm => pm.PaymentMethodPropertyValues)
					.SingleOrDefault();

			OnUIThread(() => InnerItem = item);
		}

		protected override void InitializePropertiesForViewing()
		{
			OnUIThread(InitShipingMethods);
			InitializeLanguage();
		}

		protected override void SetSubscriptionUI()
		{
			SelectedShippingMethods.CollectionChanged += ViewModel_PropertyChanged;

			if (LanguagesStepViewModel != null)
			{
				LanguagesStepViewModel.CollectionChanged = ViewModel_PropertyChanged;
			}
		}

		protected override void CloseSubscriptionUI()
		{
			if (SelectedShippingMethods != null)
				SelectedShippingMethods.CollectionChanged -= ViewModel_PropertyChanged;
		}

		protected override void BeforeSaveChanges()
		{
			if (!IsWizardMode)
			{
				InitializeGatewayValuesForSave();
				InitializeLanguagesForSave();
				GetShippingItemToInnerItem();
			}
		}

		protected override void AfterSaveChangesUI()
		{
			if (_parent != null)
			{
				OriginalItem.InjectFrom<CloneInjection>(InnerItem);
				_parent.RefreshItem(OriginalItem);
			}
		}

		#endregion

		#region IWizardStep members

		public override string Description
		{
			get { return "Enter Payment method details".Localize(); }
		}

		public override bool IsLast
		{
			get { return this is IPaymentMethodPropertiesStepViewModel; }
		}

		public override bool IsValid
		{
			get
			{
				bool result = true;

				if (this is IPaymentMethodOverviewStepViewModel)
				{

					result = InnerItem.Validate(false) && !String.IsNullOrEmpty(InnerItem.Name)
						&& !string.IsNullOrEmpty(InnerItem.Description);

				}

				return result;
			}
		}

		#endregion

		#region Commands and Requests


		public DelegateCommand<PaymentMethodPropertyValue> EditPaymentPropertyCommand { get; private set; }
		public DelegateCommand<PaymentMethodPropertyValue> RemovePaymentPropertyCommand { get; private set; }

		public InteractionRequest<ConditionalConfirmation> AddEditPaymentPropertyRequest { get; private set; }
		public InteractionRequest<ConditionalConfirmation> RemovePaymentPropertyRequest { get; private set; }


		#endregion

		#region Commands Implementation

		private void EditPaymentProperty(PaymentMethodPropertyValue selectedItem)
		{
			// todo: delete all this
			//var item = selectedItem.DeepClone(_entityFactory as IKnownSerializationTypes);

			//ConditionalConfirmation confirmation = new ConditionalConfirmation();
			//confirmation.Title = "Enter Property Value";
			//confirmation.Content =
			//    Container.Resolve<IPaymentMethodsAddPropertyValueViewModel>(new ParameterOverride("innerItem", item));

			//AddEditPaymentPropertyRequest.Raise(confirmation,
			//    (x) =>
			//    {
			//        if (x.Confirmed)
			//        {
			//            selectedItem.InjectFrom<CloneInjection>(item);
			//            IsModified = true;
			//        }
			//    });
		}

		private void RemovePaymentProperty(PaymentMethodPropertyValue selectedItem)
		{
			ConditionalConfirmation confirmation = new ConditionalConfirmation();
			confirmation.Title = "Value clear confirmation".Localize();
			confirmation.Content = string.Format("Clear Property Value '{0}'?".Localize(), selectedItem.Name);

			// RemovePaymentPropertyRequest//  ??
			CommonConfirmRequest.Raise(confirmation,
				(x) =>
				{
					if (x.Confirmed)
					{
						var item =
							InnerItem.PaymentMethodPropertyValues.SingleOrDefault(
								pp => pp.PaymentMethodPropertyValueId == selectedItem.PaymentMethodPropertyValueId);
						if (item != null)
						{
							InnerItem.PaymentMethodPropertyValues.Remove(item);
							item.PaymentMethodId = null;
						}
					}
				});
		}

		#endregion

		#region Public Properties

		protected PaymentGateway _selectedGateway;
		public virtual PaymentGateway SelectedGateway
		{
			get { return _selectedGateway; }
			set
			{
				_selectedGateway = value;
				OnPropertyChanged();

				if (_selectedGateway != null)
				{

					if (_selectedGateway.GatewayId == "None")
					{
						InnerItem.PaymentGatewayId = null;
						PropertyValues = null;
					}
					else
					{
						if (!IsInitializing)
						{
							InnerItem.PaymentGatewayId = _selectedGateway.GatewayId;
						}

						var newValuesList = new ObservableCollection<GeneralPropertyValueEditViewModel>();
						// create property values placeholders
						_selectedGateway.GatewayProperties.ToList().ForEach(x =>
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

							if (_selectedGateway.GatewayId == InnerItem.PaymentGatewayId)
							{
								// load saved property values
								var oldValue = InnerItem.PaymentMethodPropertyValues.FirstOrDefault(x1 => x1.Name == x.Name);
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

		private ObservableCollection<GeneralPropertyValueEditViewModel> _propertyValues;
		/// <summary>
		/// Gets or sets the property values. Never change initial object reference as it's used in other wizard steps.
		/// </summary>
		public ObservableCollection<GeneralPropertyValueEditViewModel> PropertyValues
		{
			get { return _propertyValues; }
			set
			{
				if (value == null)
					_propertyValues.Clear();
				else if (_propertyValues != null)
				{
					_propertyValues.SetItems(value);
				}
				else
				{
					_propertyValues = value;
					OnPropertyChanged();
				}

				if (!IsWizardMode)
				{
					OnPropertyChanged("IsPropertyValuesVisible");
				}
			}
		}

		public bool IsPropertyValuesVisible
		{
			get { return PropertyValues != null && PropertyValues.Count > 0; }
		}

		public int SelectedTabIndex { get; set; }

		private List<ShippingMethod> _allAvailableShippingMethods;
		public List<ShippingMethod> AllAvailableShippingMethods
		{
			get { return _allAvailableShippingMethods; }
			set
			{
				_allAvailableShippingMethods = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<ShippingMethod> _selectedShippingMethods;
		public ObservableCollection<ShippingMethod> SelectedShippingMethods
		{
			get { return _selectedShippingMethods; }
			set
			{
				_selectedShippingMethods = value;
				OnPropertyChanged();
			}
		}

		private List<PaymentGateway> _gateways;
		public List<PaymentGateway> Gateways
		{
			get { return _gateways; }
			set
			{
				_gateways = value;
				OnPropertyChanged();
				if (!string.IsNullOrEmpty(InnerItem.PaymentGatewayId))
				{
					var itemGateway = Gateways.FirstOrDefault(x => x.GatewayId == InnerItem.PaymentGatewayId);
					if (itemGateway != null)
						SelectedGateway = itemGateway;
				}
			}
		}

		#endregion

		#region IPaymentMethodViewModel Members

		public IGeneralLanguagesStepViewModel LanguagesStepViewModel { get; set; }

		public void InitializeLanguagesForSave()
		{
			foreach (var removedItem in LanguagesStepViewModel.RemovedItems)
			{
				var item = InnerItem.PaymentMethodLanguages.FirstOrDefault(x => x.PaymentMethodLanguageId == removedItem.Id);
				if (item == null)
					continue;
				InnerItem.PaymentMethodLanguages.Remove(item);
				Repository.Attach(item);
				Repository.Remove(item);
			}

			foreach (var updatedItem in LanguagesStepViewModel.UpdatedItems)
			{
				var item = InnerItem.PaymentMethodLanguages.SingleOrDefault(x => x.PaymentMethodLanguageId == updatedItem.Id);
				if (item == null)
					continue;

				item.InjectFrom(updatedItem);
			}

			foreach (var addedItem in LanguagesStepViewModel.AddedItems)
			{
				var item = EntityFactory.CreateEntity<PaymentMethodLanguage>();
				item.InjectFrom(addedItem);
				InnerItem.PaymentMethodLanguages.Add(item);
			}

			if (!IsWizardMode)
				Repository.UnitOfWork.Commit();

		}

		public void InitializeGatewayValuesForSave()
		{
			var RemovedItems = InnerItem.PaymentMethodPropertyValues.Where(
					x => PropertyValues.All(x1 => x1.InnerItem.Id != x.Name)).ToList();
			foreach (var removedItem in RemovedItems)
			{
				InnerItem.PaymentMethodPropertyValues.Remove(removedItem);
				Repository.Attach(removedItem);
				Repository.Remove(removedItem);
			}

			foreach (var currentItem in PropertyValues)
			{
				var item = InnerItem.PaymentMethodPropertyValues.FirstOrDefault(x => x.Name == currentItem.InnerItem.Id);
				if (item == null)
				{
					item = EntityFactory.CreateEntity<PaymentMethodPropertyValue>();
					InnerItem.PaymentMethodPropertyValues.Add(item);
				}
				item.InjectFrom(currentItem.InnerItem);
			}

			if (!IsWizardMode)
				Repository.UnitOfWork.Commit();
		}

		public void GetShippingItemToInnerItem()
		{
			var selectedPaymentShipping = new List<PaymentMethodShippingMethod>();

			foreach (var selectedShippingMethod in SelectedShippingMethods)
			{
				var itemToAdd = new PaymentMethodShippingMethod();
				itemToAdd.ShippingMethodId = selectedShippingMethod.ShippingMethodId;
				itemToAdd.PaymentMethodId = InnerItem.PaymentMethodId;

				selectedPaymentShipping.Add(itemToAdd);
			}

			var paymentShippingForDelete =
				InnerItem.PaymentMethodShippingMethods.Where(
					x => !selectedPaymentShipping.Any(ps => ps.ShippingMethodId == x.ShippingMethodId)).ToList();


			foreach (var paymentShippingToDelete in paymentShippingForDelete)
			{
				InnerItem.PaymentMethodShippingMethods.Remove(paymentShippingToDelete);
				Repository.Attach(paymentShippingToDelete);
				Repository.Remove(paymentShippingToDelete);
			}

			foreach (var paymentShippingToAdd in selectedPaymentShipping)
			{

				var sameItemFromInnerItem =
					InnerItem.PaymentMethodShippingMethods.SingleOrDefault(
						pmsm =>
						pmsm.PaymentMethodId == paymentShippingToAdd.PaymentMethodId &&
						pmsm.ShippingMethodId == paymentShippingToAdd.ShippingMethodId);

				if (sameItemFromInnerItem != null)
					continue;

				InnerItem.PaymentMethodShippingMethods.Add(paymentShippingToAdd);
			}

			if (!IsWizardMode)
				Repository.UnitOfWork.Commit();

		}

		#endregion

		#region protected members

		protected bool FilterItems(object item)
		{
			bool result = false;

			var shippingItem = item as ShippingMethod;

			if (!SelectedShippingMethods.Contains(shippingItem))
			{
				result = true;
			}

			return result;
		}

		protected void InitShipingMethods()
		{
			using (var shippingRepository = _shippingRepositoryFactory.GetRepositoryInstance())
			{
				var tmpList = new List<ShippingMethod>();
				foreach (var paymMethShipMethItem in InnerItem.PaymentMethodShippingMethods)
				{
					var shipMethFromDb =
						shippingRepository.ShippingMethods.Where(
							sm => sm.ShippingMethodId == paymMethShipMethItem.ShippingMethodId)
							.SingleOrDefault();
					if (shipMethFromDb != null)
					{
						tmpList.Add(shipMethFromDb);
					}
				}
				SelectedShippingMethods = new ObservableCollection<ShippingMethod>(tmpList);

				AllAvailableShippingMethods =
					shippingRepository.ShippingMethods.Expand(sm => sm.PaymentMethodShippingMethods).ToList();
				var view = CollectionViewSource.GetDefaultView(AllAvailableShippingMethods);
				view.Filter = FilterItems;

				if (Gateways == null)
				{
					Gateways = (Repository as IPaymentMethodRepository).PaymentGateways.ExpandAll().ToList();
				}
			}
		}

		protected void InitializeLanguage()
		{
			LanguagesStepViewModel = _langVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("selectedLanguages", InnerItem.PaymentMethodLanguages));
			OnPropertyChanged("LanguagesStepViewModel");
		}

		#endregion
	}
}
