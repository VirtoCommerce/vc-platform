using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp/{language}/products")]
	public class MerchandisingModuleReviewController : ApiController
    {
        
        [HttpGet]
        [ResponseType(typeof(webModel.Review))]
        [ClientCache(Duration = 30)]
        [Route("{productId}/reviews")]
		public IHttpActionResult GetProductReviews(string productId, string language)
        {
			var retVal = new webModel.Review();
			return Ok(retVal);
        }

  
    }
}
