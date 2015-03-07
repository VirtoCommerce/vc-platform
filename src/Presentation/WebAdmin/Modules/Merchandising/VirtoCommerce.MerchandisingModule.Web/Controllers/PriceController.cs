using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Frameworks.Tagging;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.Framework.Web.Common;
using VirtoCommerce.Framework.Web.Settings;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp")]
    public class PriceController : BaseController
    {
        #region Fields

        private readonly IPriceListAssignmentEvaluationContext _priceListEvalContext;
        private readonly Func<IPriceListAssignmentEvaluator> _priceListEvaluator;
        private readonly Func<IPricelistRepository> _priceListRepository;

        #endregion

        #region Constructors and Destructors

        public PriceController(
            Func<IStoreRepository> storeRepository,
            Func<IPricelistRepository> priceListRepository,
            Func<IPriceListAssignmentEvaluator> priceListEvaluator,
            IPriceListAssignmentEvaluationContext priceListEvalContext,
            ISettingsManager settingsManager,
            ICacheRepository cache)
            : base(storeRepository, settingsManager, cache)
        {
            this._priceListEvalContext = priceListEvalContext;
            this._priceListRepository = priceListRepository;
            this._priceListEvaluator = priceListEvaluator;
        }

        #endregion

        #region Public Methods and Operators

        [HttpGet]
        [ClientCache(Duration = 60)]
        [ResponseType(typeof(string[]))]
        [Route("pricelists")]
        public IHttpActionResult GetPriceListStack(
            string catalog,
            string currency,
            [FromUri] string[] tags
            )
        {
            var pricelists = this.GetPriceListStackInternal(catalog, currency, tags);
            return this.Ok(pricelists);
        }

        [HttpGet]
        [ResponseType(typeof(Price[]))]
        [ArrayInput(ParameterName = "priceLists")]
        [ArrayInput(ParameterName = "products")]
        [Route("prices")]
        public IHttpActionResult GetProductPrices(
            [FromUri] string[] priceLists,
            [FromUri] string[] products
            )
        {
            using (var plRepository = this._priceListRepository())
            {
                var prices = plRepository.FindLowestPrices(priceLists, products, 0, returnAll: true);
                if (prices != null && prices.Any())
                {
                    return this.Ok(prices.Select(p => p.ToWebModel()));
                }
            }

            return this.StatusCode(HttpStatusCode.NoContent);
        }

        #endregion

        #region Methods

        private string[] GetPriceListStackInternal(
            string catalog,
            string currency,
            IEnumerable<string> tags
            )
        {
            //            var catalogId = GetCatalogId(store);
            var tagSet = new TagSet();

            if (tags != null)
            {
                foreach (var tagArray in tags.Select(tag => tag.Split(new[] { ':' })))
                {
                    tagSet.Add(tagArray[0], tagArray[1]);
                }
            }

            var evaluateContext = this._priceListEvalContext;

            evaluateContext.Currency = currency;
            evaluateContext.CatalogId = catalog;
            evaluateContext.ContextObject = tags;
            evaluateContext.CurrentDate = DateTime.Now;

            var evaluator = this._priceListEvaluator();
            var lists = evaluator.Evaluate(evaluateContext);

            return lists == null ? null : lists.Select(y => y.PricelistId).ToArray();
        }

        #endregion
    }
}
