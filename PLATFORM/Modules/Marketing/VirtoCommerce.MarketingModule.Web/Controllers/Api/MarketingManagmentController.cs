using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using VirtoCommerce.CustomerModule.Web.Binders;
using webModel = VirtoCommerce.MarketingModule.Web.Model;
namespace VirtoCommerce.MarketingModule.Web.Controllers.Api
{
	[RoutePrefix("api/marketing")]
    public class MarketingManagmentController : ApiController
    {
		public MarketingManagmentController()
		{
		}

		// GET: api/marketing/promotions?q=ddd&start=0&count=20
		[HttpGet]
		[ResponseType(typeof(webModel.SearchResult))]
		[Route("promotions")]
		public IHttpActionResult Search([ModelBinder(typeof(SearchCriteriaBinder))] webModel.SearchCriteria criteria)
		{
			return Ok(new webModel.SearchResult());
		}

		// GET: api/marketing/promotions/{id}
		[HttpGet]
		[ResponseType(typeof(webModel.Promotion))]
		[Route("promotions/{id}")]
		public IHttpActionResult GetPromotionById(string id)
		{
			return Ok(new webModel.Promotion());
		}

		// PUT: api/marketing/promotions
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("promotions")]
		public IHttpActionResult UpdatePromotions(webModel.Promotion promotion)
		{
			return StatusCode(HttpStatusCode.NoContent);
		}

		// DELETE: api/marketing/promotions?ids=21
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("promotions")]
		public IHttpActionResult DeletePromotions([FromUri] string[] ids)
		{
			return StatusCode(HttpStatusCode.NoContent);
		}
    }
}
