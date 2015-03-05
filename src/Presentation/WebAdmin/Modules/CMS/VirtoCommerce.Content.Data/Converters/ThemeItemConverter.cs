using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Utility;

namespace VirtoCommerce.Content.Data.Converters
{
	public static class ThemeItemConverter
	{
		public static Theme AsTheme(this ContentItem item)
		{
			var retVal = new Theme { Name = item.Path.Replace("/", string.Empty), ThemePath = item.Path };
			return retVal;
		}

		public static ThemeAsset AsThemeAsset(this ContentItem item)
		{
			var retVal = new ThemeAsset();

			retVal.Id = item.Path;
			retVal.ByteContent = item.ByteContent;
			retVal.ContentType = item.ContentType;

			return retVal;
		}

		public static ContentItem AsContentItem(this ThemeAsset asset)
		{
			var retVal = new ContentItem();

			retVal.Path = asset.Id;
			retVal.ByteContent = asset.ByteContent;
			retVal.ContentType = asset.ContentType;

			return retVal;
		}
	}
}
