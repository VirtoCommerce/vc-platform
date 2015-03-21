#region
using System.Web.Mvc;

#endregion

namespace VirtoCommerce.Web.Controllers
{
    public class HomeController : BaseController
    {
        #region Public Methods and Operators
        public ActionResult Index()
        {
            return this.View();
        }
        #endregion
    }
}