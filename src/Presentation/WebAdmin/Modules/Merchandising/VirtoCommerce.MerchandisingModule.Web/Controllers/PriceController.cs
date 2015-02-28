using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Frameworks.Tagging;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.Framework.Web.Common;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp")]
    public class PriceController : BaseController
    {
        private readonly IPriceListAssignmentEvaluationContext _priceListEvalContext;
        private readonly Func<IPriceListAssignmentEvaluator> _priceListEvaluator;
        private readonly Func<IPricelistRepository> _priceListRepository;

        public PriceController(Func<IStoreRepository> storeRepository, Func<IPricelistRepository> priceListRepository, Func<IPriceListAssignmentEvaluator> priceListEvaluator, IPriceListAssignmentEvaluationContext priceListEvalContext)
            : base(storeRepository)
        {
            this._priceListEvalContext = priceListEvalContext;
            this._priceListRepository = priceListRepository;
            this._priceListEvaluator = priceListEvaluator;
        }

        [HttpGet]
        [ResponseType(typeof(string[]))]
        [Route("pricelists")]
        public IHttpActionResult GetPriceListStack(
            string catalog,
            string currency,
            [FromUri] string[] tags
            )
        {
            var pricelists = this.GetPriceListStackInternal(catalog, currency, tags);
            return Ok(pricelists);
        }

        [HttpGet]
        [ResponseType(typeof(Price[]))]
        [ArrayInput("priceLists", Separator = ",")]
        [ArrayInput("itemIds", Separator = ",")]
        [Route("prices")]
        public IHttpActionResult GetProductPrices(
            [FromUri] string[] priceLists,
            [FromUri] string[] itemIds
            )
        {
            using (var plRepository = _priceListRepository())
            {
                var prices = plRepository.FindLowestPrices(priceLists, itemIds, 0, returnAll: true);
                if (prices != null && prices.Any())
                {
                    return this.Ok(prices.Select(p => p.ToWebModel()));
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

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

            var evaluateContext = _priceListEvalContext;

            evaluateContext.Currency = currency;
            evaluateContext.CatalogId = catalog;
            evaluateContext.ContextObject = tags;
            evaluateContext.CurrentDate = DateTime.Now;

            var evaluator = _priceListEvaluator();
            var lists = evaluator.Evaluate(evaluateContext);

            return lists == null ? null : lists.Select(y => y.PricelistId).ToArray();
        }
    }
}
