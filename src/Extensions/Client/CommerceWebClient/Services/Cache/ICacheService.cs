using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Web.Client.Services.Cache
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
        [OperationContract]
        void ClearOuputCache(string controller, string action);

        /// <summary>
        /// Clears the database cache.
        /// </summary>
        /// <param name="cachePrefix">The cache prefix. One of prefixes available from VirtoCommerce.Foundation.Constants ...CachePrefix</param>
        [OperationContract]
        void ClearDatabaseCache(string cachePrefix);

    }
}
