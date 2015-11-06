using System.Collections.Generic;
using System.Linq;
using webModels = VirtoCommerce.Content.Web.Models;
using domainModels = VirtoCommerce.Content.Data.Models;
using System.Text;
using VirtoCommerce.Content.Data.Utility;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Content.Web.Converters
{
	public static class ThemeAssetConverter
	{
		public static domainModels.ThemeAsset ToDomainModel(this webModels.ThemeAsset item)
		{
			var retVal = new domainModels.ThemeAsset();

			if (item.ByteContent != null)
				retVal.ByteContent = item.ByteContent;
			else if (!string.IsNullOrEmpty(item.Content))
				retVal.ByteContent = Encoding.UTF8.GetBytes(item.Content);

			retVal.AssetUrl = item.AssetUrl;
			retVal.Id = item.Id;
			retVal.ContentType = item.ContentType;
			retVal.AssetName = item.Name;

			return retVal;
		}

		public static webModels.ThemeAsset ToWebModel(this domainModels.ThemeAsset item, bool loadContent = true)
		{
			var retVal = new webModels.ThemeAsset();

            if (loadContent)
            {
                if (ContentTypeUtility.IsImageContentType(item.ContentType))
                {
                    if (item.ByteContent != null)
                    {
                        //retVal.Content = ContentTypeUtility.ConvertImageToBase64String(item.ByteContent, item.ContentType);
                        retVal.ByteContent = item.ByteContent;
                    }
                    else
                    {
                        retVal.Content = retVal.AssetUrl = item.AssetUrl;
                    }
                }
                else if (ContentTypeUtility.IsTextContentType(item.ContentType))
                {
                    if (item.ByteContent != null)
                    {
                        retVal.Content = Encoding.UTF8.GetString(item.ByteContent);
                    }
                }
                else // treat as a binary file for now
                {
                    if (item.ByteContent != null)
                    {
                        retVal.ByteContent = item.ByteContent;
                    }
                }
            }
			retVal.Id = item.Id;
			retVal.ContentType = item.ContentType;
            retVal.Updated = item.Updated;
			retVal.Name = string.Join("/", item.Id.Split('/').AsEnumerable().Skip(1));

			return retVal;
		}

		public static webModels.ThemeAssetFolder[] ToWebModel(this IEnumerable<domainModels.ThemeAsset> items)
		{
			var retVal = new List<webModels.ThemeAssetFolder>();

			var folders = items.Select(i => i.Id.Split('/')[0]).Distinct();

			foreach (var folder in folders)
			{
				var themeAssetFolder = new webModels.ThemeAssetFolder
				{
					FolderName = folder
				};

				themeAssetFolder.Assets.AddRange(items.Select(i => i.ToWebModel(false)).Where(i => i.Id.StartsWith(string.Format("{0}/", folder))));

				retVal.Add(themeAssetFolder);
			}

			return retVal.ToArray();
		}
	}
}