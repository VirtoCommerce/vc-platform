using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Common.Exceptions;
using VirtoCommerce.Storefront.Model.Quote;
using VirtoCommerce.Storefront.Model.Quote.Services;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    public class QuoteRequestController : StorefrontControllerBase
    {
        private readonly IQuoteRequestBuilder _quoteRequestBuilder;
        private readonly IQuoteService _quoteService;
        private readonly ICatalogSearchService _catalogSearchService;

        public QuoteRequestController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, IQuoteRequestBuilder quoteRequestBuilder,
            IQuoteService quoteService, ICatalogSearchService catalogSearchService)
            : base(workContext, urlBuilder)
        {
            _quoteRequestBuilder = quoteRequestBuilder;
            _quoteService = quoteService;
            _catalogSearchService = catalogSearchService;
        }

        // GET: /quoterequest
        [HttpGet]
        public ActionResult Index()
        {
            return View("quote-request", WorkContext);
        }

        // GET: /quoterequest/json
        [HttpGet]
        public ActionResult QuoteRequestJson()
        {
            EnsureThatQuoteRequestExists();

            return Json(_quoteRequestBuilder.QuoteRequest, JsonRequestBehavior.AllowGet);
        }

        // GET: /quoterequest/{number}/json
        [HttpGet]
        public async Task<ActionResult> GetByNumberJson(string number)
        {
            var quoteRequest = await _quoteService.GetQuoteRequestAsync(WorkContext.CurrentCustomer.Id, number);

            return Json(quoteRequest, JsonRequestBehavior.AllowGet);
        }

        // POST: /quoterequest/totals/json
        [HttpPost]
        public async Task<ActionResult> GetTotalsJson(string quoteRequestId, string quoteItemId, TierPriceFormModel tierPrice)
        {
            QuoteRequestTotals totals = null;

            var quoteRequest = await _quoteService.GetQuoteRequestAsync(WorkContext.CurrentCustomer.Id, quoteRequestId);
            if (quoteRequest != null)
            {
                var quoteItem = quoteRequest.Items.FirstOrDefault(i => i.Id == quoteItemId);
                if (quoteItem != null)
                {
                    quoteItem.SelectedTierPrice = new TierPrice
                    {
                        Price = new Money(tierPrice.Price, WorkContext.CurrentCurrency),
                        Quantity = quoteItem.SelectedTierPrice.Quantity
                    };
                }

                totals = await _quoteService.GetQuoteRequestTotalsAsync(quoteRequest);
            }

            return Json(totals, JsonRequestBehavior.AllowGet);
        }

        // POST: /quoterequest/update?quoteRequest=...
        [HttpPost]
        public async Task<ActionResult> UpdateJson(QuoteRequestFormModel quoteRequest)
        {
            EnsureThatQuoteRequestExists();

            using (var lockObject = await AsyncLock.GetLockByKey(WorkContext.CurrentQuoteRequest.Id).LockAsync())
            {
                _quoteRequestBuilder.Update(quoteRequest);
                await _quoteRequestBuilder.SaveAsync();
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /quoterequest/additem?productId=...&quantity=...
        [HttpPost]
        public async Task<ActionResult> AddItemJson(string productId, long quantity)
        {
            EnsureThatQuoteRequestExists();

            using (var lockObject = await AsyncLock.GetLockByKey(productId).LockAsync())
            {
                var products = await _catalogSearchService.GetProductsAsync(new string[] { productId }, ItemResponseGroup.ItemLarge);
                if (products != null && products.Any())
                {
                    _quoteRequestBuilder.AddItem(products.First(), quantity);
                    await _quoteRequestBuilder.SaveAsync();
                }
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /quoterequest/removeitem?quoteItemId=...
        [HttpPost]
        public async Task<ActionResult> RemoveItemJson(string quoteItemId)
        {
            EnsureThatQuoteRequestExists();

            using (var lockObject = await AsyncLock.GetLockByKey(WorkContext.CurrentQuoteRequest.Id).LockAsync())
            {
                _quoteRequestBuilder.RemoveItem(quoteItemId);
                await _quoteRequestBuilder.SaveAsync();
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        private void EnsureThatQuoteRequestExists()
        {
            if (WorkContext.CurrentQuoteRequest == null)
            {
                throw new StorefrontException("Quote request is not found");
            }

            _quoteRequestBuilder.TakeQuoteRequest(WorkContext.CurrentQuoteRequest);
        }
    }
}