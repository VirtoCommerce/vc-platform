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

        [HttpGet]
        public async Task<ActionResult> Download(string oid, string pid, string file)
        {
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
                var customerOrder = await CustomerService.GetOrderAsync(SiteContext.Current.StoreId, SiteContext.Current.CustomerId, oid);
                if (customerOrder != null)
                {
                    var firstApprovedPayment = customerOrder.InPayments.FirstOrDefault(p => p.IsApproved);
                    if (firstApprovedPayment != null)
                    {
                        var catalogItems = await Service.GetCatalogItemsByIdsAsync(new[] { pid }, "ItemAssets");
                        if (catalogItems != null && catalogItems.Any())
                        {
                            var asset = catalogItems.First().Assets.FirstOrDefault(a => a.Name == file);
                            if (asset != null)
                            {
                                return Redirect(asset.Url);
                            }
                        }
                    }
                }
            //}

            return HttpNotFound();
        }
        #endregion
    }
}