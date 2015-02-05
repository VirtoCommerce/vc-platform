using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Client;
using VirtoCommerce.Client.Extensions;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Client.Extensions.Routing.Constraints
{

    /// <summary>
    /// Route constraint checks if store exists in database
    /// </summary>
    public class StoreRouteConstraint : BaseRouteConstraint
    {
        /// <summary>
        /// Gets the catalog client.
        /// </summary>
        /// <value>The catalog client.</value>
        private StoreClient StoreClient
        {
            get { return DependencyResolver.Current.GetService<StoreClient>(); }
        }

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

            var encoded = values[parameterName].ToString();
            var decoded = SettingsHelper.SeoDecode(encoded, SeoUrlKeywordTypes.Store, values.ContainsKey(Constants.Language) ? values[Constants.Language].ToString() : null);
            var dbStore = StoreClient.GetStoreById(decoded);

            if (dbStore == null)
            {
                return false;
            }

            if (values.ContainsKey(Constants.Language))
            {
                try
                {
                    var culture = values[Constants.Language].ToString().ToSpecificLangCode();
                    if (!dbStore.Languages.Any(l => l.LanguageCode.ToSpecificLangCode().Equals(culture, StringComparison.InvariantCultureIgnoreCase)))
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
