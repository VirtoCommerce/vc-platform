#region

using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiClient.Utilities;

#endregion

namespace VirtoCommerce.ApiClient
{
    public class PriceClient : BaseClient
    {
        #region Constructors and Destructors

        public PriceClient(Uri adminBaseEndpoint, string appId, string secretKey)
            : base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        public PriceClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        #endregion

        #region Public Methods and Operators

        public Task<string[]> GetPriceListsAsync(
            string catalog,
            string currency,
            TagQuery query)
        {
            var parameters = new { catalog, currency };
            return
                GetAsync<string[]>(
                    CreateRequestUri(
                        String.Format(RelativePaths.PriceLists),
                        query.GetQueryString(parameters)));
        }

        public Task<Price[]> GetPrices(
            string[] priceLists,
            string[] products,
            bool useCache = true)
        {
            var parameters = new { priceLists = string.Join(",", priceLists), products = string.Join(",", products) };
            return
                GetAsync<Price[]>(
                    CreateRequestUri(
                        String.Format(RelativePaths.Prices),
                        parameters),
                    useCache: useCache);
        }

        #endregion

        protected class RelativePaths
        {
            #region Constants

            public const string PriceLists = "mp/pricelists";
            public const string Prices = "mp/prices";

            #endregion
        }
    }
}
