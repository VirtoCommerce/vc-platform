using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiWebClient.Helpers;

namespace VirtoCommerce.ApiWebClient.Extensions.Routing.Constraints
{

    /// <summary>
    /// Route constraint checks if category exists in database
    /// </summary>
    public class CategoryRouteConstraint : BaseRouteConstraint
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
            var decoded = SettingsHelper.SeoDecode(encoded, SeoUrlKeywordTypes.Category, 
                values.ContainsKey(Constants.Language) ? values[Constants.Language].ToString() : null);

            var childCategoryId = decoded.Split(Separator.ToCharArray()).Last();
            var session = StoreHelper.CustomerSession;
            var category = Task.Run(() => CatalogHelper.CatalogClient.GetCategoryAsync(childCategoryId, session.CatalogId, session.Language)).Result;

            if (category == null)
            {
                return false;
            }

            return ValidateCategoryPath(category.Outline, decoded);
        }

    }
}
