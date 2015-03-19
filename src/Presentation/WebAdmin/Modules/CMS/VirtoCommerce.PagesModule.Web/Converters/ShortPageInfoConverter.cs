using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreModels = VirtoCommerce.Content.Pages.Data.Models;
using webModels = VirtoCommerce.PagesModule.Web.Models;

namespace VirtoCommerce.PagesModule.Web.Converters
{
	public static class ShortPageInfoConverter
	{
		public static webModels.ShortPageInfo ToWebModel(this coreModels.ShortPageInfo item)
		{
			var retVal = new webModels.ShortPageInfo();

			retVal.Name = item.Name;
			retVal.ModifiedDate = item.LastModified;
			retVal.Language = item.Language;

			return retVal;
		}
	}
}