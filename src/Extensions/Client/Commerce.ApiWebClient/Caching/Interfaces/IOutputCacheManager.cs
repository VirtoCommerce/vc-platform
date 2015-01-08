using System.Web.Routing;

namespace VirtoCommerce.ApiWebClient.Caching.Interfaces
{
    public interface IOutputCacheManager
    {
        /// <summary>
        /// Implementations should remove a single output cache entry for the specified controller and action.
        /// </summary>
        /// <param name="controllerName">The name of the controller that contains the action method.</param>
        /// <param name="actionName">The name of the controller action method.</param>
        void RemoveItem(string controllerName, string actionName);

        /// <summary>
        /// Implementations should remove a single output cache entry for the specified controller, action and parameters.
        /// </summary>
        /// <param name="controllerName">The name of the controller that contains the action method.</param>
        /// <param name="actionName">The name of the controller action method.</param>
        /// <param name="routeValues">An object that contains the parameters for a route.</param>
        void RemoveItem(string controllerName, string actionName, object routeValues);

        /// <summary>
        /// Implementations should remove a single output cache entry for the specified controller, action and parameters.
        /// </summary>
        /// <param name="controllerName">The name of the controller that contains the action method.</param>
        /// <param name="actionName">The name of the controller action method.</param>
        /// <param name="routeValues">A dictionary that contains the parameters for a route.</param>
        void RemoveItem(string controllerName, string actionName, RouteValueDictionary routeValues);
        
        /// <summary>
        /// Implementations should remove all output cache entries.
        /// </summary>
        int RemoveItems();

        /// <summary>
        /// Implementations should remove all output cache entries for the specified controller.
        /// </summary>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns>Number of items removed</returns>
        int RemoveItems(string controllerName);

        /// <summary>
        /// Implementations should remove all output cache entries for the specified controller and action.
        /// </summary>
        /// <param name="controllerName">The name of the controller that contains the action method.</param>
        /// <param name="actionName">The name of the controller action method.</param>
        /// <returns>Number of items removed</returns>
        int RemoveItems(string controllerName, string actionName);

        /// <summary>
        /// Implementations should remove all output cache entries for the specified controller, action and parameters.
        /// </summary>
        /// <param name="controllerName">The name of the controller that contains the action method.</param>
        /// <param name="actionName">The name of the controller action method.</param>
        /// <param name="routeValues">A dictionary that contains the parameters for a route.</param>
        /// <returns>Number of items removed</returns>
        int RemoveItems(string controllerName, string actionName, RouteValueDictionary routeValues);
    }
}
