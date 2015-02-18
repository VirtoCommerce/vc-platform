using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webModels = VirtoCommerce.ThemeModule.Web.Models;
using domainModels = VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.ThemeModule.Web.Converters
{
	public static class ThemeConverter
	{
		public static domainModels.Theme ToDomainModel(this webModels.Theme item)
		{
			var retVal = new domainModels.Theme();

			retVal.Name = item.Name;
			retVal.ThemePath = item.Path;

			return retVal;
		}

		public static webModels.Theme ToWebModel(this domainModels.Theme item)
		{
			var retVal = new webModels.Theme();

			retVal.Name = item.Name;
			retVal.Path = item.ThemePath;
			retVal.Modified = item.ModifiedDate ?? item.CreatedDate;

			return retVal;
		}
	}
}