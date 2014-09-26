using MvcSiteMapProvider;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Frameworks.Tagging;
using VirtoCommerce.Web.Client.Caching;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Virto.Helpers;
using AppConfigContext = VirtoCommerce.Foundation.AppConfig.Model.ContextFieldConstants;
using ContextFieldConstants = VirtoCommerce.Foundation.Frameworks.ContextFieldConstants;

namespace VirtoCommerce.Web.Controllers
{
    /// <summary>
	/// Class CatalogController.
	/// </summary>
	public class CatalogController : ControllerBase
    {
		/// <summary>
		/// The _catalog client
		/// </summary>
        private readonly CatalogClient _catalogClient;

		/// <summary>
		/// The _template client
		/// </summary>
        private readonly DisplayTemplateClient _templateClient;


        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogController" /> class.
        /// </summary>
        /// <param name="catalogClient">The catalog client.</param>
        /// <param name="templateClient">The template client.</param>
        public CatalogController(CatalogClient catalogClient,
                                 DisplayTemplateClient templateClient)
        {
			_catalogClient = catalogClient;
            _templateClient = templateClient;
        }

        // GET: /Catalog/
	    /// <summary>
	    /// Displays the catalog by specified URL.
	    /// </summary>
        /// <param name="category">The category code</param>
	    /// <returns>ActionResult.</returns>
	    /// <exception cref="System.Web.HttpException">404;Category not found</exception>
        [DonutOutputCache(CacheProfile = "CatalogCache", VaryByCustom = "currency;filters;pricelist")]
        public ActionResult Display(CategoryPathModel category)
        {
            var categoryBase = _catalogClient.GetCategoryById(category.Category);
            if (categoryBase != null && categoryBase.IsActive)
            {
                // set the context variable
                var set = UserHelper.CustomerSession.GetCustomerTagSet();
                set.Add(ContextFieldConstants.CategoryId, new Tag(categoryBase.CategoryId));
                UserHelper.CustomerSession.CategoryId = categoryBase.CategoryId;

                var model = CatalogHelper.CreateCategoryModel(categoryBase);

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
                            UserHelper.CustomerSession.LastShoppingPage = Url.Content("~/");
                        }
                        else
                        {
                            UserHelper.CustomerSession.LastShoppingPage = Request.UrlReferrer.AbsoluteUri;
                        }

                    }

                    if (node != null)
                    {
                        //if (node.ParentNode != null && model.CatalogOutline !=null)
                        //{

                        //    node.Attributes["Outline"] = new BrowsingOutline(model.CatalogOutline);
                        //}

                        node.Title = model.DisplayName;
                    }
                }

