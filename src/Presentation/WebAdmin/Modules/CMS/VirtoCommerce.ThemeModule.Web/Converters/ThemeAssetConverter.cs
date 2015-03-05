using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webModels = VirtoCommerce.ThemeModule.Web.Models;
using domainModels = VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Framework.Web.Common;
using System.IO;
using System.Text;

namespace VirtoCommerce.ThemeModule.Web.Converters
{
	public static class ThemeAssetConverter
	{
		public static domainModels.ThemeAsset ToDomainModel(this webModels.ThemeAsset item)
		{
			var retVal = new domainModels.ThemeAsset();

			retVal.ByteContent = Encoding.UTF8.GetBytes(item.Content);
			retVal.Id = item.Id;
			retVal.ContentType = item.ContentType;

			return retVal;
		}

		public static domainModels.ThemeAsset DomainModel(string id, string contentType, byte[] content)
		{
			var retVal = new domainModels.ThemeAsset();

			retVal.ByteContent = content;
			retVal.Id = id;
			retVal.ContentType = contentType;

			return retVal;
		}

		public static webModels.ThemeAsset ToWebModel(this domainModels.ThemeAsset item)
		{
			var retVal = new webModels.ThemeAsset();

			if (item.ByteContent != null)
			{
				retVal.Content = Encoding.UTF8.GetString(item.ByteContent);
			}
			retVal.Id = item.Id;
			retVal.ContentType = item.ContentType;
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