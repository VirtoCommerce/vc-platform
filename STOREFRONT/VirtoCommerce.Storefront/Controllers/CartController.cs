using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Builders;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    [RoutePrefix("cart")]
    public class CartController : StorefrontControllerBase
    {
        private readonly ICartBuilder _cartBuilder;
        private readonly ICatalogService _catalogService;

        public CartController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, ICartBuilder cartBuilder, ICatalogService catalogService)
            : base(workContext, urlBuilder)
        {
            _cartBuilder = cartBuilder;
            _catalogService = catalogService;
        }

        // GET: /cart
        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            return View("cart", WorkContext);
        }

        // GET: /cart/json
        [HttpGet]
        [Route("json")]
        public async Task<ActionResult> CartJson()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/changeitem?id=...&quantity=...
        [HttpPost]
        [Route("changeitem")]
        public async Task<ActionResult> ChangeItemJson(string id, int quantity)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            await _cartBuilder.UpdateItem(id, quantity).SaveAsync();

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // POST: /cart/removeitem?id=...
        [HttpPost]
        [Route("removeitem")]
        public async Task<ActionResult> RemoveItemJson(string id)
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(WorkContext.CurrentStore, WorkContext.CurrentCustomer, WorkContext.CurrentCurrency);

            await _cartBuilder.RemoveItem(id).SaveAsync();

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }

        // GET: /cart/checkout/{step}
        [HttpGet]
        [Route("checkout/{step}")]
        public ActionResult Checkout(string step)
        {
            return View("checkout", "checkout_layout", WorkContext);
        }
    }
}