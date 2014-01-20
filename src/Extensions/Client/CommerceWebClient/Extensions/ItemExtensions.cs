using System.Linq;
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
            var outlines = OutlineBuilder.BuildCategoryOutline(CustomerSession.CatalogId, item.ItemId);
            if (outlines.Outlines.Count > 0)
            {
                return outlines.Outlines[0].Categories.OfType<Category>().Last().Code;
            }

            return "undefined";
        }
    }
}
