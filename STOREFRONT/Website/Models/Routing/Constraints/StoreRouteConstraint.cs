#region
using System.Web;
using System.Web.Routing;

#endregion

namespace VirtoCommerce.Web.Models.Routing.Constraints
{
    /// <summary>
    ///     Route constraint checks if store exists in database
    /// </summary>
    public class StoreRouteConstraint : BaseRouteConstraint
    {
        #region Methods
        protected override bool IsMatch(
            HttpContextBase httpContext,
            Route route,
            string parameterName,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            if (routeDirection == RouteDirection.UrlGeneration)
            {
                return true;
            }

            if (!base.IsMatch(httpContext, route, parameterName, values, routeDirection))
            {
                return false;
            }

            /* SKIP DATABASE CHECKS!
            var slug = values[parameterName].ToString();
            var language = values.ContainsKey(Constants.Language) ? values[Constants.Language].ToString() : null;
            var dbStore = SiteContext.Current.GetShopBySlug(slug, language);

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
             * */

            return true;
        }
        #endregion
    }
}