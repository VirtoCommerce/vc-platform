#region
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Web.Models;

#endregion

namespace VirtoCommerce.Web.Controllers
{
    [RoutePrefix("search")]
    public class SearchController : StoreControllerBase
    {
        #region Public Methods and Operators
        [Route("")]
        public async Task<ActionResult> SearchAsync(string type, string q, int page = 1)
        {
            await Task.FromResult<object>(null);
            Context.Set("current_page", page);
            //Context.Set("current_query", q);
            Context.Set("current_type", type);
            Context.Set("Search", new Search() { Terms = q, Performed = true });
            return View("search");
        }
        #endregion
    }
}