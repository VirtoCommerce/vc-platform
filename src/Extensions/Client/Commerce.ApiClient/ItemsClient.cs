using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.Utilities;
using VirtoCommerce.Web.Core.DataContracts;

namespace VirtoCommerce.ApiClient
{
	public class ItemsClient : BaseClient
	{
		protected class RelativePaths
		{
			public const string AddProduct = "products/{0}";
			public const string UpdateProduct = "products/{0}";
			public const string DeleteProduct = "products/{0}";
		}

		/// <summary>
		/// Initializes a new instance of the ItemsClient class.
		/// </summary>
		/// <param name="adminBaseEndpoint">Admin endpoint</param>
		/// <param name="appId">The API application ID.</param>
		/// <param name="secretKey">The API secret key.</param>
		public ItemsClient(Uri adminBaseEndpoint, string appId, string secretKey)
			: base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
		{
		}

		/// <summary>
		/// Initializes a new instance of the ItemsClient class.
		/// </summary>
		/// <param name="adminBaseEndpoint">Admin endpoint</param>
		/// <param name="handler"></param>
		public ItemsClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
			: base(adminBaseEndpoint, handler)
		{

		}

		/// <summary>
		/// List items matching the given query
		/// </summary>
		public Task AddAsync(string categoryId, Product product)
		{
			var requestUri = CreateRequestUri(String.Format(RelativePaths.AddProduct, categoryId));
			return SendAsync(requestUri, HttpMethod.Post, product);
		}

		public Task UpdateAsync(string categoryId, Product product)
		{
			var requestUri = CreateRequestUri(String.Format(RelativePaths.UpdateProduct, categoryId));
			return SendAsync(requestUri, new HttpMethod("PATCH"), product);
		}

		public Task DeleteAsync(string productId)
		{
			var requestUri = CreateRequestUri(String.Format(RelativePaths.DeleteProduct, productId));
			return SendAsync(requestUri, HttpMethod.Delete);
		}
	}
}
