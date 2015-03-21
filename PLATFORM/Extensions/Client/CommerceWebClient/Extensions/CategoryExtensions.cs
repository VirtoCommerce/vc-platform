using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.Web.Client.Extensions
{
    public static class CategoryExtensions
    {

        public static CatalogClient CatalogClient
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CatalogClient>();
            }
        }
        public static string DisplayName(this Category category, string locale = "")
        {
            var retValue = category.Name;
            var title = CatalogClient.GetPropertyValueByName(category, "Title", true, locale);
            if (title != null)
            {
                retValue = title.ToString();
            }
            return retValue;
        }
    }
}