                // display category
                return View(GetDisplayTemplate(TargetTypes.Category, categoryBase), model);
            }

			throw new HttpException(404, "Category not found");
        }

	    /// <summary>
	    /// Displays the item by code.
	    /// </summary>
        /// <param name="item">Item code</param>
	    /// <returns>ActionResult.</returns>
	    /// <exception cref="System.Web.HttpException">404;Item not found</exception>
        [DonutOutputCache(CacheProfile = "CatalogCache", VaryByCustom = "currency;cart")]
        public ActionResult DisplayItem(string item)
        {
            var itemModel = CatalogHelper.CreateCatalogModel(item);

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
	                    UserHelper.CustomerSession.LastShoppingPage = Url.Content("~/");
	                }
	                else
	                {
	                    UserHelper.CustomerSession.LastShoppingPage = Request.UrlReferrer.AbsoluteUri;
	                }

	            }

                if (node != null)
                {
                    if (node.ParentNode != null && itemModel.CatalogItem.CatalogOutlines != null
                        && itemModel.CatalogItem.CatalogOutlines.Count > 0)
                    {

                        node.Attributes["Outline"] = new BrowsingOutline(itemModel.CatalogItem.CatalogOutlines[0]);
                    }
                    node.Title = itemModel.DisplayName;
                }
	        }

            return View(GetDisplayTemplate(TargetTypes.Item, itemModel.CatalogItem), itemModel);
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
        public ActionResult ItemVariations(string itemId, string name, string[] selections = null,
                                           string variation = null)
        {
            var variations = _catalogClient.GetItemRelations(itemId);
            var selectedVariation = string.IsNullOrEmpty(variation) ? null : _catalogClient.GetItem(variation, StoreHelper.CustomerSession.CatalogId);
            var model = new VariationsModel(variations, selections, selectedVariation);
            return PartialView(name, model);
        }

		/// <summary>
		/// Associations for the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="templateName">Name of the display template.</param>
		/// <param name="groupName">Name of the association group.</param>
		/// <returns>ActionResult.</returns>
        [DonutOutputCache(CacheProfile = "CatalogCache")]
		public ActionResult Association(ItemModel item, string templateName, string groupName)
        {
            var currentGroup = item.AssociationGroups
                                   .FirstOrDefault(
									   ag => ag.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));
			return currentGroup == null ? null : PartialView(templateName, currentGroup);
        }

		/// <summary>
		/// Renders associated item.
		/// </summary>
		/// <param name="itemId">The item identifier.</param>
		/// <param name="name">The name.</param>
		/// <param name="associationType">Type of the association.</param>
		/// <returns>ActionResult.</returns>
        public ActionResult AssociatedItem(string itemId, string name, string associationType)
        {
			return DisplayItemById(itemId, name: name, associationType: associationType);
        }

		/// <summary>
		/// Displays the item by identifier.
		/// </summary>
		/// <param name="itemId">The item identifier.</param>
		/// <param name="parentItemId">The parent item identifier.</param>
		/// <param name="name">The name.</param>
		/// <param name="associationType">Type of the association.</param>
		/// <param name="forcedActive">if set to <c>true</c> [forced active].</param>
		/// <param name="responseGroups">The response groups.</param>
		/// <param name="displayOptions">The display options.</param>
		/// <returns>ActionResult.</returns>
        [DonutOutputCache(CacheProfile = "CatalogCache")]
		public ActionResult DisplayItemById(string itemId, 
            string parentItemId = null, 
            string name = "MiniItem", 
            string associationType = null, 
            bool forcedActive = false, 
            ItemResponseGroups responseGroups = ItemResponseGroups.ItemSmall, 
            ItemDisplayOptions displayOptions = ItemDisplayOptions.ItemSmall)
		{
			return DisplayItemNoCache(itemId, parentItemId, name, associationType, forcedActive, responseGroups,
			                              displayOptions);
		}

		//Called from banner/ProductImageAndPrice to avoid nested caching
        /// <summary>
        /// Displays the item by identifier no cache.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="parentItemId">The parent item identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="associationType">Type of the association.</param>
        /// <param name="forcedActive">if set to <c>true</c> [forced active].</param>
        /// <param name="responseGroups">The response groups.</param>
        /// <param name="displayOptions">The display options.</param>
        /// <param name="bycode"></param>
        /// <returns>ActionResult.</returns>
        public ActionResult DisplayItemNoCache(string itemId, 
            string parentItemId = null, 
            string name = "MiniItem", 
            string associationType = null, 
            bool forcedActive = false, 
            ItemResponseGroups responseGroups = ItemResponseGroups.ItemSmall, 
            ItemDisplayOptions displayOptions = ItemDisplayOptions.ItemSmall, 
            bool bycode = false)
        {
            var itemModel = CatalogHelper.CreateCatalogModel(itemId, parentItemId, associationType, forcedActive, responseGroups, displayOptions, bycode);
            return itemModel != null ? PartialView(name, itemModel) : null;
        }

		/// <summary>
		/// Adds the review.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>ActionResult.</returns>
        [DonutOutputCache(CacheProfile = "CatalogCache")]
        public ActionResult AddReview(string id)
		{
		    if (UserHelper.CustomerSession.IsRegistered)
		    {
		        return PartialView("CreateReview",
		            new MReview
		            {
		                ItemId = id,
		                CreatedDateTime = DateTime.UtcNow,
		                Reviewer = new MReviewer {NickName = UserHelper.CustomerSession.CustomerName}
		            });
		    }
		    return RedirectToAction("LogOnAsync", "Account");
		}

        [ChildActionOnly, DonutOutputCache(CacheProfile = "CatalogCache", Duration = 0)]
        public ActionResult ItemDynamic(string item)
        {
            var itemModel = CatalogHelper.CreateCatalogModel(item);
            return ReferenceEquals(itemModel, null) ? null : PartialView(itemModel);
        }

		/// <summary>
		/// Method uses displayTemplateClient to resolve which display template should be displayed
		/// based on current context
		/// </summary>
		/// <param name="type">Target type (Item, Category)</param>
		/// <param name="displayObject">The display object.</param>
		/// <returns>display template name</returns>
        private string GetDisplayTemplate(TargetTypes type, object displayObject)
        {
            // set the context variable
            var set = UserHelper.CustomerSession.GetCustomerTagSet();

            //Add Tags for category
            switch (type)
            {
                case TargetTypes.Category:
                    var category = displayObject as CategoryBase;
                    if (category != null)
                    {
                        set.Add(AppConfigContext.CategoryId, new Tag(category.CategoryId));
                    }
                    break;
                case TargetTypes.Item:
                    var item = displayObject as Item;
                    if (item != null)
                    {
                        set.Add(AppConfigContext.ItemId, new Tag(item.ItemId));
                        set.Add(AppConfigContext.ItemType, new Tag(item.GetType().Name));
                    }
                    break;
            }

            var viewName = _templateClient.GetDisplayTemplate(type, set);
            return string.IsNullOrEmpty(viewName) ? type.ToString() : viewName;
        }
    }
}