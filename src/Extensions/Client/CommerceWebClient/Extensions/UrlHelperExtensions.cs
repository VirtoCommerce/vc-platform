using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.Web.Client.Extensions
{
    public static class UrlHelperExtensions
    {

        public static CatalogClient CatalogClient
        {
            get { return DependencyResolver.Current.GetService<CatalogClient>(); }
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
                routeValues.Add("item", parent.ItemId);
                if (item != null)
                {
                    routeValues.Add("variation", item.ItemId);
                }
                routeValues.Add("category", item.GetItemCategoryRouteValue());
            }
            else if (item != null)
            {
                routeValues.Add("item", item.ItemId);
                routeValues.Add("category", item.GetItemCategoryRouteValue());
            }
            return helper.RouteUrl("Item", routeValues);
        }

        //This can also be used for quicker but not exact url rendering, however it doesnt add performance boost
        public static string ItemUrlShort(this UrlHelper helper, Item item, Item parent)
        {
            if (parent != null)
            {
                return item != null ? string.Format("~/p/{0}?variation={1}", parent.Code, item.Code)
                    : string.Format("p/{0}", parent.Code);
            }

            if (item != null)
            {
                return string.Format("p/{0}", item.Code);
            }
            return string.Empty;
        }

    }
}