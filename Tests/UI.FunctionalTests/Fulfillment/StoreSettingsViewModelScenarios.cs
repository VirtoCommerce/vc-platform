using System.Linq;
using CommerceFoundation.UI.FunctionalTests.TestHelpers;
using FunctionalTests.TestHelpers;
using UI.FunctionalTests.Helpers.Common;
using VirtoCommerce.Foundation.Data.Stores;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Interfaces;
using Xunit;

namespace CommerceFoundation.UI.FunctionalTests.Fulfillment
{
	[Variant(RepositoryProvider.DataService)]
	public class StoreSettingsViewModelScenarios : FunctionalUITestBase
	{
		#region Infrastructure/ setup

		public override void DefService()
		{
			ServManager.AddService(ServiceNameEnum.Store);
			ServManager.AddService(ServiceNameEnum.AppConfig);
			ServManager.AddService(ServiceNameEnum.Catalog);
			ServManager.AddService(ServiceNameEnum.Order);
		}

		#endregion

		[RepositoryTheory]
		public void Can_create_storeviewmodel_in_wizardmode()
		{
			var overviewVmFactory = new TestFulfillmentViewModelFactory<IStoreOverviewStepViewModel>(
				ServManager.GetUri(ServiceNameEnum.Store), ServManager.GetUri(ServiceNameEnum.Catalog),
				ServManager.GetUri(ServiceNameEnum.Order), ServManager.GetUri(ServiceNameEnum.AppConfig));

			var navigationVmFactory = new TestFulfillmentViewModelFactory<IStoreNavigationStepViewModel>(
				ServManager.GetUri(ServiceNameEnum.Store), ServManager.GetUri(ServiceNameEnum.Catalog),
				ServManager.GetUri(ServiceNameEnum.Order), ServManager.GetUri(ServiceNameEnum.AppConfig));

			var localizationVmFactory = new TestFulfillmentViewModelFactory<IStoreLocalizationStepViewModel>(
				ServManager.GetUri(ServiceNameEnum.Store), ServManager.GetUri(ServiceNameEnum.Catalog),
				ServManager.GetUri(ServiceNameEnum.Order), ServManager.GetUri(ServiceNameEnum.AppConfig));

			var taxesVmFactory = new TestFulfillmentViewModelFactory<IStoreTaxesStepViewModel>(
				ServManager.GetUri(ServiceNameEnum.Store), ServManager.GetUri(ServiceNameEnum.Catalog),
				ServManager.GetUri(ServiceNameEnum.Order), ServManager.GetUri(ServiceNameEnum.AppConfig));

			var paymentsVmFactory = new TestFulfillmentViewModelFactory<IStorePaymentsStepViewModel>(
				ServManager.GetUri(ServiceNameEnum.Store), ServManager.GetUri(ServiceNameEnum.Catalog),
				ServManager.GetUri(ServiceNameEnum.Order), ServManager.GetUri(ServiceNameEnum.AppConfig));


			var repositoryFactory =
				new DSRepositoryFactory<IStoreRepository, DSStoreClient, StoreEntityFactory>(
					ServManager.GetUri(ServiceNameEnum.Store));

			//create item using entity factory
			var entityFactory = new StoreEntityFactory();
			var item = entityFactory.CreateEntity<Store>();

			var createStoreViewModel = new CreateStoreViewModel(overviewVmFactory, localizationVmFactory, taxesVmFactory, paymentsVmFactory, navigationVmFactory, item);


			//check the default values in stepViewModel
			Assert.False(createStoreViewModel.AllRegisteredSteps[0].IsValid);
			Assert.False(createStoreViewModel.AllRegisteredSteps[1].IsValid);
			Assert.True(createStoreViewModel.AllRegisteredSteps[2].IsValid);
			Assert.True(createStoreViewModel.AllRegisteredSteps[3].IsValid);
			Assert.True(createStoreViewModel.AllRegisteredSteps[4].IsValid);

			//1 step
			//fill the properties in first step
			var overviewViewModel = createStoreViewModel.AllRegisteredSteps[0] as StoreViewModel;
			Assert.NotNull(overviewViewModel);
			overviewViewModel.InnerItem.Name = "TestName";
			overviewViewModel.InnerItem.Catalog = "TestCatalog";
			overviewViewModel.InitializeForOpen();
			Assert.True(createStoreViewModel.AllRegisteredSteps[0].IsValid);


			//2 step
			//fill the properties in second step
			var localizationStep = createStoreViewModel.AllRegisteredSteps[1] as StoreViewModel;
			Assert.NotNull(localizationStep);
			localizationStep.InnerItem.Languages.Add(new StoreLanguage()
			{
				LanguageCode = "ru-ru",
				StoreId = localizationStep.InnerItem.StoreId
			});
			localizationStep.InnerItem.DefaultLanguage = "ru-ru";

			localizationStep.InnerItem.Currencies.Add(new StoreCurrency()
			{
				CurrencyCode = "RUR",
				StoreId = localizationStep.InnerItem.StoreId
			});
			localizationStep.InnerItem.DefaultCurrency = "RUR";

			Assert.True(createStoreViewModel.AllRegisteredSteps[1].IsValid);


			//3 step
			//fill the properties in third step
			var taxesStep = createStoreViewModel.AllRegisteredSteps[2] as StoreTaxesStepViewModel;
			Assert.NotNull(taxesStep);
			taxesStep.InitializeForOpen();

			taxesStep.AvailableTaxCodes[0].IsChecked = true;
			taxesStep.AvailableTaxJurisdictions[0].IsChecked = true;

			Assert.True(taxesStep.IsValid);


			//4 step
			//fill the properties in 4 step
			var paymentsStep = createStoreViewModel.AllRegisteredSteps[3] as StorePaymentsStepViewModel;
			Assert.NotNull(paymentsStep);
			paymentsStep.InitializeForOpen();

			paymentsStep.AvailableStoreCardTypes[0].IsChecked = true;

			Assert.True(paymentsStep.IsValid);


			//5 step
			//fill the properties in 5 step
			var navigationStep = createStoreViewModel.AllRegisteredSteps[4] as StoreNavigationStepViewModel;
			Assert.NotNull(navigationStep);
			navigationStep.InitializeForOpen();

			navigationStep.SettingFilteredNavigation.LongTextValue = "TestnavigationText";
			Assert.True(navigationStep.IsValid);

			createStoreViewModel.PrepareAndSave();
			using (var repository = repositoryFactory.GetRepositoryInstance())
			{
				var itemFromDb =
					repository.Stores.Where(s => s.StoreId == item.StoreId).ExpandAll().SingleOrDefault();

				Assert.NotNull(itemFromDb);
				Assert.True(itemFromDb.Name == "TestName");
				Assert.True(itemFromDb.Catalog == "TestCatalog");
				Assert.True(itemFromDb.Languages.Any(x => x.LanguageCode == "ru-ru"));
				Assert.True(itemFromDb.Currencies.Any(x => x.CurrencyCode == "RUR"));
			}
		}

