using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VirtoCommerce.Web.Controllers
{
    public class DownloadController : StoreControllerBase
    {
        // GET: Download
        [HttpGet]
        public async Task<ActionResult> Index(string file, string oid, string pid)
        {
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
                            var webClient = new WebClient();
                            var contentBytes = webClient.DownloadData(asset.Url);

                            return File(contentBytes, asset.MimeType, asset.Name);
                        }
                    }
                }
            }

            return HttpNotFound();
        }
    }
}