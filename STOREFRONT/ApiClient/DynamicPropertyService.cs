using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts.DynamicProperties;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient
{
    public class DynamicPropertyServiceClient : BaseClient
    {
        #region Constructors and Destructors

        public DynamicPropertyServiceClient(Uri adminBaseEndpoint, string appId, string secretKey)
            : base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        public DynamicPropertyServiceClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        #endregion

        #region Public Methods and Operators

        public Task<DynamicProperty[]> GetDynamicPropertiesForTypeAsync(string typeName)
        {
            return GetAsync<DynamicProperty[]>( CreateRequestUri(string.Format(RelativePaths.GetTypeProperties, typeName)),
                useCache: false);
        }

        public Task<DynamicPropertyDictionaryItem[]> GetDynamicPropertyDictionaryItemsAsync(string typeName, string propertyId)
        {
            return GetAsync<DynamicPropertyDictionaryItem[]>(
                CreateRequestUri(string.Format(RelativePaths.GetDictionaryItems, typeName, propertyId)),
                useCache: false);
        }
        #endregion

        protected class RelativePaths
        {
            #region Constants

            public const string GetTypeProperties = "platform/dynamic/types/{0}/properties";
            public const string GetDictionaryItems = "platform/dynamic/types/{0}/properties/{1}/dictionaryitems";
            #endregion
        }
    }
}
