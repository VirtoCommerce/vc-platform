using System.Collections.Generic;
using System.Web.Routing;

namespace VirtoCommerce.ApiWebClient.Caching.Interfaces
{
    public interface IKeyBuilder
    {
        /// <summary>
        /// Implementations should build a cache key given <see cref="controllerName"/>.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns></returns>
        string BuildKey(string controllerName);

        /// <summary>
        /// Implementations should build a cache key given the <see cref="controllerName"/> and <see cref="actionName"/>.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        string BuildKey(string controllerName, string actionName);

        /// <summary>
        /// Builds a cache key given the <see cref="controllerName"/>, <see cref="actionName"/> and <see cref="routeValues"/>.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="routeValues">The route values.</param>
        string BuildKey(string controllerName, string actionName, RouteValueDictionary routeValues);

        /// <summary>
        /// Implementations should build a cache key fragment for given <see cref="routeValue"/>.
        /// </summary>
        /// <param name="routeValue">The route value to process.</param>
        string BuildKeyFragment(KeyValuePair<string, object> routeValue);
    }
}
