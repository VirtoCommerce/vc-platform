using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart.Services;
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
        private readonly IQuoteModuleApi _quoteApi;
        private readonly IQuoteRequestBuilder _quoteRequestBuilder;
        private readonly ICatalogSearchService _catalogSearchService;
        private readonly ICartBuilder _cartBuilder;

        public QuoteRequestController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, IQuoteRequestBuilder quoteRequestBuilder,
            ICatalogSearchService catalogSearchService, IQuoteModuleApi quoteApi, ICartBuilder cartBuilder)
            : base(workContext, urlBuilder)
        {
            _quoteApi = quoteApi;
            _quoteRequestBuilder = quoteRequestBuilder;
            _catalogSearchService = catalogSearchService;
            _cartBuilder = cartBuilder;
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

        // GET: /quoterequest/quote-requests
        [HttpGet]
        public async Task<ActionResult> QuoteRequests()
        {
            var quoteSearchCriteria = new VirtoCommerceDomainQuoteModelQuoteRequestSearchCriteria
            {
                Start = base.WorkContext.CurrentQuoteSearchCriteria.PageNumber,
                Count = base.WorkContext.CurrentQuoteSearchCriteria.PageSize,
                StoreId = base.WorkContext.CurrentStore.Id,
                CustomerId = base.WorkContext.CurrentCustomer.Id
            };
            var searchResult = await _quoteApi.QuoteModuleSearchAsync(quoteSearchCriteria);
            WorkContext.CurrentCustomer.QuoteRequests = new StorefrontPagedList<QuoteRequest>(searchResult.QuoteRequests.Select(x => x.ToWebModel()), quoteSearchCriteria.Start.Value, quoteSearchCriteria.Count.Value,
                                                                                              searchResult.TotalCount.Value, page => workContext.RequestUrl.SetQueryParameter("page", page.ToString()).ToString());

            return View("customers/quote-requests", WorkContext);
        }

        // GET: /quoterequest/quote-request/{number}
        [HttpGet]
        public async Task<ActionResult> QuoteRequest(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return HttpNotFound();
            }
            WorkContext.QuoteRequest = await GetCustomerQuoteRequestByNumberAsync(number);

            if (WorkContext.QuoteRequest == null)
            {
                return HttpNotFound();
            }
            return View("customers/quote-request", WorkContext);
        }

        // GET: /quoterequest/quote-request/{number}/edit
        [HttpGet]
        public async Task<ActionResult> EditQuoteRequest(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return HttpNotFound();
            }

            WorkContext.QuoteRequest = await GetCustomerQuoteRequestByNumberAsync(number);
            if (WorkContext.QuoteRequest == null)
            {
                return HttpNotFound();
            }
            return View("quote-request", WorkContext);
        }

        // GET: /quoterequest/quote-request/{number}/confirm
        [HttpGet]
        public async Task<ActionResult> ConfirmQuoteRequest(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return HttpNotFound();
            }

            var quoteRequest = await GetCustomerQuoteRequestByNumberAsync(number);

            if (quoteRequest == null)
            {
                return HttpNotFound();
            }

            _cartBuilder.TakeCart(WorkContext.CurrentCart);
            await _cartBuilder.FillFromQuoteRequest(quoteRequest);
            await _cartBuilder.SaveAsync();

            return StoreFrontRedirect("~/cart/checkout");
        }

        // GET: /quoterequest/quote-request/{number}/reject
        [HttpGet]
        public async Task<ActionResult> RejectQuoteRequest(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return HttpNotFound();
            }

            var quoteRequest = await GetCustomerQuoteRequestByNumberAsync(number);

            if (quoteRequest == null)
            {
                return HttpNotFound();
            }

            _quoteRequestBuilder.TakeQuoteRequest(quoteRequest);
            _quoteRequestBuilder.Reject();
            await _quoteRequestBuilder.SaveAsync();

            return StoreFrontRedirect("~/account/quote-requests");
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


        private async Task<QuoteRequest> GetCustomerQuoteRequestByNumberAsync(string number)
        {
            var quoteSearchCriteria = new VirtoCommerceDomainQuoteModelQuoteRequestSearchCriteria
            {
                Keyword = number,
                StoreId = base.WorkContext.CurrentStore.Id,
                CustomerId = base.WorkContext.CurrentCustomer.Id
            };
            var searchResult = await _quoteApi.QuoteModuleSearchAsync(quoteSearchCriteria);
            return searchResult.QuoteRequests.Select(x => x.ToWebModel()).FirstOrDefault();
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