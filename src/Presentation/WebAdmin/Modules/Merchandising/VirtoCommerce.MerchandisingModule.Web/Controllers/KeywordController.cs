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
        #region Fields

        private readonly Func<IFoundationAppConfigRepository> _appConfigRepFactory;

        #endregion

        #region Constructors and Destructors

        public KeywordController(Func<IFoundationAppConfigRepository> appConfigRepFactory)
        {
            this._appConfigRepFactory = appConfigRepFactory;
        }

        #endregion

        #region Public Methods and Operators

        [HttpGet]
        [ResponseType(typeof(SeoKeyword[]))]
        [Route("")]
        public IHttpActionResult GetAlKeywords()
        {
            using (var repository = this._appConfigRepFactory())
            {
                var keywords = repository.GetAllSeoInformation(null).ToArray();
                var retVal = keywords.Select(x => x.ToWebModel());
                return this.Ok(retVal);
            }
        }

        #endregion
    }
}
