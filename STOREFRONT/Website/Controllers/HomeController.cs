#region
using System.Linq;
using System.Threading.Tasks;
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