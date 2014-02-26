using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Customers;
using VirtoCommerce.Foundation.Customers.Services;

namespace VirtoCommerce.Web.Client.Extensions
{
    public static class ItemExtensions
    {
        public static ICatalogOutlineBuilder OutlineBuilder
        {
            get { return DependencyResolver.Current.GetService<ICatalogOutlineBuilder>(); }
        }


        public static ICustomerSession CustomerSession
        {
            get
            {
                var session = DependencyResolver.Current.GetService<ICustomerSessionService>();
                return session.CustomerSession;
            }
        }


        public static string GetItemCategoryRouteValue(this Item item)
        {
            var outline = HttpContext.Current.Items["browsingoutline_" + item.Code.ToLower()] as string;

            if (string.IsNullOrEmpty(outline))
            {
                var outlines = item.GetItemCategoryBrowsingOutlines();
                outline = outlines.Any() ? outlines.First(): "undefined";
            }

            return outline;
        }

        public static string[] GetItemCategoryBrowsingOutlines(this Item item)
        {
            var outline = HttpContext.Current.Items["browsingoutline_" + item.Code.ToLower()] as string;
            if (string.IsNullOrEmpty(outline))
            {
                var outlines = OutlineBuilder.BuildCategoryOutline(item.CatalogId, item.ItemId);
                HttpContext.Current.Items["browsingoutline_" + item.Code.ToLower()] = outline = String.Join(";", outlines.Select(m => new BrowsingOutline(m).ToString()));
            }
            return outline.Split(';');
        }
    }
}
