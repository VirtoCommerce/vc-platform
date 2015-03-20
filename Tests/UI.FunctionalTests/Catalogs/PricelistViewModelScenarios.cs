using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FunctionalTests.Catalogs;
using FunctionalTests.TestHelpers;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using UI.FunctionalTests.Helpers.Common;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Implementations;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using CommerceFoundation.UI.FunctionalTests.TestHelpers;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;
using Xunit;

namespace CommerceFoundation.UI.FunctionalTests.Catalogs
{

	[Variant(RepositoryProvider.DataService)]
	public class PricelistViewModelScenarios : FunctionalUITestBase
	{
		#region Infrastructure/setup

		public override void DefService()
		{
			ServManager.AddService(ServiceNameEnum.AppConfig);
			ServManager.AddService(ServiceNameEnum.Catalog);
		}

		#endregion

		#region PriceList property test values
		//pricelist property values
		private const string plName = "Price list";
		private const string plNewName = "New price list name";
		private const string plDescription = "Price list Description";
		#endregion

		protected void CreateFullGraphCatalog(ICatalogRepository client, ref Item[] items, string catalogId)
		{
			var catalogBuilder = CatalogBuilder.BuildCatalog(catalogId).WithCategory("category").WithProducts(2);
			var catalog = catalogBuilder.GetCatalog();
			items = catalogBuilder.GetItems();

			client.Add(catalog);

			foreach (var item in items)
			{
				client.Add(item);
			}

			client.UnitOfWork.Commit();

		}



		[RepositoryTheory]
		public void AddCurrencies()
		{
			// create Setting from factory
			var entityFactory = new AppConfigEntityFactory();
			var setting = entityFactory.CreateEntity<Setting>();
			setting.Name = "Currencies";
			setting.SettingValueType = "ShortText";
			setting.IsMultiValue = true;
			setting.IsSystem = true;

			// add currencies
			var id = setting.SettingId;
			setting.SettingValues.Add(new SettingValue() { ValueType = "ShortText", ShortTextValue = "USD", SettingId = id });
			setting.SettingValues.Add(new SettingValue() { ValueType = "ShortText", ShortTextValue = "EUR", SettingId = id });


			var appConfigFactory = new DSRepositoryFactory<IAppConfigRepository, DSAppConfigClient, AppConfigEntityFactory>(ServManager.GetUri(ServiceNameEnum.AppConfig));
			using (var appConfigRepository = appConfigFactory.GetRepositoryInstance())
			{
				appConfigRepository.Add(setting);
				appConfigRepository.UnitOfWork.Commit();
			}
		}

		[RepositoryTheory]
		public void Can_add_pricelist()
		{
			// create ViewModelsFactory ( it should be resolve all view models for the test)
			var overviewVmFactory = new TestCatalogViewModelFactory<IPriceListOverviewStepViewModel>(ServManager.GetUri(ServiceNameEnum.Catalog), ServManager.GetUri(ServiceNameEnum.AppConfig));

			// create Item using EntityFactory
			var entityFactory = new CatalogEntityFactory();
			var item = entityFactory.CreateEntity<Pricelist>();

			// create Wizard main class. Constructor of the class creates wizard steps with help vmFactory
			var createPriceListViewModel = new CreatePriceListViewModel(overviewVmFactory, item);

			// IsValid of wizard step should be false at the begin.
			Assert.False(createPriceListViewModel.AllRegisteredSteps[0].IsValid);

			var step = createPriceListViewModel.AllRegisteredSteps[0] as PriceListOverviewStepViewModel;
			step.InitializeForOpen();
			step.InnerItem.Name = "New test PriceList";
			Assert.Null(step.AllAvailableCurrencies);
			step.InnerItem.Currency = "USD";
			Assert.True(step.IsValid);
			createPriceListViewModel.PrepareAndSave();

			var priceListRepositoryFactory = new DSRepositoryFactory<IPricelistRepository, DSCatalogClient, CatalogEntityFactory>(ServManager.GetUri(ServiceNameEnum.Catalog));
			using (var repository = priceListRepositoryFactory.GetRepositoryInstance())
			{
				var checkItem = repository.Pricelists.Where(x => x.Name == "New test PriceList").FirstOrDefault();
				Assert.NotNull(checkItem);
			}
		}

