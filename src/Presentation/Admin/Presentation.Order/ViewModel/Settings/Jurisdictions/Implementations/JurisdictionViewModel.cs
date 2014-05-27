using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model.Countries;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Jurisdictions.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Jurisdictions.Implementations
{
	public class JurisdictionViewModel : ViewModelDetailAndWizardBase<Jurisdiction>, IJurisdictionViewModel
	{
		#region Dependencies

		private readonly INavigationManager _navManager;
		private readonly IHomeSettingsViewModel _parent;
		private readonly IRepositoryFactory<IOrderRepository> _repositoryFactory;
		private readonly IRepositoryFactory<ICountryRepository> _countryRepositoryFactory;

		#endregion

		private readonly JurisdictionTypes _jurisdictionType;

		#region Constructor

		public JurisdictionViewModel(IRepositoryFactory<IOrderRepository> repositoryFactory, IRepositoryFactory<ICountryRepository> countryRepositoryFactory,
			IOrderEntityFactory entityFactory, IHomeSettingsViewModel parent, INavigationManager navManager,
			JurisdictionTypes jurisdictionType, Jurisdiction item)
			: base(entityFactory, item, false)
		{
			_repositoryFactory = repositoryFactory;
			_countryRepositoryFactory = countryRepositoryFactory;
			_navManager = navManager;
			_parent = parent;
			_jurisdictionType = jurisdictionType;
            ViewTitle = new ViewTitleBase { SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory), Title = "Jurisdiction" };

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));

		}

		protected JurisdictionViewModel(IRepositoryFactory<IOrderRepository> repositoryFactory, IRepositoryFactory<ICountryRepository> countryRepositoryFactory,
			IOrderEntityFactory entityFactory, JurisdictionTypes jurisdictionType, Jurisdiction item)
			: base(entityFactory, item, true)
		{
			_repositoryFactory = repositoryFactory;
			_countryRepositoryFactory = countryRepositoryFactory;
			_jurisdictionType = jurisdictionType;
		}

		#endregion

		#region ViewModelBase Members

		public override string DisplayName
		{
			get
			{
				//todo check another view model to use OriginalItem
				return OriginalItem != null ? OriginalItem.DisplayName : string.Empty;
			}
		}

		public override string IconSource
		{
			get
			{
				return "Icon_Jurisdiction";
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
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.JurisdictionId),
															Configuration.NavigationNames.HomeName, NavigationNames.MenuName, this));
			}
		}

		#endregion

		#region ViewModelDetailAndWizardBase Members

		public override string ExceptionContextIdentity { get { return string.Format("Jurisdiction ({0})", DisplayName); } }

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
				Content = string.Format("Save changes to Jurisdiction '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}


		protected override void LoadInnerItem()
		{
			var item =
				(Repository as IOrderRepository).Jurisdictions.Where(j => j.JurisdictionId == OriginalItem.JurisdictionId)
					.SingleOrDefault();

			OnUIThread(() => InnerItem = item);
		}

		protected override void InitializePropertiesForViewing()
		{
			if (AllCountries == null)
			{
				//Get All Countries
				using (var repository = _countryRepositoryFactory.GetRepositoryInstance())
				{
					var countries = repository.Countries.Expand(c => c.Regions).OrderBy(c => c.Name).ToArray();
					OnUIThread(() => { AllCountries = countries; });
				}
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

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				bool result = false;

				result = InnerItem.Validate(false) && !string.IsNullOrEmpty(InnerItem.Code) &&
						 !string.IsNullOrEmpty(InnerItem.CountryCode);

				return result;
			}
		}

		public override bool IsLast
		{
			get { return true; }
		}

		public override string Description
		{
			get { return "Enter Jurisdiction information.".Localize(); }
		}


		#endregion

		#region IJurisdictionViewModel Members

		private JurisdictionTypes[] _allAvailableJurisdictionTypes;
		public JurisdictionTypes[] AllAvailableJurisdictionTypes
		{
			get
			{
				if (_allAvailableJurisdictionTypes == null)
				{
					_allAvailableJurisdictionTypes = new JurisdictionTypes[]
                        {
                            _jurisdictionType,
                            JurisdictionTypes.All
                        };
				}
				return _allAvailableJurisdictionTypes;
			}
		}

		private Country[] _allCountries;
		public Country[] AllCountries
		{
			get { return _allCountries; }
			set { _allCountries = value; OnPropertyChanged(); }
		}

		#endregion

	}
}
