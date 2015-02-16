using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts.Contents;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiClient.Utilities;
using VirtoCommerce.Web.Core.DataContracts;

namespace VirtoCommerce.ApiClient
{
	public class ContentClient : BaseClient
	{
		protected class RelativePaths
		{
			public const string Contents = "contents/{0}";
		}

		/// <summary>
		/// Initializes a new instance of the ContentClient class.
		/// </summary>
		/// <param name="adminBaseEndpoint">Admin endpoint</param>
		/// <param name="appId">The API application ID.</param>
		/// <param name="secretKey">The API secret key.</param>
		public ContentClient(Uri adminBaseEndpoint, string appId, string secretKey)
			: base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
		{
		}

		/// <summary>
		/// Initializes a new instance of the ContentClient class.
		/// </summary>
		/// <param name="adminBaseEndpoint">Admin endpoint</param>
		/// <param name="handler"></param>
		public ContentClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
			: base(adminBaseEndpoint, handler)
		{

		}

		/// <summary>
		/// List items matching the given query
		/// </summary>
		public Task<ResponseCollection<DynamicContentItemGroup>> GetDynamicContentAsync(string[] placeHolder, TagQuery query)
		{
			return GetAsync<ResponseCollection<DynamicContentItemGroup>>(CreateRequestUri(String.Format(RelativePaths.Contents, string.Join(",", placeHolder)), query.GetQueryString()));
		}
	}
}
