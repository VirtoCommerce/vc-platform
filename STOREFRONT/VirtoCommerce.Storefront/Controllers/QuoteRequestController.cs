using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Quote.Services;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    public class QuoteRequestController : StorefrontControllerBase
    {
        private readonly IQuoteRequestBuilder _quoteRequestBuilder;
        private readonly ICatalogSearchService _catalogSearchService;

        private Store _store;
        private CustomerInfo _customer;
        private Language _language;
        private Currency _currency;

        public QuoteRequestController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, IQuoteRequestBuilder quoteRequestBuilder,
            ICatalogSearchService catalogSearchService)
            : base(workContext, urlBuilder)
        {
            _quoteRequestBuilder = quoteRequestBuilder;
            _catalogSearchService = catalogSearchService;

            _store = workContext.CurrentStore;
            _customer = workContext.CurrentCustomer;
            _language = workContext.CurrentLanguage;
            _currency = workContext.CurrentCurrency;
        }

        // GET: /quoterequest
        [HttpGet]
        public ActionResult Index()
        {
            return View("quote-request", WorkContext);
        }

        // GET: /quoterequest/json
        [HttpGet]
        public async Task<ActionResult> QuoteRequestJson()
        {
            await _quoteRequestBuilder.GetOrCreateNewTransientQuoteRequestAsync(_store, _customer, _language, _currency);

            return Json(_quoteRequestBuilder.QuoteRequest, JsonRequestBehavior.AllowGet);
        }

        // POST: /quoterequest/additem?productId=...&quantity=...
        [HttpPost]
        public async Task<ActionResult> AddItemJson(string productId, long quantity)
        {
            var products = await _catalogSearchService.GetProductsAsync(new string[] { productId }, ItemResponseGroup.ItemLarge);
            if (products != null && products.Any())
            {
                await _quoteRequestBuilder.GetOrCreateNewTransientQuoteRequestAsync(_store, _customer, _language, _currency);

                _quoteRequestBuilder.AddItem(products.First(), quantity);
                await _quoteRequestBuilder.SaveAsync();
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // POST: /quoterequest/removeitem?quoteItemId=...
        [HttpPost]
        public async Task<ActionResult> RemoveItemJson(string quoteItemId)
        {
            await _quoteRequestBuilder.GetOrCreateNewTransientQuoteRequestAsync(_store, _customer, _language, _currency);

            _quoteRequestBuilder.RemoveItem(quoteItemId);
            await _quoteRequestBuilder.SaveAsync();

            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}