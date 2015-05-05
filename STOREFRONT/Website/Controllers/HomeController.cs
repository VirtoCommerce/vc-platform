#region
using System.Web.Mvc;

#endregion

namespace VirtoCommerce.Web.Controllers
{
    public class HomeController : StoreControllerBase
    {
        #region Public Methods and Operators
        public ActionResult Index()
        {
            return this.View();
        }
        #endregion
    }
}