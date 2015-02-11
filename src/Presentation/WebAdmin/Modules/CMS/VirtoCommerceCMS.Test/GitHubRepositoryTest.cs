using Moq;
using System;
using System.Web.Http.Results;
using VirtoCommerce.Framework.Web.Settings;
using VirtoCommerceCMS.Data.Repositories;
using VirtoCommerceCMS.ThemeModule.Web.Controllers.Api;
using VirtoCommerceCMS.ThemeModule.Web.Models;
using Xunit;

namespace VirtoCommerceCMS.Test
{
	public class GitHubRepositoryTest
	{
		[Fact]
		public void GetCollectionRepositoryTest()
		{
			var repository = new GitHubFileRepositoryImpl(
								"EugeneOkhriemnko",
								"MfZUbM2wSDCdDADBEGpo",
								"Site-Theme",
								"EugeneOkhriemnko",
								"Site_Themes");

			var items = repository.GetContentItems("/");

			Assert.Equal(items.Length, 5);
		}

		[Fact]
		public void GetItemRepositoryTest()
		{
			var repository = new GitHubFileRepositoryImpl(
								"EugeneOkhriemnko",
								"MfZUbM2wSDCdDADBEGpo",
								"Site-Theme",
								"EugeneOkhriemnko",
								"Site_Themes");

			var item = repository.GetContentItem("/docs/new1.txt");

			Assert.Equal(item.Path, "docs/new1.txt");
			Assert.True(item.Content.Contains("!!!\n"));
		}

		[Fact]
		public void SaveAndDeleteItemRepositoryTest()
		{
			var repository = new GitHubFileRepositoryImpl(
								"EugeneOkhriemnko",
								"MfZUbM2wSDCdDADBEGpo",
								"Site-Theme",
								"EugeneOkhriemnko",
								"Site_Themes");

			var items = repository.GetContentItems("/docs/");

			Assert.Equal(items.Length, 1);

			var content = new VirtoCommerceCMS.Data.Models.ContentItem();
			content.Content = "Some new stuff";
			content.Path = "docs/some.txt";

			repository.SaveContentItem(content);

			items = repository.GetContentItems("/docs/");

			Assert.Equal(items.Length, 2);

			var item = repository.GetContentItem("/docs/some.txt");

			Assert.Equal(item.Content, "Some new stuff");

			repository.DeleteContentItem(content);

			items = repository.GetContentItems("/docs/");

			Assert.Equal(items.Length, 1);
		}

		[Fact]
		public void GetCollectionControllerTest()
		{
			var controller = GetController();

			var result = controller.GetItems("/");

			Assert.IsType<OkNegotiatedContentResult<VirtoCommerceCMS.ThemeModule.Web.Models.ContentItem[]>>(result);

			var items = result as OkNegotiatedContentResult<VirtoCommerceCMS.ThemeModule.Web.Models.ContentItem[]>;

			Assert.Equal(items.Content.Length, 5);
		}

		[Fact]
		public void GetItemControllerTest()
		{
			var controller = GetController();

			var result = controller.GetItem("/docs/new1.txt");

			Assert.IsType<OkNegotiatedContentResult<VirtoCommerceCMS.ThemeModule.Web.Models.ContentItem>>(result);

			var item = result as OkNegotiatedContentResult<VirtoCommerceCMS.ThemeModule.Web.Models.ContentItem>;

			Assert.True(item.Content.Content.Contains("!!!\n"));
		}

		private ThemeController GetController()
		{
			Func<string, IFileRepository> factory = (x) =>
				{
					switch(x)
					{
						default:
							return new GitHubFileRepositoryImpl(
								"EugeneOkhriemnko",
								"MfZUbM2wSDCdDADBEGpo",
								"Site-Theme",
								"EugeneOkhriemnko",
								"Site_Themes");
					}
				};

			var mock = new Mock<ISettingsManager>();
			mock.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>())).Returns("Github");

			var controller = new ThemeController(factory, mock.Object);

			return controller;
		}
	}
}
