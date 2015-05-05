using System;
using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
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
				Currencies = new CurrencyCodes[] { CurrencyCodes.USD, CurrencyCodes.RUB },
				DefaultCurrency = CurrencyCodes.USD,
				Languages = new string[] { "ru-ru", "en-us" },
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
				PaymentGateways = new string[] { "PayPal", "Clarna" },
				StoreState = Domain.Store.Model.StoreState.Open,
				Settings = new StoreSetting[] { new StoreSetting { Name = "test", Value = "sss", ValueType = Domain.Store.Model.SettingValueType.ShortText, Locale = "en-us" } }

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
			store.Settings.Add(new StoreSetting { Name = "setting2", Value = "1223", ValueType = Domain.Store.Model.SettingValueType.Integer });
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
			controller.Delete(new string[] { "testStore" });
			var result = controller.GetStoreById("testStore") as OkNegotiatedContentResult<Store>;

			Assert.IsNull(result);

		}
		private static StoreModuleController GetStoreController()
		{
			Func<IStoreRepository> repositoryFactory = () =>
			{
				return new StoreRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor());
			};
			
			var storeService = new StoreServiceImpl(repositoryFactory, null);
			var controller = new StoreModuleController(storeService);
			return controller;
		}
	}
}
