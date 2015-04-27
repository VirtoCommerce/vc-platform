using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webModels = VirtoCommerce.Content.Web.Models;
using coreModels = VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Web.Converters
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
			retVal.ModifiedDate = page.ModifiedDate.HasValue ? page.ModifiedDate.Value : page.CreatedDate;

			return retVal;
		}

	}
}