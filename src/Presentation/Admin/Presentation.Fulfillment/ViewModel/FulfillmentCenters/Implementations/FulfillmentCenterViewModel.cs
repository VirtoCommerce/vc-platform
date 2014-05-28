using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Model.Countries;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.FulfillmentCenters.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.FulfillmentCenters.Implementations
{
	public class FulfillmentCenterViewModel : ViewModelDetailAndWizardBase<FulfillmentCenter>, IFulfillmentCenterViewModel
	{

		#region Dependencies

		private readonly INavigationManager _navManager;
		private readonly IHomeSettingsViewModel _parent;
		private readonly IRepositoryFactory<IFulfillmentCenterRepository> _repositoryFactory;
		private readonly IRepositoryFactory<ICountryRepository> _countryRepositoryFactory;

		#endregion

		#region constructor


		public FulfillmentCenterViewModel(IRepositoryFactory<IFulfillmentCenterRepository> repositoryFactory, IRepositoryFactory<ICountryRepository> countryRepositoryFactory,
			IStoreEntityFactory entityFactory, IHomeSettingsViewModel parent,
			INavigationManager navManager, FulfillmentCenter item)
			: base(entityFactory, item, false)
		{
            ViewTitle = new ViewTitleBase { SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory), Title = "Fulfillment center" };
			_repositoryFactory = repositoryFactory;
			_countryRepositoryFactory = countryRepositoryFactory;
			_navManager = navManager;
			_parent = parent;

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
		}


		protected FulfillmentCenterViewModel(IRepositoryFactory<IFulfillmentCenterRepository> repositoryFactory, IRepositoryFactory<ICountryRepository> countryRepositoryFactory,
			IStoreEntityFactory entityFactory, FulfillmentCenter item)
			: base(entityFactory, item, true)
		{
			_repositoryFactory = repositoryFactory;
			_countryRepositoryFactory = countryRepositoryFactory;
		}

		#endregion

		#region ViewModelBase members

		public override string DisplayName
		{
			get
			{
				return (InnerItem == null) ? string.Empty : InnerItem.Name;
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
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.FulfillmentCenterId),
						Configuration.NavigationNames.HomeName, NavigationNames.MenuName, this));
			}
		}

		#endregion

		#region ViewModelDetailAndWizardBase members

		public override string ExceptionContextIdentity { get { return string.Format("Fulfillment ({0})", DisplayName); } }

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override bool IsValidForSave()
		{
			bool result = InnerItem.Validate() && !string.IsNullOrEmpty(InnerItem.City) && !string.IsNullOrEmpty(InnerItem.CountryCode) &&
					 !string.IsNullOrEmpty(InnerItem.DaytimePhoneNumber)
					 && !string.IsNullOrEmpty(InnerItem.Line1)
					 && !string.IsNullOrEmpty(InnerItem.PostalCode)
					 && !string.IsNullOrEmpty(InnerItem.Name) && !string.IsNullOrEmpty(InnerItem.Description);

			return result;
		}

		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to Fulfillment center '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			var item =
				(Repository as IFulfillmentCenterRepository).FulfillmentCenters.Where(
					fc => fc.FulfillmentCenterId == OriginalItem.FulfillmentCenterId).SingleOrDefault();

			OnUIThread(() => InnerItem = item);
		}

		protected async override void InitializePropertiesForViewing()
		{
			var countryRepository = _countryRepositoryFactory.GetRepositoryInstance();
			{
				AllCountries = await Task.Run(() => countryRepository.Countries.Expand("Regions").OrderBy(x => x.Name).ToArray());
			}

			OnPropertyChanged("SelectedCountry");
			OnPropertyChanged("SelectedRegion");
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

		private bool _isValid;
		public override bool IsValid
		{
			get
			{
				if (this is IFulfillmentCenterOverviewStepViewModel)
				{
					_isValid = !string.IsNullOrEmpty(InnerItem.Description) && !string.IsNullOrEmpty(InnerItem.Name);
				}
				else if (this is IFulfillmentCenterAddressStepViewModel)
				{
					_isValid = !string.IsNullOrEmpty(InnerItem.CountryCode) && !string.IsNullOrEmpty(InnerItem.City)
							   && !string.IsNullOrEmpty(InnerItem.DaytimePhoneNumber)
							   && !string.IsNullOrEmpty(InnerItem.Line1)
							   && !string.IsNullOrEmpty(InnerItem.PostalCode);
				}

				return _isValid;
			}
		}

		public override bool IsLast
		{
			get
			{
				return this is IFulfillmentCenterAddressStepViewModel;
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
				var result = string.Empty;
				if (this is IFulfillmentCenterOverviewStepViewModel)
				{
					result = string.Format("Enter fulfillment center details".Localize());
				}
				else if (this is IFulfillmentCenterAddressStepViewModel)
					result = "Enter fulfillment center contact information".Localize();

				return result;
			}
		}

		#endregion

		#region IFulfillmentCenterViewModel Members

		private Country _selectedCountry;
		public Country SelectedCountry
		{
			get
			{
				if (_selectedCountry == null && AllCountries != null)
				{
					var c =
						AllCountries.SingleOrDefault(coun => ((Country)coun).CountryId == InnerItem.CountryCode) as
						Country;
					if (c != null)
					{
						_selectedCountry = c;
					}

				}
				return _selectedCountry;
			}
			set
			{
				if (_selectedCountry != value)
				{
					_selectedCountry = value;
					OnPropertyChanged();

					//set InnerItem CountryDetails
					if (value != null && !string.IsNullOrEmpty(value.Name) && value.DisplayName != "Select country...".Localize())
					{
						InnerItem.CountryCode = value.CountryId;
						InnerItem.CountryName = value.DisplayName;
					}
				}
			}
		}

		private Region _selectedRegion;
		public Region SelectedRegion
		{
			get
			{
				if (_selectedRegion == null && InnerItem.CountryCode != null && AllCountries != null)
				{

					var country =
						AllCountries.SingleOrDefault(c => ((Country)c).CountryId == InnerItem.CountryCode) as
						Country;

					if (country != null)
					{
						var regions = country.Regions;

						if (InnerItem.StateProvince != null)
						{
							var region =
								regions.SingleOrDefault(r => r.RegionId == InnerItem.StateProvince);
							_selectedRegion = region;
						}
					}

				}

				return _selectedRegion;
			}
			set
			{
				if (_selectedRegion != value)
				{
					_selectedRegion = value;
					OnPropertyChanged();
					//set InnerItem REgionDetails
					if (value != null && !string.IsNullOrEmpty(value.Name) && value.DisplayName != "Select state...".Localize())
					{
						InnerItem.StateProvince = value.RegionId;
					}

				}
			}
		}


		private object[] _allCountries;
		public object[] AllCountries
		{
			get
			{
				return _allCountries;
			}
			set
			{
				_allCountries = value;
				OnPropertyChanged();
			}
		}

		#endregion


	}
}
