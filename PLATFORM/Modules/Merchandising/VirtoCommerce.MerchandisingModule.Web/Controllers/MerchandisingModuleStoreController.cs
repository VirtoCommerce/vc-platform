using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.Platform.Core.Common;
using Store = VirtoCommerce.MerchandisingModule.Web.Model.Store;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
	[RoutePrefix("api/mp/stores")]
	public class MerchandisingModuleStoreController  : ApiController
	{
		private readonly IStoreService _storeService;

		public MerchandisingModuleStoreController(IStoreService storeService)
		{
			_storeService = storeService;
		}

		[HttpGet]
		[ResponseType(typeof(Store[]))]
		[ClientCache(Duration = 60)]
		[Route("")]
		public IHttpActionResult GetStores()
		{
            var dbStores = _storeService.GetStoreList();
            var stores = dbStores.Select(s => s.ToWebModel());
			return Ok(stores);
		}
	}
}