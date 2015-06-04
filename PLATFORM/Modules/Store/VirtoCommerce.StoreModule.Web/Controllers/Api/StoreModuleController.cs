using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Domain.Shipping.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.StoreModule.Web.Converters;
using coreModel = VirtoCommerce.Domain.Store.Model;
using webModel = VirtoCommerce.StoreModule.Web.Model;

namespace VirtoCommerce.StoreModule.Web.Controllers.Api
{
	[RoutePrefix("api/stores")]
    [CheckPermission(Permission = PredefinedPermissions.Query)]
	public class StoreModuleController : ApiController
	{
		private readonly IStoreService _storeService;
		private readonly IShippingService _shippingService;
		private readonly IPaymentMethodsService _paymentService;
		public StoreModuleController(IStoreService storeService, IShippingService shippingService, IPaymentMethodsService paymentService)
		{
			_storeService = storeService;
			_shippingService = shippingService;
			_paymentService = paymentService;
		}

		// GET: api/stores
		[HttpGet]
		[ResponseType(typeof(webModel.Store[]))]
		[Route("")]
		public IHttpActionResult GetStores()
		{
			var retVal = _storeService.GetStoreList().Select(x => x.ToWebModel()).ToArray();
			return Ok(retVal);
		}

		// GET: api/stores/{id}
		[HttpGet]
		[ResponseType(typeof(webModel.Store))]
		[Route("{id}")]
		public IHttpActionResult GetStoreById(string id)
		{
			var retVal = _storeService.GetById(id);
			if(retVal == null)
			{
				return NotFound();
			}
			return Ok(retVal.ToWebModel());
		}

		
		// POST: api/stores
		[HttpPost]
		[ResponseType(typeof(webModel.Store))]
		[Route("")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult Create(webModel.Store store)
		{
			var coreStore = store.ToCoreModel(_shippingService.GetAllShippingMethods(), _paymentService.GetAllPaymentMethods());
			var retVal = _storeService.Create(coreStore);
			return Ok(retVal.ToWebModel());
		}

		// PUT: api/stores
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult Update(webModel.Store store)
		{
			var coreStore = store.ToCoreModel(_shippingService.GetAllShippingMethods(), _paymentService.GetAllPaymentMethods());
			_storeService.Update(new coreModel.Store[] { coreStore });
			return StatusCode(HttpStatusCode.NoContent);
		}

		// DELETE: api/stores?ids=21
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult Delete([FromUri] string[] ids)
		{
			_storeService.Delete(ids);
			return StatusCode(HttpStatusCode.NoContent);
		}
	}
}
