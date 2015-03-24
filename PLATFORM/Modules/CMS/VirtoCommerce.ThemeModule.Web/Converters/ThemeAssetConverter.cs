using System.Collections.Generic;
using System.Linq;
using webModels = VirtoCommerce.ThemeModule.Web.Models;
using domainModels = VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Framework.Web.Common;
using System.Text;
using VirtoCommerce.Content.Data.Utility;

namespace VirtoCommerce.ThemeModule.Web.Converters
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

		public static webModels.ThemeAsset ToWebModel(this domainModels.ThemeAsset item)
		{
			var retVal = new webModels.ThemeAsset();

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
			retVal.Id = item.Id;
			retVal.ContentType = item.ContentType;
            retVal.Updated = item.Updated;
			retVal.Name = string.Join("/", item.Id.Split('/').AsEnumerable().Skip(1));

			return retVal;
		}

		public static webModels.ThemeAssetFolder[] ToWebModel(this IEnumerable<domainModels.ThemeAsset> items)
		{
			var retVal = new List<Models.ThemeAssetFolder>();

			var folders = items.Select(i => i.Id.Split('/')[0]).Distinct();

			foreach (var folder in folders)
			{
				var themeAssetFolder = new Models.ThemeAssetFolder
				{
					FolderName = folder
				};

				themeAssetFolder.Assets.AddRange(items.Select(i => i.ToWebModel()).Where(i => i.Id.StartsWith(string.Format("{0}/", folder))));

				retVal.Add(themeAssetFolder);
			}

			return retVal.ToArray();
		}
	}
}