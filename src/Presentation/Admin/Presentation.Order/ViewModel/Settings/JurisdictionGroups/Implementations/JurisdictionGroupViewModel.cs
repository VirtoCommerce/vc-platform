using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.JurisdictionGroups.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.JurisdictionGroups.Implementations
{
	public class JurisdictionGroupViewModel : ViewModelDetailAndWizardBase<JurisdictionGroup>, IJurisdictionGroupViewModel
	{
		#region Dependencies

		private readonly JurisdictionTypes _jurisdictionType;
		private readonly IHomeSettingsViewModel _parent;
		private readonly INavigationManager _navManager;
		private readonly IRepositoryFactory<IOrderRepository> _repositoryFactory;

		#endregion

		#region Constructor

		public JurisdictionGroupViewModel(IRepositoryFactory<IOrderRepository> repositoryFactory, IOrderEntityFactory entityFactory,
			INavigationManager navManager, IHomeSettingsViewModel parent, JurisdictionTypes jurisdictionType, JurisdictionGroup item)
			: base(entityFactory, item, false)
		{
			_repositoryFactory = repositoryFactory;
			_parent = parent;
			_navManager = navManager;
			_jurisdictionType = jurisdictionType;
            ViewTitle = new ViewTitleBase { SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory), Title = "Jurisdiction Group" };

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));

		}

		protected JurisdictionGroupViewModel(IRepositoryFactory<IOrderRepository> repositoryFactory, IOrderEntityFactory entityFactory, JurisdictionTypes jurisdictionType, JurisdictionGroup item)
			: base(entityFactory, item, true)
		{
			_repositoryFactory = repositoryFactory;
			_jurisdictionType = jurisdictionType;
		}

		#endregion

		#region ViewModelBase Members

		public override string DisplayName
		{
			get
			{
				return OriginalItem.DisplayName;
			}
		}

		public override string IconSource
		{
			get
			{
				return "Icon_JurisdictionGroup";
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
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.JurisdictionGroupId),
						Configuration.NavigationNames.HomeName, NavigationNames.MenuName, this));
			}
		}

		#endregion

		#region ViewModelDetailAndWizardBase Members

		public override string ExceptionContextIdentity { get { return string.Format("Jurisdiction group ({0})", DisplayName); } }

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
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
				Content = string.Format("Save changes to JurisdictionGroup '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			var item = (Repository as IOrderRepository).JurisdictionGroups
				.Where(x => x.JurisdictionGroupId == OriginalItem.JurisdictionGroupId)
				.Expand("JurisdictionRelations/Jurisdiction")
				.SingleOrDefault();
			OnUIThread(() => { InnerItem = item; });
		}

		protected override void InitializePropertiesForViewing()
		{
			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				var items = repository.Jurisdictions.Where(x =>
														   x.JurisdictionType == (int)JurisdictionTypes.All ||
														   x.JurisdictionType == (int)_jurisdictionType);
				OnUIThread(() =>
					{
						AllAvailableJurisdictions.SetItems(items);

						SelectedJurisdictions.SetItems(InnerItem.JurisdictionRelations.Select(x => x.Jurisdiction));

						var view = CollectionViewSource.GetDefaultView(AllAvailableJurisdictions);
						view.Filter = FilterItems;
					});
			}
		}

		protected override void BeforeSaveChanges()
		{
			if (!IsWizardMode)
			{
				UptadeOfPaymentShipping();
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

		protected override void SetSubscriptionUI()
		{
			if (SelectedJurisdictions != null)
			{
				SelectedJurisdictions.CollectionChanged += ViewModel_PropertyChanged;
			}
		}

		protected override void CloseSubscriptionUI()
		{
			if (SelectedJurisdictions != null)
			{
				SelectedJurisdictions.CollectionChanged -= ViewModel_PropertyChanged;
			}
		}

		#endregion

		#region IJurisdictionGroupViewModel Members

		private JurisdictionTypes[] _allAvailableJurisdictionTypes;
		public JurisdictionTypes[] AllAvailableJurisdictionTypes
		{
			get
			{
				if (_allAvailableJurisdictionTypes == null)
				{
					_allAvailableJurisdictionTypes = new[]
                        {
                            _jurisdictionType,
                            JurisdictionTypes.All
                        };
				}
				return _allAvailableJurisdictionTypes;
			}
		}

		private List<Jurisdiction> _allAvailableJurisdictions = new List<Jurisdiction>();
		public List<Jurisdiction> AllAvailableJurisdictions
		{
			get { return _allAvailableJurisdictions; }
			set
			{
				_allAvailableJurisdictions = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<Jurisdiction> _selectedJurisdictions = new ObservableCollection<Jurisdiction>();
		public ObservableCollection<Jurisdiction> SelectedJurisdictions
		{
			get { return _selectedJurisdictions; }
			set
			{
				_selectedJurisdictions = value;
				OnPropertyChanged();
			}
		}

		public void UptadeOfPaymentShipping()
		{
			var selectedPaymentShipping = new List<JurisdictionRelation>();

			foreach (var selectedJurisdiction in SelectedJurisdictions)
			{
				var itemToAdd = new JurisdictionRelation
				{
					JurisdictionId = selectedJurisdiction.JurisdictionId,
					JurisdictionGroupId = InnerItem.JurisdictionGroupId
				};

				selectedPaymentShipping.Add(itemToAdd);
			}

			var paymentShippingForDelete =
				InnerItem.JurisdictionRelations.Where(
					x => selectedPaymentShipping.All(ps => ps.JurisdictionId != x.JurisdictionId)).ToList();

			foreach (var paymentShippingToDelete in paymentShippingForDelete)
			{
				InnerItem.JurisdictionRelations.Remove(paymentShippingToDelete);
				//repository.Attach(paymentShippingToDelete);
				//repository.Remove(paymentShippingToDelete);
			}

			foreach (var paymentShippingToAdd in selectedPaymentShipping)
			{
				var sameItemFromInnerItem =
					InnerItem.JurisdictionRelations.SingleOrDefault(
						pmsm =>
						pmsm.JurisdictionGroupId == paymentShippingToAdd.JurisdictionGroupId &&
						pmsm.JurisdictionId == paymentShippingToAdd.JurisdictionId);

				if (sameItemFromInnerItem != null)
					continue;

				InnerItem.JurisdictionRelations.Add(paymentShippingToAdd);
			}

		}

		#endregion

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				bool result = false;

				result = InnerItem.Validate(false) && !string.IsNullOrEmpty(InnerItem.Code) &&
						 !string.IsNullOrEmpty(InnerItem.DisplayName);

				return result;
			}
		}

		public override bool IsLast
		{
			get { return true; }
		}

		public override string Description
		{
			get { return "Enter Jurisdiction Group information.".Localize(); }
		}

		#endregion

		#region Private members

		private bool FilterItems(object item)
		{
			bool result = false;
			var typedItem = item as Jurisdiction;

			if (typedItem != null && SelectedJurisdictions.All(x => x.JurisdictionId != typedItem.JurisdictionId))
			{
				result = true;
			}

			return result;
		}

		#endregion
	}
}
