using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webModels = VirtoCommerce.ThemeModule.Web.Models;
using domainModels = VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.ThemeModule.Web.Converters
{
	public static class ThemeAssetConverter
	{
		public static domainModels.ThemeAsset ToDomainModel(this webModels.ThemeAsset item)
		{
			var retVal = new domainModels.ThemeAsset();

			retVal.Content = item.Content;
			retVal.Id = item.Id;
			retVal.ContentType = item.ContentType;

			return retVal;
		}

		public static webModels.ThemeAsset ToWebModel(this domainModels.ThemeAsset item)
		{
			var retVal = new webModels.ThemeAsset();

			retVal.Content = item.Content;
			retVal.Id = item.Id;
			retVal.ContentType = item.ContentType;

			return retVal;
		}
	}
}