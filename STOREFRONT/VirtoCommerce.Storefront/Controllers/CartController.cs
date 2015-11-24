using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Builders;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
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
            return View("cart", base.WorkContext);
        }
     

        // GET: /cart/json
        [HttpGet]
        [Route("json")]
        public async Task<ActionResult> CartJson()
        {
            await _cartBuilder.GetOrCreateNewTransientCartAsync(base.WorkContext.CurrentStore, base.WorkContext.CurrentCustomer, base.WorkContext.CurrentCurrency);

            return Json(_cartBuilder.Cart, JsonRequestBehavior.AllowGet);
        }
    }
}