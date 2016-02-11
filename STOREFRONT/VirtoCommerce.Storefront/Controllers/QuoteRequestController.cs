using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Converters;
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
        private readonly IQuoteModuleApi _quoteApi;
        private readonly IQuoteRequestBuilder _quoteRequestBuilder;
        private readonly ICatalogSearchService _catalogSearchService;

        public QuoteRequestController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, IQuoteModuleApi quoteApi, IQuoteRequestBuilder quoteRequestBuilder,
            ICatalogSearchService catalogSearchService)
            : base(workContext, urlBuilder)
        {
            _quoteApi = quoteApi;
            _quoteRequestBuilder = quoteRequestBuilder;
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


        //private readonly IQuoteModuleApi _quoteApi;
        //private readonly IQuoteRequestBuilder _quoteRequestBuilder;
        //private readonly ICatalogSearchService _catalogSearchService;
        //private readonly ICartBuilder _cartBuilder;

        //public QuoteRequestController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, IQuoteRequestBuilder quoteRequestBuilder,
        //    ICatalogSearchService catalogSearchService, IQuoteModuleApi quoteApi, ICartBuilder cartBuilder)
        //    : base(workContext, urlBuilder)
        //{
        //    _quoteApi = quoteApi;
        //    _quoteRequestBuilder = quoteRequestBuilder;
        //    _catalogSearchService = catalogSearchService;
        //    _cartBuilder = cartBuilder;
        //}

        //// GET: /quoterequest
        //[HttpGet]
        //public ActionResult GetCustomerCurrentQuoteRequest()
        //{
        //    return View("quote-request", WorkContext);
        //}

        //// GET: /account/quoterequests
        //[HttpGet]
        //public async Task<ActionResult> GetCustomerQuoteRequests()
        //{
        //    var criteria = WorkContext.CurrentQuoteSearchCriteria;
        //    var quoteSearchCriteria = new VirtoCommerceDomainQuoteModelQuoteRequestSearchCriteria
        //    {
        //        Start = criteria.Start,
        //        Count = criteria.PageSize,
        //        StoreId = WorkContext.CurrentStore.Id,
        //        CustomerId = WorkContext.CurrentCustomer.Id,
        //    };

        //    var searchResult = await _quoteApi.QuoteModuleSearchAsync(quoteSearchCriteria);
        //    if (searchResult != null)
        //    {
        //        WorkContext.CurrentCustomer.QuoteRequests = new StorefrontPagedList<QuoteRequest>(
        //            searchResult.QuoteRequests.Select(x => x.ToWebModel(WorkContext.AllCurrencies, WorkContext.CurrentLanguage)),
        //            criteria.PageNumber, criteria.PageSize,
        //            searchResult.TotalCount.Value,
        //            page => WorkContext.RequestUrl.SetQueryParameter("page", page.ToString()).ToString());
        //    }

        //    return View("customers/quote-requests", WorkContext);
        //}

        //// GET: /quoterequest/{number}
        //[HttpGet]
        //public async Task<ActionResult> QuoteRequestByNumber(string number)
        //{
        //    if (string.IsNullOrEmpty(number))
        //    {
        //        throw new HttpException(404, string.Empty);
        //    }

        //    WorkContext.CurrentQuoteRequest = await GetCustomerQuoteRequestByIdAsync(number);

        //    if (WorkContext.CurrentQuoteRequest == null)
        //    {
        //        throw new HttpException(404, string.Empty);
        //    }

        //    return View("quote-request", WorkContext);
        //}

        //// GET: /currentquoterequest/json
        //[HttpGet]
        //public ActionResult CurrentQuoteRequestJson()
        //{
        //    EnsureThatQuoteRequestExists();

        //    return Json(WorkContext.CurrentQuoteRequest, JsonRequestBehavior.AllowGet);
        //}

        //// GET: /customerquoterequest/{number}/json
        //[HttpGet]
        //public async Task<ActionResult> CustomerQuoteRequestJson(string number)
        //{
        //    if (string.IsNullOrEmpty(number))
        //    {
        //        throw new HttpException(404, string.Empty);
        //    }

        //    var quoteRequest = await GetCustomerQuoteRequestByIdAsync(number);

        //    return Json(quoteRequest, JsonRequestBehavior.AllowGet);
        //}

        //// POST: /quoterequest/additem?productId=...&quantity=...
        //[HttpPost]
        //public async Task<ActionResult> AddItemJson(string productId, long quantity)
        //{
        //    EnsureThatQuoteRequestExists();

        //    using (var lockObject = await AsyncLock.GetLockByKey("quote-product:" + productId).LockAsync())
        //    {
        //        var products = await _catalogSearchService.GetProductsAsync(new string[] { productId }, ItemResponseGroup.ItemLarge);
        //        if (products != null && products.Any())
        //        {
        //            _quoteRequestBuilder.AddItem(products.First(), quantity);
        //            await _quoteRequestBuilder.SaveAsync();
        //        }
        //    }

        //    return Json(null, JsonRequestBehavior.AllowGet);
        //}

        //// POST: /quoterequest/removeitem?quoteItemId=...
        //[HttpPost]
        //public async Task<ActionResult> RemoveItemJson(string quoteItemId)
        //{
        //    EnsureThatQuoteRequestExists();

        //    using (var lockObject = await AsyncLock.GetLockByKey("quote-item:" + WorkContext.CurrentQuoteRequest.Id).LockAsync())
        //    {
        //        _quoteRequestBuilder.RemoveItem(quoteItemId);
        //        await _quoteRequestBuilder.SaveAsync();
        //    }

        //    return Json(null, JsonRequestBehavior.AllowGet);
        //}

        //// POST: /quoterequest/update?quoteRequest=...
        //[HttpPost]
        //public async Task<ActionResult> UpdateJson(QuoteRequestFormModel quoteRequest)
        //{
        //    EnsureThatQuoteRequestExists();

        //    using (var lockObject = await AsyncLock.GetLockByKey("quote:" + WorkContext.CurrentQuoteRequest.Id).LockAsync())
        //    {
        //        _quoteRequestBuilder.Update(quoteRequest);
        //        await _quoteRequestBuilder.SaveAsync();
        //    }

        //    return Json(null, JsonRequestBehavior.AllowGet);
        //}


        //// POST: /quoterequest/totals?quoteRequestId=...&quoteItemId=...&tierPrice=...
        //[HttpPost]
        //public async Task<ActionResult> GetTotalsJson(string quoteRequestId, string quoteItemId, TierPriceFormModel tierPrice)
        //{
        //    QuoteRequestTotals totals = null;

        //    var quoteRequest = await GetCustomerQuoteRequestByIdAsync(quoteRequestId);
        //    if (quoteRequest != null)
        //    {
        //        var quoteItem = quoteRequest.Items.FirstOrDefault(i => i.Id == quoteItemId);
        //        if (quoteItem != null)
        //        {
        //            quoteItem.SelectedTierPrice = new TierPrice
        //            {
        //                Price = new Money(tierPrice.Price, WorkContext.CurrentCurrency),
        //                Quantity = quoteItem.SelectedTierPrice.Quantity
        //            };
        //        }

        //        var quoteResult = await _quoteApi.QuoteModuleCalculateTotalsAsync(quoteRequest.ToServiceModel());
        //        totals = quoteResult.Totals.ToWebModel(quoteRequest.Currency);
        //    }

        //    return Json(totals, JsonRequestBehavior.AllowGet);
        //}

        //// GET: /quoterequest/quote-request/{number}/edit
        //[HttpGet]
        //public async Task<ActionResult> EditQuoteRequest(string number)
        //{
        //    if (string.IsNullOrEmpty(number))
        //    {
        //        throw new HttpException(404, string.Empty);
        //    }

        //    WorkContext.CurrentQuoteRequest = await GetCustomerQuoteRequestByIdAsync(number);
        //    if (WorkContext.CurrentQuoteRequest == null)
        //    {
        //        throw new HttpException(404, string.Empty);
        //    }
        //    return View("quote-request", WorkContext);
        //}

        //// GET: /quoterequest/quote-request/{number}/confirm
        //[HttpGet]
        //public async Task<ActionResult> ConfirmQuoteRequest(string number)
        //{
        //    if (string.IsNullOrEmpty(number))
        //    {
        //        throw new HttpException(404, string.Empty);
        //    }
        //    //Need lock to prevent concurrent access to same quote
        //    using (var lockObject = await AsyncLock.GetLockByKey("quote:" + number).LockAsync())
        //    {
        //        var quoteRequest = await GetCustomerQuoteRequestByIdAsync(number);

        //        if (quoteRequest == null)
        //        {
        //            throw new HttpException(404, string.Empty);
        //        }

        //        _cartBuilder.TakeCart(WorkContext.CurrentCart);
        //        await _cartBuilder.FillFromQuoteRequest(quoteRequest);
        //        await _cartBuilder.SaveAsync();
        //    }

        //    return StoreFrontRedirect("~/cart/checkout");
        //}

        //// GET: /quoterequest/quote-request/{number}/reject
        //[HttpGet]
        //public async Task<ActionResult> RejectQuoteRequest(string number)
        //{
        //    if (string.IsNullOrEmpty(number))
        //    {
        //        throw new HttpException(404, string.Empty);
        //    }
        //    //Need lock to prevent concurrent access to same quote
        //    using (var lockObject = await AsyncLock.GetLockByKey("quote:" + number).LockAsync())
        //    {
        //        var quoteRequest = await GetCustomerQuoteRequestByIdAsync(number);

        //        if (quoteRequest == null)
        //        {
        //            throw new HttpException(404, string.Empty);
        //        }

        //        _quoteRequestBuilder.TakeQuoteRequest(quoteRequest);
        //        _quoteRequestBuilder.Reject();
        //        await _quoteRequestBuilder.SaveAsync();
        //    }

        //    return StoreFrontRedirect("~/account/quote-requests");
        //}

        //private void EnsureThatQuoteRequestExists()
        //{
        //    if (WorkContext.CurrentQuoteRequest == null)
        //    {
        //        throw new StorefrontException("Quote request is not found");
        //    }

        //    _quoteRequestBuilder.TakeQuoteRequest(WorkContext.CurrentQuoteRequest);
        //}

        //private async Task<QuoteRequest> GetCustomerQuoteRequestByIdAsync(string id)
        //{
        //    var result = await _quoteApi.QuoteModuleGetByIdAsync(id);

        //    if (WorkContext.CurrentCustomer.Id != result.CustomerId)
        //    {
        //        throw new StorefrontException("Requested quote not belongs to current user");
        //    }

        //    return result != null ? result.ToWebModel(WorkContext.AllCurrencies, WorkContext.CurrentLanguage) : null;
        //}
    }
}