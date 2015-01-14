using System;
using System.Linq;
using System.Web.Mvc;
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

    }
}