using System.Web.Mvc;

namespace VirtoCommerce.Platform.Web.Controllers
{
    public class HelpController : Controller
    {
        [Route("~/collections")]
        public ActionResult RedirectToCategory(string tutorial)
        {
            return this.Redirect("#/catalog");
        }

        [Route("~/products")]
        public ActionResult RedirectToProducts()
        {
            return this.Redirect("#/catalog");
        }
    }
}