using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.Utilities;
using VirtoCommerce.Web.Core.DataContracts.Store;

namespace VirtoCommerce.ApiClient
{
	public class StoreClient : BaseClient
	{
		protected class RelativePaths
		{
			public const string Stores = "stores";
		}

		/// <summary>
		/// Initializes a new instance of the StoreClient class.
		/// </summary>
		/// <param name="adminBaseEndpoint">Admin endpoint</param>
		/// <param name="appId">The API application ID.</param>
		/// <param name="secretKey">The API secret key.</param>
		public StoreClient(Uri adminBaseEndpoint, string appId, string secretKey)
			: base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
		{
		}

		/// <summary>
		/// Initializes a new instance of the StoreClient class.
		/// </summary>
		/// <param name="adminBaseEndpoint">Admin endpoint</param>
		/// <param name="handler"></param>
		public StoreClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
			: base(adminBaseEndpoint, handler)
		{

		}

		/// <summary>
		/// List items matching the given query
		/// </summary>
		public Task<Store[]> GetStoresAsync()
		{
			return GetAsync<Store[]>(CreateRequestUri(RelativePaths.Stores));
		}
	}
}