		[RepositoryTheory(Skip = "The test fails appdomain unload, remove wait parameters")]
		public void Can_delete_pricelist()
		{
			#region Init parameters for PriceListHomeViewModel

			var priceListRepositoryFactory =
				new DSRepositoryFactory<IPricelistRepository, DSCatalogClient, CatalogEntityFactory>(
					ServManager.GetUri(ServiceNameEnum.Catalog));
			IAuthenticationContext authenticationContext = new TestAuthenticationContext();
			var navigationManager = new TestNavigationManager();

			// create ViewModelsFactory ( it should be resolve all view models for the test)
			var itemVmFactory = new TestCatalogViewModelFactory<IPriceListViewModel>(ServManager.GetUri(ServiceNameEnum.Catalog),
																		   ServManager.GetUri(ServiceNameEnum.AppConfig));

			var wizardVmFactory = new TestCatalogViewModelFactory<ICreatePriceListViewModel>(ServManager.GetUri(ServiceNameEnum.Catalog),
																		   ServManager.GetUri(ServiceNameEnum.AppConfig));

			// create Item using EntityFactory
			var entityFactory = new CatalogEntityFactory();

			#endregion

			#region Add price list to DB

			using (var repository = priceListRepositoryFactory.GetRepositoryInstance())
			{
				var pricelist = entityFactory.CreateEntity<Pricelist>();
				pricelist.Name = "Test price (Can_delete_pricelist)";
				pricelist.Currency = "USD";

				repository.Add(pricelist);
				repository.UnitOfWork.Commit();
			}

			#endregion

			#region VM test

			var priceListHomeViewModel = new PriceListHomeViewModel(entityFactory, itemVmFactory, wizardVmFactory,
																	priceListRepositoryFactory, authenticationContext,
																	navigationManager, null);
			priceListHomeViewModel.InitializeForOpen();

			Thread.Sleep(3000); // waiting for InitializeForOpen to finish in background thread

			priceListHomeViewModel.CommonConfirmRequest.Raised += DeletePriceListConfirmation;
			priceListHomeViewModel.ListItemsSource.MoveCurrentToFirst();
			var item = priceListHomeViewModel.ListItemsSource.CurrentItem as VirtualListItem<IPriceListViewModel>;
			var itemsToDelete = new List<VirtualListItem<IPriceListViewModel>>() { item };
			priceListHomeViewModel.ItemDeleteCommand.Execute(itemsToDelete);

			Thread.Sleep(1000);// waiting for ItemDeleteCommand to finish in background thread

			#endregion

			#region Check

			using (var repository = priceListRepositoryFactory.GetRepositoryInstance())
			{
				var checkItem = repository.Pricelists.Where(x => x.Name == "Test price (Can_delete_pricelist)").SingleOrDefault();
				Assert.Null(checkItem);
			}

			#endregion
		}

		private void DeletePriceListConfirmation(object o, InteractionRequestedEventArgs args)
		{
			(args.Context as Confirmation).Confirmed = true;
			args.Callback.Invoke();
		}

		[Fact(Skip = "not fully implemented")]
		public void Can_save_pricelist_name()
		{
			//var factory = new TestPricelistRepositoryFactory1<IPricelistRepository>();
			//var catalogFactory = new TestCatalogRepositoryFactory1<ICatalogRepository>();
			//var appConfigFactory = new TestAppConfigRepositoryFactory<IAppConfigRepository>();
			//var catalogRepository = catalogFactory.GetRepositoryInstance();
			//var items = new Item[] { };
			//CreateFullGraphCatalog(catalogRepository, ref items, Guid.NewGuid().ToString());

			//var pricelistItem = new Pricelist() {Name = plName};
			//var priceListEditVM = new PriceListViewModel(factory, appConfigFactory, null, new CatalogEntityFactory(), null, null,
			//											 pricelistItem);

			//priceListEditVM.InnerItem.Name = plNewName;
			//Assert.Equal(plNewName, priceListEditVM.DisplayName);

			//priceListEditVM.SaveChangesCommand.Execute(null);
			//priceListEditVM.InitializeForOpen();
			//Assert.Equal(plNewName, priceListEditVM.InnerItem.Name);

			//catalogRepository.Dispose();
		}

		#region Help


		#endregion
	}

}
