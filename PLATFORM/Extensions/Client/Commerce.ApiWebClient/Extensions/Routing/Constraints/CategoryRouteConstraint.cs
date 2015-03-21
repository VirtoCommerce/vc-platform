using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.ApiWebClient.Helpers;

namespace VirtoCommerce.ApiWebClient.Extensions.Routing.Constraints
{
    using VirtoCommerce.ApiClient;
    using VirtoCommerce.ApiClient.Extensions;

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

            var categoryPath = values[parameterName].ToString();


            var childCategorySlug = categoryPath.Split(Separator.ToCharArray()).Last();
            var session = ClientContext.Session;
            var language = values.ContainsKey(Constants.Language) ? values[Constants.Language].ToString() : session.Language;

            var client = ClientContext.Clients.CreateBrowseClient(session.StoreId, language);
            var category = Task.Run(() => client.GetCategoryAsync(childCategorySlug)).Result;

            if (category == null)
            {
                return false;
            }

            return ValidateCategoryPath(category.BuildOutline(language), categoryPath);
        }

    }
}
