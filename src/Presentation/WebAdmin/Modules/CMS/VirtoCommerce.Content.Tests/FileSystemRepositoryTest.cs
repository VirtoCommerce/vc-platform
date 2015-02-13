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

	public class FileSystemRepositoryTest
    {
		private string _path = Environment.CurrentDirectory.Replace("\\bin\\Debug", string.Empty);

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
			var fullPath = string.Format("{0}\\Themes\\", _path);

			var repository = new FileSystemFileRepositoryImpl(fullPath);

            var items = repository.GetContentItems("Expo");

            Assert.Equal(items.Length, 6);
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
			var fullPath = string.Format("{0}\\Themes\\", _path);

			var repository = new FileSystemFileRepositoryImpl(fullPath);

			var item = repository.GetContentItem("Expo/layout/theme.liquid");

			Assert.Equal(item.Path, "Expo/layout/theme.liquid");
			Assert.True(item.Content.Contains("<!DO"));
		}

		[Fact]
		public void SaveAndDeleteItemRepositoryTest()
		{
			var fullPath = string.Format("{0}\\Themes\\", _path);

			var repository = new FileSystemFileRepositoryImpl(fullPath);

			var content = new ContentItem();
			content.Content = "Some new stuff";
			content.Path = "Expo/new/new123.liquid";

			repository.SaveContentItem(content);

			var items = repository.GetContentItems("Expo/new");

			Assert.Equal(items.Length, 1);

			var item = repository.GetContentItem("Expo/new/new123.liquid");

			Assert.True(item.Content.Contains("Some"));

			content = new ContentItem();
			content.Content = "Some new stuff. Changes";
			content.Path = "Expo/new/new123.liquid";

			repository.SaveContentItem(content);

			items = repository.GetContentItems("Expo/new");

			Assert.Equal(items.Length, 1);

			item = repository.GetContentItem("Expo/new/new123.liquid");

			Assert.True(item.Content.Contains("Some") && item.Content.Contains("Changes"));

			content = new ContentItem();
			content.Path = "Expo/new/new123.liquid";

			repository.DeleteContentItem(content);

			items = repository.GetContentItems("Expo/new");

			Assert.Equal(items.Length, 0);
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
		//				return new FileSystemFileRepositoryImpl(_path);
		//		}
		//	};

		//	var mock = new Mock<ISettingsManager>();
		//	mock.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>())).Returns("FileSystem");

		//	var controller = new ThemeController(factory, mock.Object);

		//	return controller;
		//}

        #endregion
    }
}