using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Client;
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

        public static CatalogClient CatalogClient
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CatalogClient>();
            }
        }

        public static string GetItemCategoryRouteValue(this Item item)
        {
            var outline = HttpContext.Current.Items["browsingoutline_" + item.ItemId.ToLower()] as string;

            if (string.IsNullOrEmpty(outline))
            {
                var outlines = item.GetItemCategoryBrowsingOutlines();
                outline = outlines.Any() ? outlines.First(): "undefined";
            }
            else
            {
                outline = outline.Split(';').First();
            }

            return outline;
        }

        public static string[] GetItemCategoryBrowsingOutlines(this Item item)
        {
            var outline = HttpContext.Current.Items["browsingoutline_" + item.ItemId.ToLower()] as string;
            if (string.IsNullOrEmpty(outline))
            {
                var outlines = OutlineBuilder.BuildCategoryOutline(CustomerSession.CatalogId, item.ItemId);
                HttpContext.Current.Items["browsingoutline_" + item.ItemId.ToLower()] = outline = String.Join(";", outlines.Select(m => new BrowsingOutline(m).ToString()));
            }
            return outline.Split(';');
        }

        public static string DisplayName(this Item item, string defaultValue = "", string locale="")
        {
            var retValue = string.IsNullOrEmpty(defaultValue) ? item.Name : defaultValue;
            var title = CatalogClient.GetPropertyValueByName(item, "Title", locale:locale);
            if (title != null)
            {
                retValue = title.ToString();
            }
            return retValue;
        }
    }
}
