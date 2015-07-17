using System;
using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.CoreModule.Data.Repositories;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.DynamicProperties;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.StoreModule.Data.Repositories;
using VirtoCommerce.StoreModule.Data.Services;
using VirtoCommerce.StoreModule.Web.Controllers.Api;
using VirtoCommerce.StoreModule.Web.Model;

namespace VirtoCommerce.StoreModule.Test
{
    [TestClass]
    public class StoreControllerTest
    {
        [TestMethod]
        public void GetStoreListTest()
        {
            var controller = GetStoreController();
            var result = controller.GetStores() as OkNegotiatedContentResult<Store[]>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void CreateNewStore()
        {
            var controller = GetStoreController();
            var store = new Store
            {
                Id = "testStore",
                Name = "testStore",
                Catalog = "catalog",
                Currencies = new[] { CurrencyCodes.USD, CurrencyCodes.RUB },
                DefaultCurrency = CurrencyCodes.USD,
                Languages = new[] { "ru-ru", "en-us" },
                DefaultLanguage = "ru-ru",
                FulfillmentCenter = new FulfillmentCenter
                {
                    City = "New York",
                    CountryCode = "USA",
                    Line1 = "line1",
                    DaytimePhoneNumber = "+821291921",
                    CountryName = "USA",
                    Name = "Name",
                    StateProvince = "State",
                    PostalCode = "code"
                },
                //PaymentGateways = new string[] { "PayPal", "Clarna" },
                StoreState = Domain.Store.Model.StoreState.Open,
                Settings = new[] { new Setting { Name = "test", Value = "sss", ValueType = Platform.Core.Settings.SettingValueType.ShortText } }

            };
            var result = controller.Create(store) as OkNegotiatedContentResult<Store>;
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void UpdateStore()
        {
            var controller = GetStoreController();
            var result = controller.GetStoreById("testStore") as OkNegotiatedContentResult<Store>;
            var store = result.Content;

            store.Name = "diff name";
            store.DefaultCurrency = CurrencyCodes.UYU;
            store.Currencies.Add(CurrencyCodes.UYU);
            store.Languages.Remove(store.Languages.FirstOrDefault());
            store.Settings.Add(new Setting { Name = "setting2", Value = "1223", ValueType = Platform.Core.Settings.SettingValueType.Integer });
            store.FulfillmentCenter.CountryCode = "SSS";
            store.ReturnsFulfillmentCenter = store.FulfillmentCenter;

            controller.Update(store);

            result = controller.GetStoreById("testStore") as OkNegotiatedContentResult<Store>;

            store = result.Content;
        }

        [TestMethod]
        public void DeleteStore()
        {
            var controller = GetStoreController();
            controller.Delete(new[] { "testStore" });
            var result = controller.GetStoreById("testStore") as OkNegotiatedContentResult<Store>;

            Assert.IsNull(result);

        }

        private ICommerceService GetCommerceService()
        {
            return new CommerceServiceImpl(() => new CommerceRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor()));
        }

        private StoreModuleController GetStoreController()
        {
            Func<IPlatformRepository> platformRepositoryFactory = () => new PlatformRepository("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor());
            Func<IStoreRepository> repositoryFactory = () => new StoreRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor());

            var dynamicPropertyService = new DynamicPropertyService(platformRepositoryFactory);
            var storeService = new StoreServiceImpl(repositoryFactory, GetCommerceService(), null, dynamicPropertyService, null, null);

            var controller = new StoreModuleController(storeService, null, null);
            return controller;
        }
    }
}
