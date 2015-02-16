using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.Utilities;
using VirtoCommerce.Web.Core.DataContracts;

namespace VirtoCommerce.ApiClient
{
	public class SeoClient : BaseClient
	{
		protected class RelativePaths
		{
			public const string Keywords = "keywords";
		}

		/// <summary>
		/// Initializes a new instance of the SeoClient class.
		/// </summary>
		/// <param name="adminBaseEndpoint">Admin endpoint</param>
		/// <param name="appId">The API application ID.</param>
		/// <param name="secretKey">The API secret key.</param>
		public SeoClient(Uri adminBaseEndpoint, string appId, string secretKey)
			: base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
		{
		}

		/// <summary>
		/// Initializes a new instance of the SeoClient class.
		/// </summary>
		/// <param name="adminBaseEndpoint">Admin endpoint</param>
		/// <param name="handler"></param>
		public SeoClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
			: base(adminBaseEndpoint, handler)
		{

		}

		/// <summary>
		/// List items matching the given query
		/// </summary>
		public Task<SeoKeyword[]> GetKeywordsAsync()
		{
			return GetAsync<SeoKeyword[]>(CreateRequestUri(RelativePaths.Keywords));
		}
	}
}
