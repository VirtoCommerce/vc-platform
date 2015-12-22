using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using VirtoCommerce.Content.Data.Models;
using webModels = VirtoCommerce.Content.Web.Models;

namespace VirtoCommerce.Web.Utilities
{
	public static class PagesUtility
	{
		public static IEnumerable<webModels.Page> GetShortPageInfoFromString(string[] pageNameAndLanguages)
		{
			var retVal = new List<webModels.Page>();

			foreach (var pageNameAndLanguage in pageNameAndLanguages)
			{
				var addedItem = new webModels.Page();
				addedItem.Name = pageNameAndLanguage.Split('^')[1];
				addedItem.Language = pageNameAndLanguage.Split('^')[0];
				retVal.Add(addedItem);
			}

			return retVal;
		}
	}
}