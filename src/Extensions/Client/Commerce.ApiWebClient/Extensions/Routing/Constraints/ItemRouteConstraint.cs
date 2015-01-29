using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.ApiWebClient.Helpers;

namespace VirtoCommerce.ApiWebClient.Extensions.Routing.Constraints
{
    using VirtoCommerce.ApiClient;
    using VirtoCommerce.ApiClient.Extensions;
    using VirtoCommerce.Web.Core.DataContracts;

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
            var session = ClientContext.Session;
            var language = values.ContainsKey(Constants.Language) ? values[Constants.Language].ToString() : session.Language;
            var productSlug = encoded;

            var client = ClientContext.Clients.CreateBrowseClient(session.StoreId, language);
            var item = Task.Run(() => client.GetProductAsync(productSlug, ItemResponseGroups.ItemMedium)).Result;

            if (item == null)
            {
                return false;
            }

            //Check if category is correct
            //if (values.ContainsKey(Constants.Category))
            //{
            //    encoded = values[Constants.Category].ToString();
            //    //var decoded = SettingsHelper.SeoDecode(encoded, SeoUrlKeywordTypes.Category, values.ContainsKey(Constants.Language) ? values[Constants.Language].ToString() : null);

            //    //Todo mark valid outline somehow
            //    return ValidateCategoryPath(item.Outline, decoded);
            //}

            return true;
        }
    }
}
