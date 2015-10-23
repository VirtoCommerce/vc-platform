#region
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Web.Models.Routing;

#endregion

namespace VirtoCommerce.Web.Controllers
{
    [Canonicalized(typeof(HomeController))]
    public class HomeController : StoreControllerBase
    {
        #region Public Methods and Operators
        //[OutputCache(CacheProfile = "HomeProfile")]
        public ActionResult Index()
        {
            return this.View("index");
        }
        #endregion
    }
}