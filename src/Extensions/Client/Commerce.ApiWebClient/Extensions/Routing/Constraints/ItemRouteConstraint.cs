using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiWebClient.Helpers;

namespace VirtoCommerce.ApiWebClient.Extensions.Routing.Constraints
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
            var productId = decoded;
            var session = StoreHelper.CustomerSession;
            var item = Task.Run(() => CatalogHelper.CatalogClient.GetItemAsync(productId, session.CatalogId, session.Language)).Result;

            if (item == null)
            {
                return false;
            }

            //Check if category is correct
            if (values.ContainsKey(Constants.Category) && !string.IsNullOrEmpty(item.Outline))
            {
                encoded = values[Constants.Category].ToString();
                decoded = SettingsHelper.SeoDecode(encoded, SeoUrlKeywordTypes.Category, values.ContainsKey(Constants.Language) ? values[Constants.Language].ToString() : null);

                //Todo mark valid outline somehow
                return ValidateCategoryPath(item.Outline, decoded);
            }

            return true;
        }
    }
}
