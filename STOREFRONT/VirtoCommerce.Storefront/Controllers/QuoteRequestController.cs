using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.LiquidThemeEngine.Extensions;
using VirtoCommerce.Storefront.Converters;
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

            return Json(WorkContext.CurrentQuoteRequest, JsonRequestBehavior.AllowGet);
        }

        // GET: /quoterequest/quote-requests
        [HttpGet]
        public async Task<ActionResult> QuoteRequests()
        {
            var criteria = base.WorkContext.CurrentQuoteSearchCriteria;
            var quoteSearchCriteria = new VirtoCommerceDomainQuoteModelQuoteRequestSearchCriteria
            {
                Start = criteria.Start,
                Count = criteria.PageSize,
                StoreId = base.WorkContext.CurrentStore.Id,
                CustomerId = base.WorkContext.CurrentCustomer.Id
            };
            var searchResult = await _quoteApi.QuoteModuleSearchAsync(quoteSearchCriteria);
            WorkContext.CurrentCustomer.QuoteRequests = new StorefrontPagedList<QuoteRequest>(searchResult.QuoteRequests.Select(x => x.ToWebModel(base.WorkContext.AllCurrencies, base.WorkContext.CurrentLanguage)), criteria.PageNumber, criteria.PageSize,
                                                                                              searchResult.TotalCount.Value, page => WorkContext.RequestUrl.SetQueryParameter("page", page.ToString()).ToString());

            return View("customers/quote-requests", WorkContext);
        }

        // GET: /quoterequest/quote-request/{number}
        [HttpGet]
        public async Task<ActionResult> GetQuoteRequestByNumber(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                throw new HttpException(404, string.Empty);
            }
            WorkContext.CurrentQuoteRequest = await GetCustomerQuoteRequestByIdAsync(number);

            if (WorkContext.CurrentQuoteRequest == null)
            {
                throw new HttpException(404, string.Empty);
            }
            return View("customers/quote-request", WorkContext);
        }

        // GET: /quoterequest/quote-request/{number}/edit
        [HttpGet]
        public async Task<ActionResult> EditQuoteRequest(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                throw new HttpException(404, string.Empty);
            }

            WorkContext.CurrentQuoteRequest = await GetCustomerQuoteRequestByIdAsync(number);
            if (WorkContext.CurrentQuoteRequest == null)
            {
                throw new HttpException(404, string.Empty);
            }
            return View("quote-request", WorkContext);
        }

        // GET: /quoterequest/quote-request/{number}/confirm
        [HttpGet]
        public async Task<ActionResult> ConfirmQuoteRequest(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                throw new HttpException(404, string.Empty);
            }
            //Need lock to prevent concurrent access to same quote
            using (var lockObject = await AsyncLock.GetLockByKey("quote:" + number).LockAsync())
            {
                var quoteRequest = await GetCustomerQuoteRequestByIdAsync(number);

                if (quoteRequest == null)
                {
                    throw new HttpException(404, string.Empty);
                }

                _cartBuilder.TakeCart(WorkContext.CurrentCart);
                await _cartBuilder.FillFromQuoteRequest(quoteRequest);
                await _cartBuilder.SaveAsync();
            }

            return StoreFrontRedirect("~/cart/checkout");
        }

        // GET: /quoterequest/quote-request/{number}/reject
        [HttpGet]
        public async Task<ActionResult> RejectQuoteRequest(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                throw new HttpException(404, string.Empty);
            }
            //Need lock to prevent concurrent access to same quote
            using (var lockObject = await AsyncLock.GetLockByKey("quote:" + number).LockAsync())
            {
                var quoteRequest = await GetCustomerQuoteRequestByIdAsync(number);

                if (quoteRequest == null)
                {
                    throw new HttpException(404, string.Empty);
                }

                _quoteRequestBuilder.TakeQuoteRequest(quoteRequest);
                _quoteRequestBuilder.Reject();
                await _quoteRequestBuilder.SaveAsync();
            }

            return StoreFrontRedirect("~/account/quote-requests");
        }


        // POST: /quoterequest/update?quoteRequest=...
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

        // POST: /quoterequest/additem?productId=...&quantity=...
        [HttpPost]
        public async Task<ActionResult> AddItemJson(string productId, long quantity)
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

        // POST: /quoterequest/removeitem?quoteItemId=...
        [HttpPost]
        public async Task<ActionResult> RemoveItemJson(string quoteItemId)
        {
            EnsureThatQuoteRequestExists();

            using (var lockObject = await AsyncLock.GetLockByKey("quote-item:" + WorkContext.CurrentQuoteRequest.Id).LockAsync())
            {
                _quoteRequestBuilder.RemoveItem(quoteItemId);
                await _quoteRequestBuilder.SaveAsync();
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }


        private async Task<QuoteRequest> GetCustomerQuoteRequestByIdAsync(string id)
        {
            var result = await _quoteApi.QuoteModuleGetByIdAsync(id);
            if(base.WorkContext.CurrentCustomer.Id != result.CustomerId)
            {
                throw new StorefrontException("Requested quote not belongs to current user");
            }
            return result != null ? result.ToWebModel(base.WorkContext.AllCurrencies, base.WorkContext.CurrentLanguage) : null;
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