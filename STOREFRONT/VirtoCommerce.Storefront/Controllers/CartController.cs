using System.Web.Mvc;
using VirtoCommerce.Storefront.Builders;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Controllers
{
    [RoutePrefix("cart")]
    public class CartController : Controller
    {
        private readonly WorkContext _workContext;
        private readonly ICartBuilder _cartBuilder;

        public CartController(WorkContext workContext, ICartBuilder cartBuilder)
        {
            _workContext = workContext;
            _cartBuilder = cartBuilder;
        }

        // GET: /cart
        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            return View("cart", "cart_layout", _workContext);
        }

        // POST: /cart/add?id=...&quantity=...
        [HttpPost]
        [Route("add")]
        public ActionResult Add(string id, int quantity = 1)
        {
            return RedirectToAction("Index");
        }
    }
}