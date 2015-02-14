using VirtoCommerce.Content.Data.Models;

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
            var retVal = new ThemeAsset { Id = item.Path.Replace("/", string.Empty) };
            return retVal;
        }

        public static ContentItem AsContentItem(this ThemeAsset asset)
        {
            var retVal = new ContentItem { Id = asset.Id };
            return retVal;
        }
	}
}
