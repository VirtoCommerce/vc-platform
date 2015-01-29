using MvcSiteMapProvider;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiClient.Session;
using VirtoCommerce.ApiWebClient.Caching;
using VirtoCommerce.ApiWebClient.Extensions;
using VirtoCommerce.ApiWebClient.Helpers;
using VirtoCommerce.Web.Converters;
using VirtoCommerce.Web.Core.DataContracts;
using VirtoCommerce.Web.Models;

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

         [DonutOutputCache(CacheProfile = "CatalogCache", VaryByCustom = "currency;cart")]
        public async Task<ActionResult> DisplayItem(string item)
        {
            var itemModel = await GetItem(item, ItemResponseGroups.ItemLarge);

            if (ReferenceEquals(itemModel, null))
            {
                throw new HttpException(404, "Item not found");
            }

            if (SiteMaps.Current != null)
            {
                var node = SiteMaps.Current.CurrentNode;

                if (Request.UrlReferrer != null &&
                    Request.UrlReferrer.AbsoluteUri.StartsWith(Request.Url.GetLeftPart(UriPartial.Authority)))
                {
                    if (node != null)
                    {
                        node.RootNode.Attributes["ShowBack"] = true;
                    }

                    if (Request.UrlReferrer.AbsoluteUri.Equals(Request.Url.AbsoluteUri))
                    {
                        ClientContext.Session.LastShoppingPage = Url.Content("~/");
                    }
                    else
                    {
                        ClientContext.Session.LastShoppingPage = Request.UrlReferrer.AbsoluteUri;
                    }

                }

                if (node != null)
                {
                    if (node.ParentNode != null && itemModel.CatalogItem.Outline != null)
                    {

                        node.Attributes["Outline"] = itemModel.Category.BuildTitleOutline();
                    }
                    node.Title = itemModel.DisplayName;
                }
            }

            return View(itemModel.DisplayTemplate, itemModel);
        }

         [ChildActionOnly, DonutOutputCache(CacheProfile = "CatalogCache", Duration = 0)]
         public ActionResult ItemDynamic(string id)
         {
             var session = ClientContext.Session;
             var itemModel = Task.Run(() => GetItem(id, session: session)).Result;
             return ReferenceEquals(itemModel, null) ? null : PartialView(itemModel);
         }

        [ChildActionOnly]
        public ActionResult DisplayDynamic(string itemCode)
        {
            try
            {
                var session = ClientContext.Session;
                var model = Task.Run(() => GetItem(itemCode, session: session, byCode: true)).Result;
                return PartialView("DisplayTemplates/Item", model);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Displays item variations.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="selections">The selections.</param>
        /// <param name="variation">The selected variation item code.</param>
        /// <returns>ActionResult.</returns>
        [DonutOutputCache(CacheProfile = "CatalogCache")]
        public async Task<ActionResult> ItemVariations(string itemId, string name, string[] selections = null,
                                           string variation = null)
        {
            var item = await GetItem(itemId, ItemResponseGroups.ItemLarge);
            var product = item.CatalogItem as Product;

            if (product == null || product.Variations == null || !product.Variations.Any())
                return null;

            var variations = product.Variations;
            var selectedVariation = string.IsNullOrEmpty(variation) ? null : variations.FirstOrDefault(x=>x.Id == variation);
            var model = new VariationsModel(variations, selections, selectedVariation);
            return PartialView(name, model);
        }

        [ChildActionOnly]
        public ActionResult AssociatedItem(string itemid, string name, string associationtype)
        {
            var session = ClientContext.Session;
            var item = Task.Run(() => GetItem(itemid, session: session, associationType: associationtype)).Result;
            return item != null ? PartialView(name, item) : null;
        }

        /// <summary>
        /// Adds the review.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [DonutOutputCache(CacheProfile = "CatalogCache")]
        public ActionResult AddReview(string id)
        {
            if (StoreHelper.CustomerSession.IsRegistered)
            {
                return PartialView("CreateReview",
                    new MReview
                    {
                        ItemId = id,
                        CreatedDateTime = DateTime.UtcNow,
                        Reviewer = new MReviewer { NickName = StoreHelper.CustomerSession.CustomerName }
                    });
            }
            return RedirectToAction("LogOnAsync", "Account");
        }

        #region Private helpers

        private async Task<ItemModel> GetItem(string item, ItemResponseGroups responseGroup = ItemResponseGroups.ItemMedium, ICustomerSession session = null, bool byCode = false, string associationType = null)
        {
            session = session ?? ClientContext.Session;

            //Create client
            var client = ClientContext.Clients.CreateBrowseClient(session.StoreId, session.Language);

            //Get product
            var product = byCode ?
                await client.GetProductByCodeAsync(item, responseGroup) :
                await client.GetProductAsync(item, responseGroup);

            //Get reviews
            var reviewClient = ClientContext.Clients.CreateReviewsClient(session.Language);
            var productReviews = Task.Run(() => reviewClient.GetReviewsAsync(product.Id)).Result;
            var model = product.ToWebModel(associationType);
            if (productReviews.TotalCount > 0)
            {
                model.CatalogItem.ReviewsTotal = productReviews.TotalCount;
                model.CatalogItem.Rating = productReviews.Items.Average(x => x.Rating);
            }

            if (responseGroup == ItemResponseGroups.ItemLarge)
            {
                //Load item category if full item is loaded
                if (!string.IsNullOrEmpty(model.CategoryId))
                {
                    model.Category = await client.GetCategoryAsync(model.CategoryId);
                }
            }

          
            return model;
        }

        #endregion
    }
}