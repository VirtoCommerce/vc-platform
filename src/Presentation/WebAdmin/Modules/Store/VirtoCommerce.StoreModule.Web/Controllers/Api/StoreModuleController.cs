using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.StoreModule.Web.Converters;
using coreModel = VirtoCommerce.Domain.Store.Model;
using webModel = VirtoCommerce.StoreModule.Web.Model;

namespace VirtoCommerce.StoreModule.Web.Controllers.Api
{
	[RoutePrefix("api/stores")]
	public class StoreModuleController : ApiController
	{
		private readonly IStoreService _storeService;
		public StoreModuleController(IStoreService storeService)
		{
			_storeService = storeService;
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
		public IHttpActionResult Create(webModel.Store store)
		{
			var coreStore = store.ToCoreModel();
			var retVal = _storeService.Create(coreStore);
			return Ok(retVal.ToWebModel());
		}

		// PUT: api/stores
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("")]
		public IHttpActionResult Update(webModel.Store store)
		{
			var coreStore = store.ToCoreModel();
			_storeService.Update(new coreModel.Store[] { coreStore });
			return StatusCode(HttpStatusCode.NoContent);
		}

		// DELETE: api/stores?ids=21
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("")]
		public IHttpActionResult Delete([FromUri] string[] ids)
		{
			_storeService.Delete(ids);
			return StatusCode(HttpStatusCode.NoContent);
		}
	}
}
