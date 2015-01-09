using System;
using System.Linq;
using System.Web.Mvc;
using PublicWebApp.Models;
using VirtoCommerce.ApiClient.DataContracts.Store;
using VirtoCommerce.ApiWebClient.Clients;
using VirtoCommerce.ApiWebClient.Extensions;
using VirtoCommerce.ApiWebClient.Helpers;

namespace PublicWebApp.Controllers
{
    /// <summary>
    /// Class StoreController.
    /// </summary>
    public class CatalogController : ControllerBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreController"/> class.
        /// </summary>
        /// <param name="storeClient">The store client.</param>
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

    }
}