		[RepositoryTheory]
		public void create_storeviewmodel_in_detailmode_and_edit()
		{
			var overviewVmFactory = new TestFulfillmentViewModelFactory<IStoreOverviewStepViewModel>(
				ServManager.GetUri(ServiceNameEnum.Store), ServManager.GetUri(ServiceNameEnum.Catalog),
				ServManager.GetUri(ServiceNameEnum.Order), ServManager.GetUri(ServiceNameEnum.AppConfig));

			var navigationVmFactory = new TestFulfillmentViewModelFactory<IStoreNavigationStepViewModel>(
				ServManager.GetUri(ServiceNameEnum.Store), ServManager.GetUri(ServiceNameEnum.Catalog),
				ServManager.GetUri(ServiceNameEnum.Order), ServManager.GetUri(ServiceNameEnum.AppConfig));

			var localizationVmFactory = new TestFulfillmentViewModelFactory<IStoreLocalizationStepViewModel>(
				ServManager.GetUri(ServiceNameEnum.Store), ServManager.GetUri(ServiceNameEnum.Catalog),
				ServManager.GetUri(ServiceNameEnum.Order), ServManager.GetUri(ServiceNameEnum.AppConfig));

			var taxesVmFactory = new TestFulfillmentViewModelFactory<IStoreTaxesStepViewModel>(
				ServManager.GetUri(ServiceNameEnum.Store), ServManager.GetUri(ServiceNameEnum.Catalog),
				ServManager.GetUri(ServiceNameEnum.Order), ServManager.GetUri(ServiceNameEnum.AppConfig));

			var paymentsVmFactory = new TestFulfillmentViewModelFactory<IStorePaymentsStepViewModel>(
				ServManager.GetUri(ServiceNameEnum.Store), ServManager.GetUri(ServiceNameEnum.Catalog),
				ServManager.GetUri(ServiceNameEnum.Order), ServManager.GetUri(ServiceNameEnum.AppConfig));

			var linkedStoresVmFactory = new TestFulfillmentViewModelFactory<IStoreLinkedStoresStepViewModel>(
				ServManager.GetUri(ServiceNameEnum.Store), ServManager.GetUri(ServiceNameEnum.Catalog),
				ServManager.GetUri(ServiceNameEnum.Order), ServManager.GetUri(ServiceNameEnum.AppConfig));

			var settingVmFactory = new TestFulfillmentViewModelFactory<IStoreSettingStepViewModel>(
				ServManager.GetUri(ServiceNameEnum.Store), ServManager.GetUri(ServiceNameEnum.Catalog),
				ServManager.GetUri(ServiceNameEnum.Order), ServManager.GetUri(ServiceNameEnum.AppConfig));

			var entityFactory = new StoreEntityFactory();
			var item = entityFactory.CreateEntity<Store>();

			var repositoryFactory =
				new DSRepositoryFactory<IStoreRepository, DSStoreClient, StoreEntityFactory>(
					ServManager.GetUri(ServiceNameEnum.Store));

			var navigationManager = new TestNavigationManager();



			//fill the properties of InnerItem;
			item.Name = "testName";
			item.Catalog = "testcatalog";
			item.Languages.Add(new StoreLanguage() { LanguageCode = "ru-ru", StoreId = item.StoreId });
			item.DefaultLanguage = "ru-ru";
			item.Currencies.Add(new StoreCurrency() { CurrencyCode = "RUR", StoreId = item.StoreId });
			item.DefaultCurrency = "RUR";

			using (var repository = repositoryFactory.GetRepositoryInstance())
			{
				repository.Add(item);
				repository.UnitOfWork.Commit();
			}


			var detailStoreViewModel = new StoreViewModel(repositoryFactory, entityFactory, overviewVmFactory, localizationVmFactory, taxesVmFactory, paymentsVmFactory, navigationVmFactory, settingVmFactory, linkedStoresVmFactory, null, null,
				navigationManager, item);
			Assert.NotNull(detailStoreViewModel);
			detailStoreViewModel.InitializeForOpen();


			//edit various properties
			detailStoreViewModel.InnerItem.Name = "EditingName";
			detailStoreViewModel.InnerItem.Catalog = "EditedCatalog";


			detailStoreViewModel.InnerItem.Languages.Add(new StoreLanguage()
			{
				LanguageCode = "de-de",
				StoreId = detailStoreViewModel.InnerItem.StoreId
			});
			detailStoreViewModel.InnerItem.DefaultLanguage = "de-de";

			detailStoreViewModel.InnerItem.Currencies.Add(new StoreCurrency()
			{
				CurrencyCode = "USD",
				StoreId = detailStoreViewModel.InnerItem.StoreId
			});


			detailStoreViewModel.TaxesStepViewModel.AvailableTaxCodes[0].IsChecked = true;
			detailStoreViewModel.TaxesStepViewModel.AvailableTaxCodes[1].IsChecked = true;
			detailStoreViewModel.TaxesStepViewModel.AvailableTaxJurisdictions[0].IsChecked = true;

			detailStoreViewModel.PaymentsStepViewModel.AvailableStoreCardTypes[0].IsChecked = true;

			(detailStoreViewModel.NavigationStepViewModel as StoreNavigationStepViewModel).SettingFilteredNavigation
				.LongTextValue = "NewNavigationText";

			detailStoreViewModel.SaveWithoutUIChanges();


			Store storeFromDb = null;
			using (var repository = repositoryFactory.GetRepositoryInstance())
			{
				storeFromDb =
					repository.Stores.Where(s => s.StoreId == detailStoreViewModel.InnerItem.StoreId).SingleOrDefault();

				Assert.NotNull(storeFromDb);
				Assert.True(storeFromDb.Name == "EditingName");
			}


			//edit various properties

			var detailStoreViewModel2 = new StoreViewModel(repositoryFactory, entityFactory, overviewVmFactory, localizationVmFactory, taxesVmFactory, paymentsVmFactory, navigationVmFactory, settingVmFactory, linkedStoresVmFactory, null, null,
			   navigationManager, item);
			Assert.NotNull(detailStoreViewModel2);
			detailStoreViewModel2.InitializeForOpen();

			detailStoreViewModel.InnerItem.Name = "2 edit";
			detailStoreViewModel.TaxesStepViewModel.AvailableTaxCodes[0].IsChecked = false;
			detailStoreViewModel.TaxesStepViewModel.AvailableTaxCodes[1].IsChecked = false;

			detailStoreViewModel.InnerItem.Settings.Add(new StoreSetting()
			{
				Name = "testSettings",
				ValueType = "0",
				ShortTextValue = "ShortTextValue",
				StoreId = detailStoreViewModel.InnerItem.StoreId
			});


			detailStoreViewModel.SaveWithoutUIChanges();


			using (var repository = repositoryFactory.GetRepositoryInstance())
			{
				var itemFromDb =
					repository.Stores.Where(s => s.StoreId == detailStoreViewModel.InnerItem.StoreId).Expand(s => s.Settings).SingleOrDefault();

				Assert.NotNull(itemFromDb);
				Assert.True(itemFromDb.Name == "2 edit");


				var setting = itemFromDb.Settings.SingleOrDefault(ss => ss.Name == "testSettings");
				Assert.NotNull(setting);

			}

		}
	}
}
