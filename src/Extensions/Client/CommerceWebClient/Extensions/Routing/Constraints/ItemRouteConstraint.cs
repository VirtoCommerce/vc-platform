using System.Linq;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Client.Extensions.Routing.Constraints
{

    /// <summary>
    /// Route constraint checks if item exists in database
    /// </summary>
    public class ItemRouteConstraint : BaseRouteConstraint
    {

        protected override bool IsMatch(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!base.IsMatch(httpContext, route, parameterName, values, routeDirection))
            {
                return false;
            }

            if (routeDirection == RouteDirection.UrlGeneration)
            {
                return true;
            }

            var encoded = values[parameterName].ToString();
            var decoded = SettingsHelper.SeoDecode(encoded, SeoUrlKeywordTypes.Item, values.ContainsKey(Constants.Language) ? values[Constants.Language].ToString() : null);

            var item = CartHelper.CatalogClient.GetItem(decoded, StoreHelper.CustomerSession.CatalogId);

            if (item == null)
            {
                return false;
            }

            //Check if category is correct
            if (values.ContainsKey(Constants.Category))
            {
                encoded = values[Constants.Category].ToString();
                decoded = SettingsHelper.SeoDecode(encoded, SeoUrlKeywordTypes.Category, values.ContainsKey(Constants.Language) ? values[Constants.Language].ToString() : null);

                //Todo mark valid outline somehow
                return item.GetItemCategoryBrowsingOutlines().Any(outline => ValidateCategoryPath(outline, decoded));
            }

            return true;
        }
    }
}
