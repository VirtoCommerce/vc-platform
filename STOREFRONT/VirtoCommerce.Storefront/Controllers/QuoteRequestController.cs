using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart.Services;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Common.Exceptions;
using VirtoCommerce.Storefront.Model.Quote;
using VirtoCommerce.Storefront.Model.Quote.Events;
using VirtoCommerce.Storefront.Model.Quote.Services;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    public class QuoteRequestController : StorefrontControllerBase
    {
        private readonly IQuoteModuleApi _quoteApi;
        private readonly IQuoteRequestBuilder _quoteRequestBuilder;
        private readonly ICartBuilder _cartBuilder;
        private readonly ICatalogSearchService _catalogSearchService;

        public QuoteRequestController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, ICartBuilder cartBuilder,
            IQuoteModuleApi quoteApi, IQuoteRequestBuilder quoteRequestBuilder, ICatalogSearchService catalogSearchService)
            : base(workContext, urlBuilder)
        {
            _quoteApi = quoteApi;
            _quoteRequestBuilder = quoteRequestBuilder;
            _cartBuilder = cartBuilder;
            _catalogSearchService = catalogSearchService;
        }

        // GET: /quoterequest
        [HttpGet]
        public ActionResult ActualQuoteRequest()
        {
            EnsureThatQuoteRequestExists();

            return View("quote-request", WorkContext);
        }

        // GET: /quoterequest/{number}
        [HttpGet]
        public async Task<ActionResult> QuoteRequest(string number)
        {
            WorkContext.CurrentQuoteRequest = await GetQuoteRequestByNumberAsync(number);

            return View("quote-request", WorkContext);
        }

        // GET: /actualquoterequest/itemscount/json
        [HttpGet]
        public ActionResult ActualQuoteRequestItemsCountJson()
        {
            EnsureThatQuoteRequestExists();

            var quoteRequest = _quoteRequestBuilder.QuoteRequest;

            return Json(new { Id = quoteRequest.Id, ItemsCount = quoteRequest.ItemsCount }, JsonRequestBehavior.AllowGet);
        }

        // GET: /quoterequest/{number}/json
        [HttpGet]
        public async Task<ActionResult> QuoteRequestJson(string number)
        {
            var quoteRequest = await GetQuoteRequestByNumberAsync(number);
            if (quoteRequest != null)
            {
                quoteRequest.Customer = WorkContext.CurrentCustomer;
            }

            return Json(quoteRequest, JsonRequestBehavior.AllowGet);
        }

        // POST: /actualquoterequest/addproduct
        [HttpPost]
        public async Task<ActionResult> AddProductJson(string productId, int quantity)
        {
            EnsureThatQuoteRequestExists();

            using (var lockObject = await AsyncLock.GetLockByKey("quote-product:" + productId).LockAsync())
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

        // POST: /actualquoterequest/removeproduct
        [HttpPost]
        public async Task<ActionResult> RemoveProductJson(string quoteItemId)
        {
            EnsureThatQuoteRequestExists();

            using (var lockObject = await AsyncLock.GetLockByKey("quote-item:" + WorkContext.CurrentQuoteRequest.Id).LockAsync())
            {
                _quoteRequestBuilder.RemoveItem(quoteItemId);
                await _quoteRequestBuilder.SaveAsync();
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /quoterequest/submit
        [HttpPost]
        public async Task<ActionResult> SubmitJson(QuoteRequestFormModel quoteRequest)
        {
            EnsureThatQuoteRequestExists();

            using (var lockObject = await AsyncLock.GetLockByKey("quote:" + WorkContext.CurrentQuoteRequest.Id).LockAsync())
            {
                _quoteRequestBuilder.Update(quoteRequest).Submit();
                await _quoteRequestBuilder.SaveAsync();
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /quoterequest/reject
        [HttpPost]
        public async Task<ActionResult> RejectJson(string number)
        {
            var quoteRequest = await GetQuoteRequestByNumberAsync(number);
            if (quoteRequest != null)
            {
                _quoteRequestBuilder.TakeQuoteRequest(quoteRequest).Reject();
                await _quoteRequestBuilder.SaveAsync();
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /quoterequest/update
        [HttpPost]
        public async Task<ActionResult> UpdateJson(QuoteRequestFormModel quoteRequest)
        {
            EnsureThatQuoteRequestExists();

            using (var lockObject = await AsyncLock.GetLockByKey("quote:" + WorkContext.CurrentQuoteRequest.Id).LockAsync())
            {
                _quoteRequestBuilder.Update(quoteRequest);
                await _quoteRequestBuilder.SaveAsync();
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /quoterequest/totals
        [HttpPost]
        public async Task<ActionResult> TotalsJson(QuoteRequestFormModel quoteRequest)
        {
            QuoteRequestTotals totals = null;

            var foundedQuoteRequest = await GetQuoteRequestByNumberAsync(quoteRequest.Id);
            if (foundedQuoteRequest != null)
            {
                _quoteRequestBuilder.TakeQuoteRequest(foundedQuoteRequest).Update(quoteRequest);
                var quoteResult = await _quoteApi.QuoteModuleCalculateTotalsAsync(_quoteRequestBuilder.QuoteRequest.ToServiceModel());
                totals = quoteResult.Totals.ToWebModel(WorkContext.CurrentCurrency);
            }

            return Json(totals, JsonRequestBehavior.AllowGet);
        }

        // POST: /quoterequest/confirm
        [HttpPost]
        public async Task<ActionResult> ConfirmJson(QuoteRequestFormModel quoteRequest)
        {
            var foundedQuoteRequest = await GetQuoteRequestByNumberAsync(quoteRequest.Id);
            if (foundedQuoteRequest != null)
            {
                _quoteRequestBuilder.TakeQuoteRequest(foundedQuoteRequest).Update(quoteRequest).Confirm();
                await _quoteRequestBuilder.SaveAsync();

                _cartBuilder.TakeCart(WorkContext.CurrentCart);
                await _cartBuilder.FillFromQuoteRequest(foundedQuoteRequest);
                await _cartBuilder.SaveAsync();
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // GET: /account/quoterequests
        [HttpGet]
        public ActionResult QuoteRequests()
        {
            return View("customers/quote-requests", WorkContext);
        }

        private void EnsureThatQuoteRequestExists()
        {
            if (WorkContext.CurrentQuoteRequest == null)
            {
                throw new StorefrontException("Quote request is not found");
            }

            _quoteRequestBuilder.TakeQuoteRequest(WorkContext.CurrentQuoteRequest);
        }

        private async Task<QuoteRequest> GetQuoteRequestByNumberAsync(string number)
        {
            var quoteRequest = await _quoteApi.QuoteModuleGetByIdAsync(number);

            if (WorkContext.CurrentCustomer.Id != quoteRequest.CustomerId)
            {
                throw new StorefrontException("Requested quote not belongs to current user");
            }

            return quoteRequest != null ? quoteRequest.ToWebModel(WorkContext.AllCurrencies, WorkContext.CurrentLanguage) : null;
        }
    }
}