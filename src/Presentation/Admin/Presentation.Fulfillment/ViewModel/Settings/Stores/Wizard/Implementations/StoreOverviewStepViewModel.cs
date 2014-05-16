using System;
using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Implementations
{
	public class StoreOverviewStepViewModel : StoreViewModel, IStoreOverviewStepViewModel
	{
		#region Dependencies

		private readonly IRepositoryFactory<ICatalogRepository> _catalogRepositoryFactory;
		private readonly IRepositoryFactory<ICountryRepository> _countryRepositoryFactory;
		private readonly IRepositoryFactory<IFulfillmentCenterRepository> _fulfillmetnRepositoryFactory;

		#endregion

		public StoreOverviewStepViewModel(IStoreEntityFactory entityFactory, Store item,
			IRepositoryFactory<IStoreRepository> repositoryFactory,
			IRepositoryFactory<ICatalogRepository> catalogRepositoryFactory, IRepositoryFactory<ICountryRepository> countryRepositoryFactory,
			IRepositoryFactory<IFulfillmentCenterRepository> fulfillmentRepositoryFactory)
			: base(repositoryFactory, entityFactory, item)
		{
			_catalogRepositoryFactory = catalogRepositoryFactory;
			_countryRepositoryFactory = countryRepositoryFactory;
			_fulfillmetnRepositoryFactory = fulfillmentRepositoryFactory;
		}

		#region IStoreOverviewStepViewModel Members

		public CatalogBase[] AvailableCatalogs { get; private set; }
		public ReadOnlyCollection<TimeZoneInfo> AvailableTimezones { get; private set; }
		public object[] AvailableCountries { get; private set; }
		public FulfillmentCenter[] AvailableFulfillmentCenters { get; private set; }
		public FulfillmentCenter[] AvailableReturnFulfillmentCenters { get; private set; }

		#endregion

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				var retval = true;
				bool doNotifyChanges = false;

				InnerItem.Validate(doNotifyChanges);
				if (InnerItem.Errors.ContainsKey("Name") || InnerItem.Errors.ContainsKey("Catalog"))
				{
					retval = false;
					InnerItem.Errors.Clear();
				}

				return retval;
			}
		}

		public override bool IsLast
		{
			get
			{
				return false;
			}
		}

		public override string Description
		{
			get
			{
				return "Enter store general information.".Localize();
			}
		}
		#endregion



		protected override void InitializePropertiesForViewing()
		{
			if (AvailableCatalogs == null)
			{
				using (var catalogRepository = _catalogRepositoryFactory.GetRepositoryInstance())
				{
					AvailableCatalogs = catalogRepository.Catalogs.OrderBy(x => x.Name).ToArray();
					OnPropertyChanged("AvailableCatalogs");
				}

				AvailableTimezones = TimeZoneInfo.GetSystemTimeZones();
				OnPropertyChanged("AvailableTimezones");


				using (var countryRepository = _countryRepositoryFactory.GetRepositoryInstance())
				{
					AvailableCountries = countryRepository.Countries.OrderBy(x => x.Name).Expand(x => x.Regions).ToArray();
					OnPropertyChanged("AvailableCountries");
				}

				using (var fulfillmentCenterRepository = _fulfillmetnRepositoryFactory.GetRepositoryInstance())
				{
					AvailableFulfillmentCenters = fulfillmentCenterRepository.FulfillmentCenters.ToArray();
					OnPropertyChanged("AvailableFulfillmentCenters");
					AvailableReturnFulfillmentCenters = AvailableFulfillmentCenters;
					OnPropertyChanged("AvailableReturnFulfillmentCenters");
				}
			}
		}
	}
}
