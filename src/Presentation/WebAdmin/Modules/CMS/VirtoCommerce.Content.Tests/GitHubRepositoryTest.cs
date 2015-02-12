namespace VirtoCommerce.Content.Tests
{
	#region

	using System;
	using System.Web.Http.Results;

	using Moq;

	using VirtoCommerce.Content.Data.Models;
	using VirtoCommerce.Content.Data.Repositories;
	using VirtoCommerce.Framework.Web.Settings;
	using VirtoCommerce.ThemeModule.Web.Controllers.Api;

	using Xunit;

	#endregion

	public class GitHubRepositoryTest
	{
		private string _githubMainPath = "/Themes/";

		#region Public Methods and Operators

		//[Fact]
		//public void GetCollectionControllerTest()
		//{
		//	var controller = this.GetController();

		//	var result = controller.GetItems("/");

		//	Assert.IsType<OkNegotiatedContentResult<ThemeModule.Web.Models.ContentItem[]>>(result);

		//	var items = result as OkNegotiatedContentResult<ThemeModule.Web.Models.ContentItem[]>;

		//	Assert.Equal(items.Content.Length, 5);
		//}

		[Fact]
		public void GetCollectionRepositoryTest()
		{
			var repository = new GitHubFileRepositoryImpl(
				"EugeneOkhriemnko",
				"MfZUbM2wSDCdDADBEGpo",
				"Site-Theme",
				"EugeneOkhriemnko",
				"Site_Themes",
				_githubMainPath);

			var items = repository.GetContentItems("/");

			Assert.Equal(items.Length, 5);
		}

		//[Fact]
		//public void GetItemControllerTest()
		//{
		//	var controller = this.GetController();

		//	var result = controller.GetItem("/docs/new1.txt");

		//	Assert.IsType<OkNegotiatedContentResult<ThemeModule.Web.Models.ContentItem>>(result);

		//	var item = result as OkNegotiatedContentResult<ThemeModule.Web.Models.ContentItem>;

		//	Assert.True(item.Content.Content.Contains("!!!\n"));
		//}

		[Fact]
		public void GetItemRepositoryTest()
		{
			var repository = new GitHubFileRepositoryImpl(
				"EugeneOkhriemnko",
				"MfZUbM2wSDCdDADBEGpo",
				"Site-Theme",
				"EugeneOkhriemnko",
				"Site_Themes",
				_githubMainPath);

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
				"Site_Themes",
				_githubMainPath);

			var items = repository.GetContentItems("/docs/");

			Assert.Equal(items.Length, 1);

			var content = new ContentItem();
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

		#endregion

		#region Methods

		//private ThemeController GetController()
		//{
		//	Func<string, IFileRepository> factory = (x) =>
		//	{
		//		switch (x)
		//		{
		//			default:
		//				return new GitHubFileRepositoryImpl(
		//					"EugeneOkhriemnko",
		//					"MfZUbM2wSDCdDADBEGpo",
		//					"Site-Theme",
		//					"EugeneOkhriemnko",
		//					"Site_Themes",
		//					_githubMainPath);
		//		}
		//	};

		//	var mock = new Mock<ISettingsManager>();
		//	mock.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>())).Returns("Github");

		//	var controller = new ThemeController(factory, mock.Object);

		//	return controller;
		//}

		#endregion
	}
}