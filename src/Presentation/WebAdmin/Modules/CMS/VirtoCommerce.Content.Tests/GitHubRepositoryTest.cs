//namespace VirtoCommerce.Content.Tests
//{
//    #region

//    using System;
//    using System.Web.Http.Results;
//    using System.Linq;

//    using Moq;

//    using VirtoCommerce.Content.Data.Models;
//    using VirtoCommerce.Content.Data.Repositories;
//    using VirtoCommerce.Framework.Web.Settings;
//    using VirtoCommerce.ThemeModule.Web.Controllers.Api;

//    using Xunit;

//    #endregion

//    public class GitHubRepositoryTest
//    {
//        private string _githubMainPath = "Themes/";

//        #region Public Methods and Operators

//        //[Fact]
//        //public void GetCollectionControllerTest()
//        //{
//        //	var controller = this.GetController();

//        //	var result = controller.GetItems("/");

//        //	Assert.IsType<OkNegotiatedContentResult<ThemeModule.Web.Models.ContentItem[]>>(result);

//        //	var items = result as OkNegotiatedContentResult<ThemeModule.Web.Models.ContentItem[]>;

//        //	Assert.Equal(items.Content.Length, 5);
//        //}

//        [Fact]
//        public void GetRootCollectionRepositoryTest()
//        {
//            var repository = new GitHubFileRepositoryImpl(
//                "EugeneOkhriemnko",
//                "MfZUbM2wSDCdDADBEGpo",
//                "Site-Theme",
//                "EugeneOkhriemnko",
//                "Site_Themes",
//                _githubMainPath);

//            var items = repository.GetContentItems("Apple/", string.Empty);

//            Assert.Equal(items.Length, 2);
//            Assert.Equal(items[0].Path, "Simple");
//        }

//        [Fact]
//        public void GetCollectionRepositoryTest()
//        {
//            var repository = new GitHubFileRepositoryImpl(
//                "EugeneOkhriemnko",
//                "MfZUbM2wSDCdDADBEGpo",
//                "Site-Theme",
//                "EugeneOkhriemnko",
//                "Site_Themes",
//                _githubMainPath);

//            var items = repository.GetContentItems("Apple/Simple/", "templates");

//            Assert.Equal(items.Length, 13);
//            Assert.Equal(items[0].Path, "templates/404.liquid");
//        }

//        [Fact]
//        public void GetItemRepositoryTest()
//        {
//            var repository = new GitHubFileRepositoryImpl(
//                "EugeneOkhriemnko",
//                "MfZUbM2wSDCdDADBEGpo",
//                "Site-Theme",
//                "EugeneOkhriemnko",
//                "Site_Themes",
//                _githubMainPath);

//            var item = repository.GetContentItem("Apple/Simple/", "layout/theme.liquid");

//            Assert.Equal(item.Path, "layout/theme.liquid");
//            Assert.True(item.Content.Contains("<!DO"));
//        }

//        [Fact]
//        public void SaveAndDeleteItemRepositoryTest()
//        {
//            var repository = new GitHubFileRepositoryImpl(
//                "EugeneOkhriemnko",
//                "MfZUbM2wSDCdDADBEGpo",
//                "Site-Theme",
//                "EugeneOkhriemnko",
//                "Site_Themes",
//                _githubMainPath);

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

//            items = repository.GetContentItems("Apple/Simple/", string.Empty);

//            Assert.Equal(items.Where(i => i.Path.Contains("new")).Count(), 0);
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
//        //				return new GitHubFileRepositoryImpl(
//        //					"EugeneOkhriemnko",
//        //					"MfZUbM2wSDCdDADBEGpo",
//        //					"Site-Theme",
//        //					"EugeneOkhriemnko",
//        //					"Site_Themes",
//        //					_githubMainPath);
//        //		}
//        //	};

//        //	var mock = new Mock<ISettingsManager>();
//        //	mock.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>())).Returns("Github");

//        //	var controller = new ThemeController(factory, mock.Object);

//        //	return controller;
//        //}

//        //[Fact]
//        //public void GetItemControllerTest()
//        //{
//        //	var controller = this.GetController();

//        //	var result = controller.GetItem("/docs/new1.txt");

//        //	Assert.IsType<OkNegotiatedContentResult<ThemeModule.Web.Models.ContentItem>>(result);

//        //	var item = result as OkNegotiatedContentResult<ThemeModule.Web.Models.ContentItem>;

//        //	Assert.True(item.Content.Content.Contains("!!!\n"));
//        //}

//        #endregion
//    }
//}