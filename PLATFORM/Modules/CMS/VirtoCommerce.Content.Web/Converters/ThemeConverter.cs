using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webModels = VirtoCommerce.Content.Web.Models;
using domainModels = VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Platform.Core.Asset;

namespace VirtoCommerce.Content.Web.Converters
{
	public static class ThemeConverter
	{
	
        public static webModels.Theme ToThemeWebModel(this BlobFolder folder)
        {
            var retVal = new webModels.Theme();

            retVal.Name = folder.Name;
            retVal.Path = folder.Url;
            return retVal;
        }
    }
}