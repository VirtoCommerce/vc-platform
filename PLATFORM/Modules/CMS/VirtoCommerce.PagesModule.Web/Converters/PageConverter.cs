using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webModels = VirtoCommerce.PagesModule.Web.Models;
using coreModels = VirtoCommerce.Content.Pages.Data.Models;

namespace VirtoCommerce.PagesModule.Web.Converters
{
	public static class PageConverter
	{
		public static coreModels.Page ToCoreModel(this webModels.Page page)
		{
			var retVal = new coreModels.Page();

			retVal.Name = page.Name;
			retVal.Content = page.Content;
			retVal.Language = page.Language;

			return retVal;
		}

		public static webModels.Page ToWebModel(this coreModels.Page page)
		{
			var retVal = new webModels.Page();

			retVal.Name = page.Name;
			retVal.Content = page.Content;
			retVal.Language = page.Language;

			return retVal;
		}

	}
}