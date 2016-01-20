using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.LiquidThemeEngine.Converters;
using VirtoCommerce.LiquidThemeEngine.Filters;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart.Services;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Common;

namespace VirtoCommerce.Storefront.Controllers
{
    public class ShopifyCompatibilityController : StorefrontControllerBase
    {
        private readonly WorkContext _workContext;
        private readonly IStorefrontUrlBuilder _urlBuilder;
        private readonly ICartBuilder _cartBuilder;
        private readonly ICatalogSearchService _catalogService;

        public ShopifyCompatibilityController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, ICartBuilder cartBuilder, ICatalogSearchService catalogService)
            : base(workContext, urlBuilder)
        {
            _workContext = workContext;
            _urlBuilder = urlBuilder;
            _cartBuilder = cartBuilder;
            _catalogService = catalogService;
        }

        // GET: /cart/change?line=...&quantity=...
        [HttpGet]
        public async Task<ActionResult> Change(int line, int quantity)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.ChangeItemQuantityAsync(line - 1, quantity);
            await _cartBuilder.SaveAsync();

            return StoreFrontRedirect("~/cart");
        }

        // POST: /cart/add
        [HttpPost]
        public async Task<ActionResult> Add(string id, int quantity)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var product = (await _catalogService.GetProductsAsync(new string[] { id }, Model.Catalog.ItemResponseGroup.ItemLarge)).FirstOrDefault();
            if (product != null)
            {
                await _cartBuilder.AddItemAsync(product, quantity);
                await _cartBuilder.SaveAsync();
            }

            return StoreFrontRedirect("~/cart");
        }

        // POST: /cart?updates=...&update=...
        [HttpPost]
        public async Task<ActionResult> Cart(int[] updates, string checkout)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.ChangeItemsQuantitiesAsync(updates);
            await _cartBuilder.SaveAsync();

            string virtualRedirectUrl = "~/cart";

            if (Request.Form.Get("checkout") != null)
            {
                if (_cartBuilder.Cart.HasPhysicalProducts)
                {
                    virtualRedirectUrl = "~/cart/checkout/#/customer-information";
                }
                else
                {
                    virtualRedirectUrl = "~/cart/checkout/#/payment-method";
                }
            }

            return StoreFrontRedirect(virtualRedirectUrl);
        }

        // GET: /cart/clear
        [HttpGet]
        public async Task<ActionResult> Clear()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.ClearAsync();
            await _cartBuilder.SaveAsync();

            return StoreFrontRedirect("~/cart");
        }

        // GET: /cart.js
        [HttpGet]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> CartJs()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            return LiquidJson(_cartBuilder.Cart.ToShopifyModel(WorkContext));
        }

        // POST: /cart/add.js
        [HttpPost]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> AddJs(string id, int quantity = 1)
        {
            LineItem lineItem = null;

            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            var product = (await _catalogService.GetProductsAsync(new[] { id }, Model.Catalog.ItemResponseGroup.ItemLarge)).FirstOrDefault();
            if (product != null)
            {
                await _cartBuilder.AddItemAsync(product, quantity);
                await _cartBuilder.SaveAsync();

                var addedItem = _cartBuilder.Cart.Items.FirstOrDefault(i => i.ProductId == id);
                if (addedItem != null)
                {
                    lineItem = addedItem.ToShopifyModel(WorkContext);
                }
            }

            return LiquidJson(lineItem);
        }

        // POST: /cart/change.js
        [HttpPost]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> ChangeJs(string id, int quantity)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.ChangeItemQuantityAsync(id, quantity);
            await _cartBuilder.SaveAsync();

            return LiquidJson(_cartBuilder.Cart.ToShopifyModel(WorkContext));
        }

        // POST: /cart/update.js
        [HttpPost]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> UpdateJs(int[] updates)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.ChangeItemsQuantitiesAsync(updates);
            await _cartBuilder.SaveAsync();

            return LiquidJson(_cartBuilder.Cart.ToShopifyModel(WorkContext));
        }

        // POST: /cart/clear.js
        [HttpPost]
        [HandleJsonErrorAttribute]
        public async Task<ActionResult> ClearJs()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentLanguage, WorkContext.CurrentCurrency);

            await _cartBuilder.ClearAsync();
            await _cartBuilder.SaveAsync();

            return LiquidJson(_cartBuilder.Cart.ToShopifyModel(WorkContext));
        }

        private JsonResult LiquidJson(object obj)
        {
            var serializedString = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new RubyContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return new JsonResult
            {
                ContentEncoding = Encoding.UTF8,
                ContentType = "application/json",
                Data = JsonConvert.DeserializeObject(serializedString),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}