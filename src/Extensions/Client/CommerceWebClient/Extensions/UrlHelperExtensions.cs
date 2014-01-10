using System.Linq;
using System.Web.Routing;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Customers;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Web.Client.Extensions.RouteHandlers;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Client.Extensions
{
    public static class UrlHelperExtensions
    {

        public static ICatalogOutlineBuilder OutlineBuilder
        {
            get { return DependencyResolver.Current.GetService<ICatalogOutlineBuilder>(); }
        }

        public static CatalogClient CatalogClient
        {
            get { return DependencyResolver.Current.GetService<CatalogClient>(); }
        }

        public static ICustomerSession CustomerSession
        {
            get
            {
                var session = ServiceLocator.Current.GetInstance<ICustomerSessionService>();
                return session.CustomerSession;
            }
        }

        public static string Image(this UrlHelper helper, Item item, string name)
        {
            const string defaultImage = "blank.png";

            if (item == null)
                return null;

            var asset = FindItemAsset(item.ItemAssets, name);

            return helper.Content(asset == null ? String.Format("~/Content/themes/default/images/{0}", defaultImage) : AssetUrl(asset));
        }

        public static string Image(this UrlHelper helper, ItemAsset asset)
        {
            const string defaultImage = "blank.png";

            return helper.Content(asset == null ? String.Format("~/Content/themes/default/images/{0}", defaultImage) : AssetUrl(asset));
        }

        public static string ImageThumbnail(this UrlHelper helper, ItemAsset asset)
        {
            const string defaultImage = "blank.png";

            return helper.Content(asset == null ? String.Format("~/Content/themes/default/images/{0}", defaultImage) : AssetUrl(asset, true));
        }

        private static string AssetUrl(ItemAsset asset, bool thumb = false)
        {
            var service = ServiceLocator.Current.GetInstance<IAssetUrl>();
            return service.ResolveUrl(asset.AssetId, thumb);
        }

        private static ItemAsset FindItemAsset(IEnumerable<ItemAsset> assets, string name)
        {
            return assets == null ? null : assets.OrderBy(x => x.SortOrder).FirstOrDefault(asset => asset.GroupName.Equals(name, StringComparison.OrdinalIgnoreCase));
        }


        public static string ItemUrl(this UrlHelper helper, string itemId, string parentItemId)
        {
            return helper.ItemUrl(!string.IsNullOrEmpty(itemId) ? CatalogClient.GetItem(itemId) : null,
                !string.IsNullOrEmpty(parentItemId) ? CatalogClient.GetItem(parentItemId) : null);
        }

        public static string ItemUrl(this UrlHelper helper, Item item, string parentItemId)
        {
            return helper.ItemUrl(item, !string.IsNullOrEmpty(parentItemId) ? CatalogClient.GetItem(parentItemId) : null);
        }

        public static string ItemUrl(this UrlHelper helper, Item item, Item parent)
        {
            var routeValues = new RouteValueDictionary();

          
            if (parent != null)
            {
                string parentId = ItemRouteHandler.ItemMappings.ContainsValue(parent.ItemId)
                    ? ItemRouteHandler.ItemMappings.First(x => x.Value == parent.ItemId).Key
                    : parent.ItemId;

                routeValues.Add("item", parentId);
                if (item != null)
                {
                    string itemId = ItemRouteHandler.ItemMappings.ContainsValue(item.ItemId)
                        ? ItemRouteHandler.ItemMappings.First(x => x.Value == item.ItemId).Key
                        : item.ItemId;
                    routeValues.Add("variationId", itemId);
                }
                routeValues.Add("category", OutlineBuilder.BuildCategoryOutline(CustomerSession.CatalogId, parent).Outlines[0].Categories.OfType<Category>().Last().Code);
                return helper.RouteUrl("Item", routeValues);
            }

            if (item != null)
            {
                string itemId = ItemRouteHandler.ItemMappings.ContainsValue(item.ItemId)
                ? ItemRouteHandler.ItemMappings.First(x => x.Value == item.ItemId).Key
                : item.ItemId;

                routeValues.Add("item", itemId);
                routeValues.Add("category", OutlineBuilder.BuildCategoryOutline(CustomerSession.CatalogId, item).Outlines[0].Categories.OfType<Category>().Last().Code);
                return helper.RouteUrl("Item", routeValues);
            }

            return string.Empty;
        }
    }
}