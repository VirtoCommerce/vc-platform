using System;
using System.IO;
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
            var retVal = new ThemeAsset { Id = item.Path, ByteContent = item.ByteContent, ContentType = item.ContentType, Updated = item.ModifiedDate.HasValue ? RemoveMilliseconds(item.ModifiedDate.Value) : RemoveMilliseconds(item.CreatedDate) };

		    return retVal;
		}

		public static ContentItem AsContentItem(this ThemeAsset asset)
		{
			var retVal = new ContentItem { Path = asset.Id, ByteContent = asset.ByteContent, ContentType = asset.ContentType, Name = Path.GetFileName(asset.AssetName) };

		    return retVal;
		}

	    public static DateTime RemoveMilliseconds(this DateTime dateTime)
	    {
	        if (dateTime.Millisecond > 0)
	        {
	            dateTime = dateTime.AddSeconds(1);
	            dateTime = dateTime.AddMilliseconds(-dateTime.Millisecond);
	        }

	        return dateTime;
	    }
	}
}
