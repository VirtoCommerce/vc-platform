using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Builders;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    [RoutePrefix("cart")]
    public class CartController : Controller
    {
        private readonly WorkContext _workContext;
        private readonly ICartBuilder _cartBuilder;
        private readonly ICatalogService _catalogService;

        public CartController(
            WorkContext workContext,
            ICartBuilder cartBuilder,
            ICatalogService catalogService)
        {
            _workContext = workContext;
            _cartBuilder = cartBuilder;
            _catalogService = catalogService;
        }

        #region "Methods for compatibility with Shopify themes"

        // GET: /cart
        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            return View("cart", _workContext);
        }

        // POST: /cart/add?id=...&quantity=...
        [HttpPost]
        [Route("add")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(string id, int quantity = 1)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(_workContext.CurrentStore, _workContext.CurrentCustomer, _workContext.CurrentCurrency);

            var product = await _catalogService.GetProduct(id, _workContext.CurrentCurrency.Code, Model.Catalog.ItemResponseGroup.ItemLarge);
            if (product != null)
            {
                await _cartBuilder.AddItem(product, quantity).SaveAsync();
            }

            return RedirectToAction("Index", "Cart");
        }

        // GET: /cart/change?line=...&quantity=...
        [HttpGet]
        [Route("change")]
        public async Task<ActionResult> Change(int line, int quantity)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(_workContext.CurrentStore, _workContext.CurrentCustomer, _workContext.CurrentCurrency);

            await _cartBuilder.UpdateItem(line - 1, quantity).SaveAsync();

            return RedirectToAction("Index", "Cart");
        }

        // POST: /cart
        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Cart(int[] updates, string update)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(_workContext.CurrentStore, _workContext.CurrentCustomer, _workContext.CurrentCurrency);

            for (var i = 0; i < updates.Length; i++)
            {
                _cartBuilder.UpdateItem(i, updates[i]);
            }

            await _cartBuilder.SaveAsync();

            if (string.IsNullOrEmpty(update))
            {
                return Redirect("~/cart/customer_information");
            }

            return RedirectToAction("Index", "Cart");
        }

        // GET: /cart?step=...
        [HttpGet]
        [Route("")]
        public ActionResult Checkout(string step)
        {
            return View("checkout", "checkout_layout", _workContext);
        }

        #endregion

        // GET: /cart/json
        [HttpGet]
        [Route("json")]
        public async Task<ActionResult> CartJson()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(_workContext.CurrentStore, _workContext.CurrentCustomer, _workContext.CurrentCurrency);

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }
    }
}