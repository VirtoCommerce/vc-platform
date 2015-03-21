#region
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion

namespace VirtoCommerce.Web.Controllers
{
    [RoutePrefix("search")]
    public class SearchController : BaseController
    {
        #region Public Methods and Operators
        [Route("")]
        public async Task<ActionResult> SearchAsync(string type, string q)
        {
            this.Context.Set("Search", await this.Service.SearchAsync(type, q));
            return this.View("search");
        }
        #endregion
    }
}