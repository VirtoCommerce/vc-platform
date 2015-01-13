using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.ApiWebClient.Helpers;

namespace VirtoCommerce.ApiWebClient.Extensions.Routing.Constraints
{

    /// <summary>
    /// Route constraint checks if store exists in database
    /// </summary>
    public class StoreRouteConstraint : BaseRouteConstraint
    {
        protected override bool IsMatch(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {

            if (routeDirection == RouteDirection.UrlGeneration)
            {
                return true;
            }

            if (!base.IsMatch(httpContext, route, parameterName, values, routeDirection))
            {
                return false;
            }

            var slug = values[parameterName].ToString();
            var language = values.ContainsKey(Constants.Language) ? values[Constants.Language].ToString() : null;
            var dbStore = StoreHelper.StoreClient.GetStore(slug, language);

            if (dbStore == null)
            {
                return false;
            }

            if (values.ContainsKey(Constants.Language))
            {
                try
                {
                    var culture = values[Constants.Language].ToString().ToSpecificLangCode();
                    if (!dbStore.Languages.Any(l => l.ToSpecificLangCode().Equals(culture, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        //Store does not support this language
                        return false;
                    }
                }
                catch
                {
                    //Language is not valid
                    return false;
                }
            }

            return true;
        }
    }
}
