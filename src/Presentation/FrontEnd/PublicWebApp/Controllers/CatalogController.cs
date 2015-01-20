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
    using VirtoCommerce.ApiClient.Extensions;
    using VirtoCommerce.Web.Core.DataContracts;

    /// <summary>
    /// Class StoreController.
    /// </summary>
    public class CatalogController : ControllerBase
    {
        public CatalogController()
        {
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
                var client = ClientContext.Clients.CreateBrowseClient(ClientContext.Session.CatalogId, ClientContext.Session.Language);
                var product = Task.Run(() => client.GetProductByCodeAsync(itemCode, ItemResponseGroups.ItemMedium)).Result;
                var model = product.ToWebModel();
                return PartialView("DisplayTemplates/Item", model);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}