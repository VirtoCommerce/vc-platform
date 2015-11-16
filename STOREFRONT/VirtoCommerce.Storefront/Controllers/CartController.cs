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

        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            return View("cart", _workContext);
        }
    }
}