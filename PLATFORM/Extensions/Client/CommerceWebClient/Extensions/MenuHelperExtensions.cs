using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider.Web.Html;

namespace VirtoCommerce.Web.Client.Extensions
{
	public static class MenuHelperExtensions
	{
		[CLSCompliant(false)]
		public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, bool startFromCurrentNode, bool startingNodeInChildLevel, bool showStartingNode, string menu)
		{
			var meta = new Dictionary<string, object>();
			if (!String.IsNullOrEmpty(menu))
			{
				meta.Add("menu", menu);
			}

			var str = helper.Menu(templateName, helper.SiteMap.FindSiteMapNodeFromKey(menu), true, false, 1, meta);

			return str;
		}
	}
}