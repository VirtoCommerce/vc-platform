using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using Store = VirtoCommerce.MerchandisingModule.Web.Model.Store;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
	[RoutePrefix("api/mp/stores")]
	public class MerchandisingModuleStoreController : ApiController
	{
		private readonly IStoreService _storeService;
		private readonly CacheManager _cacheManager;


		public MerchandisingModuleStoreController(IStoreService storeService, CacheManager cacheManager)
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
			var stores = _storeService.GetStoreList().Select(fullLoadedStore => fullLoadedStore.ToWebModel());
			return Ok(stores);
		}

	}
}