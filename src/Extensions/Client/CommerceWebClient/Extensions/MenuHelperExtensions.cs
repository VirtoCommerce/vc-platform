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
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, bool startFromCurrentNode, bool startingNodeInChildLevel, bool showStartingNode, string menu)
        {
            if (!String.IsNullOrEmpty(menu))
            {
                if (HttpContext.Current.Items.Contains("menu"))
                    HttpContext.Current.Items["menu"] = menu;
                else
                    HttpContext.Current.Items.Add("menu", menu);
            }

            MvcHtmlString str = helper.Menu(templateName, startFromCurrentNode, startingNodeInChildLevel, showStartingNode);

            if (!String.IsNullOrEmpty(menu))
                HttpContext.Current.Items.Remove("menu");

            return str;
        }
    }
}