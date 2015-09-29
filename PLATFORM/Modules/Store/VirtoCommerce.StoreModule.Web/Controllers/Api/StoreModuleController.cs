using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Domain.Shipping.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Domain.Tax.Services;
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
        private readonly ITaxService _taxService;
		public StoreModuleController(IStoreService storeService, IShippingService shippingService, IPaymentMethodsService paymentService, ITaxService taxService)
		{
			_storeService = storeService;
			_shippingService = shippingService;
			_paymentService = paymentService;
            _taxService = taxService;
        }

		/// <summary>
		/// Get all stores
		/// </summary>
		[HttpGet]
		[ResponseType(typeof(webModel.Store[]))]
		[Route("")]
        [OverrideAuthorization]
		public IHttpActionResult GetStores()
		{
			var retVal = _storeService.GetStoreList().Select(x => x.ToWebModel()).ToArray();
			return Ok(retVal);
		}

		/// <summary>
		/// Get store by id
		/// </summary>
		/// <param name="id">Store id</param>
		/// <responce code="404">Store not found</responce>
		/// <responce code="200">Store returned successfully OK</responce>
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

		
		/// <summary>
		/// Create store
		/// </summary>
		/// <param name="store">Store</param>
		[HttpPost]
		[ResponseType(typeof(webModel.Store))]
		[Route("")]
        [CheckPermission(Permission = PredefinedPermissions.Create)]
		public IHttpActionResult Create(webModel.Store store)
		{
			var coreStore = store.ToCoreModel(_shippingService.GetAllShippingMethods(), _paymentService.GetAllPaymentMethods(), _taxService.GetAllTaxProviders());
			var retVal = _storeService.Create(coreStore);
			return Ok(retVal.ToWebModel());
		}

		/// <summary>
		/// Update store
		/// </summary>
		/// <param name="store">Store</param>
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("")]
        [CheckPermission(Permission = PredefinedPermissions.Update)]
		public IHttpActionResult Update(webModel.Store store)
		{
			var coreStore = store.ToCoreModel(_shippingService.GetAllShippingMethods(), _paymentService.GetAllPaymentMethods(), _taxService.GetAllTaxProviders());
			_storeService.Update(new coreModel.Store[] { coreStore });
			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Delete stores
		/// </summary>
		/// <param name="ids">Ids of store that needed to delete</param>
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("")]
        [CheckPermission(Permission = PredefinedPermissions.Delete)]
		public IHttpActionResult Delete([FromUri] string[] ids)
		{
			_storeService.Delete(ids);
			return StatusCode(HttpStatusCode.NoContent);
		}
	}
}
