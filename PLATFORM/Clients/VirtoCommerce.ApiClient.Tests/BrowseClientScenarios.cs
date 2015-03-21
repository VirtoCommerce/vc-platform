using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.MerchandisingModule.Models;
using Xunit;

namespace VirtoCommerce.ApiClient.Tests
{
	public class BrowseClientScenarios
	{
		readonly string ServerAddress;
		//readonly string TestStore;
		readonly string Token;

		public BrowseClientScenarios()
		{
			ServerAddress = "http://localhost:8084/";
			//TestStore = "samplestore";
			Token = "secret";
		}

		[Fact]
		public void Can_browse_catalogitems()
		{
			using (WebApp.Start<OwinStartup>(ServerAddress)) // base hosting address
			{
				var client = new BrowseClient(new Uri(ServerAddress), Token);
				var results = Task.Run(() => client.GetProductsAsync(new BrowseQuery() { Take = 10 })).Result;
				Assert.True(results.TotalCount > 0);
			}
		}

		[Fact]
		public void Can_add_catalogitem()
		{
			using (WebApp.Start<OwinStartup>(ServerAddress)) // base hosting address
			{
				var client = new BrowseClient(new Uri(ServerAddress), Token);
				var results = Task.Run(() => client.GetProductsAsync(new BrowseQuery() { Take = 10 })).Result;
				Assert.True(results.TotalCount > 0);
			}
		}

		[Fact]
		public void Can_update_product()
		{
			using (WebApp.Start<OwinStartup>(ServerAddress)) // base hosting address
			{
				var itemClient = new ItemsClient(new Uri(ServerAddress), Token);
				var product = new Product() { Id = "2323", Name = "new name" };
				//Task.Run(() => itemClient.UpdateProductAsync("castegoryId", product)).Wait();
				/*
				var client = new BrowseClient(new Uri(ServerAddress), Token);
				var results = Task.Run(() => client.GetItemsAsync(TestStore, new BrowseQuery() { Take = 10 })).Result;
				Assert.True(results.TotalCount > 0);
				var itemId = results.Items[0].Id;

				var itemClient = new ItemsClient(new Uri(ServerAddress), Token);
				var product = new Product() {Id = itemId, Name = "new name"};
				Task.Run(() => itemClient.UpdateProductAsync(product)).Wait();
				 * */
			}
		}
	}
}
