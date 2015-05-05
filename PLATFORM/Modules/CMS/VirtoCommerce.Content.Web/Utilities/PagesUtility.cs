using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Web.Utilities
{
	public static class PagesUtility
	{
		public static IEnumerable<Page> GetShortPageInfoFromString(string[] pageNameAndLanguages)
		{
			var retVal = new List<Page>();

			foreach (var pageNameAndLanguage in pageNameAndLanguages)
			{
				var addedItem = new Page();
				addedItem.PageName = pageNameAndLanguage.Split('^')[1];
				addedItem.Language = pageNameAndLanguage.Split('^')[0];
				retVal.Add(addedItem);
			}

			return retVal;
		}
	}
}