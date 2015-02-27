using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Frameworks.Tagging;
using VirtoCommerce.Foundation.Stores.Repositories;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp/stores/{store}/pricelists/{currency}")]
    public class PriceController : BaseController
    {
        private readonly IPriceListAssignmentEvaluationContext _priceListEvalContext;
        private readonly Func<IPriceListAssignmentEvaluator> _priceListEvaluator;

        public PriceController(Func<IStoreRepository> storeRepository, Func<IPriceListAssignmentEvaluator> priceListEvaluator, IPriceListAssignmentEvaluationContext priceListEvalContext)
            : base(storeRepository)
        {
            this._priceListEvalContext = priceListEvalContext;
            this._priceListEvaluator = priceListEvaluator;
        }

        [HttpGet]
        [ResponseType(typeof(string[]))]
        [Route("")]
        public IHttpActionResult GetPriceListStack(
            string storeId,
            string currency,
            [FromUri] string[] tags
            )
        {
            var catalogId = GetCatalogId(storeId);
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
            evaluateContext.CatalogId = catalogId;
            evaluateContext.ContextObject = tags;
            evaluateContext.CurrentDate = DateTime.Now;

            var evaluator = _priceListEvaluator();
            var lists = evaluator.Evaluate(evaluateContext);    

            return Ok(lists == null ? null : lists.Select(y => y.PricelistId).ToArray());
        }
    }
}
