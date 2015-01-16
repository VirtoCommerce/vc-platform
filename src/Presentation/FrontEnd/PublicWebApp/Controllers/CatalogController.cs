using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.ApiClient;
using VirtoCommerce.Web.Converters;
using VirtoCommerce.Web.Models;
using VirtoCommerce.ApiWebClient.Clients;
using VirtoCommerce.ApiWebClient.Extensions;
using VirtoCommerce.ApiWebClient.Helpers;

namespace VirtoCommerce.Web.Controllers
{
    /// <summary>
    /// Class StoreController.
    /// </summary>
    public class CatalogController : ControllerBase
    {
        public CatalogController()
        {
        }

        /// <summary>
        /// Show available currencies
        /// </summary>
        /// <returns>ActionResult.</returns>
        //[ChildActionOnly, DonutOutputCache(CacheProfile = "StoreCache", VaryByParam = Constants.Language, VaryByCustom = "currency")]
        public ActionResult Display(string category)
        {
            return null;
        }

        public ActionResult DisplayItem(string item)
        {
            return null;
        }

        [ChildActionOnly]
        public ActionResult DisplayDynamic(string itemCode)
        {
            try
            {
                var session = StoreHelper.CustomerSession;
                var product = Task.Run(() => CatalogHelper.CatalogClient.GetItemByCodeAsync(itemCode, session.CatalogId, session.Language)).Result;
                //var reviews = Task.Run(() => ReviewsClient.GetReviewsAsync(product.Id)).Result;
                var model = product.ToWebModel();
                //model.Rating = reviews.TotalCount > 0 ? reviews.Items.Average(x => x.Rating) : 0;
                return PartialView("DisplayTemplates/Item", model);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}