#region
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Web.Models;

#endregion

namespace VirtoCommerce.Web.Controllers
{
    [RoutePrefix("search")]
    public class SearchController : BaseController
    {
        #region Public Methods and Operators
        [Route("")]
        public async Task<ActionResult> SearchAsync(string type, string q, int page = 1)
        {
            this.Context.Set("current_page", page);
            //this.Context.Set("current_query", q);
            this.Context.Set("current_type", type);
            this.Context.Set("Search", new Search() { Terms = q, Performed = true });
            return this.View("search");
        }
        #endregion
    }
}