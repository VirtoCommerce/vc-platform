//namespace VirtoCommerce.Content.Tests
//{
//    #region

//    using System;
//    using System.Web.Http.Results;

//    using Moq;

//    using VirtoCommerce.Content.Data.Models;
//    using VirtoCommerce.Content.Data.Repositories;
//    using VirtoCommerce.Framework.Web.Settings;
//    using VirtoCommerce.ThemeModule.Web.Controllers.Api;

//    using Xunit;

//    #endregion

//    public class FileSystemRepositoryTest
//    {
//        private string _path = Environment.CurrentDirectory.Replace("\\bin\\Debug", string.Empty);

//        #region Public Methods and Operators

//        [Fact]
//        public void GetRootCollectionRepositoryTest()
//        {
//            var fullPath = string.Format("{0}\\Themes\\", _path);

//            var repository = new FileSystemFileRepositoryImpl(fullPath);

//            var items = repository.GetContentItems("Apple/", "");

//            Assert.Equal(items.Length, 2);
//            Assert.Equal(items[0].Path, "Simple");
//        }

//        [Fact]
//        public void GetCollectionRepositoryTest()
//        {
//            var fullPath = string.Format("{0}\\Themes\\", _path);

//            var repository = new FileSystemFileRepositoryImpl(fullPath);

//            var items = repository.GetContentItems("Apple/Simple/", "templates");

//            Assert.Equal(items.Length, 13);
//            Assert.Equal(items[0].Path, "templates/customers");
//        }

//        [Fact]
//        public void GetItemRepositoryTest()
//        {
//            var fullPath = string.Format("{0}\\Themes\\", _path);

//            var repository = new FileSystemFileRepositoryImpl(fullPath);

//            var item = repository.GetContentItem("Apple/Simple/", "layout/theme.liquid");

//            Assert.Equal(item.Path, "layout/theme.liquid");
//            Assert.True(item.Content.Contains("<!DO"));
//        }

//        [Fact]
//        public void SaveAndDeleteItemRepositoryTest()
//        {
//            var fullPath = string.Format("{0}\\Themes\\", _path);

//            var repository = new FileSystemFileRepositoryImpl(fullPath);

//            var content = new ContentItem();
//            content.Content = "Some new stuff";
//            content.Path = "new/new123.liquid";

//            repository.SaveContentItem("Apple/Simple/", content);

//            var items = repository.GetContentItems("Apple/Simple/", "new");

//            Assert.Equal(items.Length, 1);

//            var item = repository.GetContentItem("Apple/Simple/", "new/new123.liquid");

//            Assert.True(item.Content.Contains("Some"));

//            content = new ContentItem();
//            content.Content = "Some new stuff. Changes";
//            content.Path = "new/new123.liquid";

//            repository.SaveContentItem("Apple/Simple/", content);

//            items = repository.GetContentItems("Apple/Simple/", "new");

//            Assert.Equal(items.Length, 1);

//            item = repository.GetContentItem("Apple/Simple/", "new/new123.liquid");

//            Assert.True(item.Content.Contains("Some") && item.Content.Contains("Changes"));

//            content = new ContentItem();
//            content.Path = "new/new123.liquid";

//            repository.DeleteContentItem("Apple/Simple/", content);

//            items = repository.GetContentItems("Apple/Simple/", "new");

//            Assert.Equal(items.Length, 0);
//        }

//        #endregion

//        #region Methods

//        //private ThemeController GetController()
//        //{
//        //	Func<string, IFileRepository> factory = (x) =>
//        //	{
//        //		switch (x)
//        //		{
//        //			default:
//        //				return new FileSystemFileRepositoryImpl(_path);
//        //		}
//        //	};

//        //	var mock = new Mock<ISettingsManager>();
//        //	mock.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>())).Returns("FileSystem");

//        //	var controller = new ThemeController(factory, mock.Object);

//        //	return controller;
//        //}

//        #endregion
//    }
//}