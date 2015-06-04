using System.ServiceModel;

namespace VirtoCommerce.Foundation.AppConfig.Services
{
    [ServiceContract(Namespace = "http://schemas.virtocommerce.com/1.0/cache/")]
    public interface ICacheService
    {
        /// <summary>
        /// Clears the ouput cache. Optional controller and action.
        /// If both are null then all output cache will be cleared.
        /// If action is provided then controller must also be provided.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <returns>Number of items removed</returns>
        [OperationContract]
        int ClearOuputCache(string controller, string action);

        /// <summary>
        /// Clears the database cache.
        /// </summary>
        /// <param name="cachePrefix">The cache prefix. One of prefixes available from VirtoCommerce.Foundation.Constants ...CachePrefix</param>
        /// <returns>Number of items removed</returns>
        [OperationContract]
        int ClearDatabaseCache(string cachePrefix);

    }
}
