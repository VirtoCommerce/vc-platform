using System.Linq;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.Web.Client.Extensions
{
	public static class UrlHelperExtensions
	{
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


		public static string ItemUrl(this UrlHelper helper, string itemId, string parentId)
		{
			if (!string.IsNullOrEmpty(parentId))
			{
				return helper.Content(String.Format("~/p/{0}{1}", parentId,
					!string.IsNullOrEmpty(itemId) ? "?variationId=" + itemId : string.Empty));
			}
			return !string.IsNullOrEmpty(itemId) ? helper.Content(String.Format("~/p/{0}", itemId)) : string.Empty;
		}

		public static string ItemUrl(this UrlHelper helper, Item item)
		{
			return helper.ItemUrl(item, (Item)null);
		}

		public static string ItemUrl(this UrlHelper helper, Item item, string parentItemId)
		{
			return helper.ItemUrl(item != null ? item.ItemId : null, parentItemId);
		}

		public static string ItemUrl(this UrlHelper helper, Item item, Item parent)
		{
			var itemId = item != null ? item.ItemId : null;
			var parentId = parent != null ? parent.ItemId : null;

			return helper.ItemUrl(itemId, parentId);
		}
	}
}