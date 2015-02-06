using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp/{language}/keywords")]
    public class KeywordController : ApiController
    {
        private readonly Func<IFoundationAppConfigRepository> _appConfigRepFactory;

        public KeywordController(Func<IFoundationAppConfigRepository> appConfigRepFactory)
        {
            _appConfigRepFactory = appConfigRepFactory;
        }

        [HttpGet]
        [ResponseType(typeof(SeoKeyword[]))]
        [Route("")]
        public IHttpActionResult GetAlKeywords()
        {
            using (var repository = _appConfigRepFactory())
            {
                var keywords = repository.GetAllSeoInformation(null).ToArray();
                var retVal = keywords.Select(x => x.ToWebModel());
                return Ok(retVal);
            }
        }
    }
}
