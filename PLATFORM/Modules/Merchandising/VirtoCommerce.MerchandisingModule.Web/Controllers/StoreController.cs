using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.MerchandisingModule.Web.Model;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
	[RoutePrefix("api/mp/stores")]
	public class StoreController  : ApiController
	{
		private readonly IStoreService _storeService;
		private readonly CacheManager _cacheManager;


		public StoreController(IStoreService storeService, CacheManager cacheManager)
		{
			_storeService = storeService;
			_cacheManager = cacheManager;
		}


		[HttpGet]
		[ResponseType(typeof(Store[]))]
		[ClientCache(Duration = 60)]
		[Route("")]
		public IHttpActionResult GetStores()
		{
			var cacheKey = CacheKey.Create("PriceController.GetStores");
			var stores = _cacheManager.Get(cacheKey, () => GetFullLoadedStoreList());


			return Ok(stores);
		}

		private Store[] GetFullLoadedStoreList()
		{
			var retVal = new List<Store>();
			var stores = _storeService.GetStoreList();
			foreach(var store in stores)
			{
				var fullLoadedStore = _storeService.GetById(store.Id);
				retVal.Add(fullLoadedStore.ToWebModel());
			}
			return retVal.ToArray();
		}


	}
}