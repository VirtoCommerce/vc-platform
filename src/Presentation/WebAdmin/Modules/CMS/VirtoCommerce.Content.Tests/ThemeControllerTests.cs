//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Http.Results;
//using VirtoCommerce.Content.Data.Repositories;
//using VirtoCommerce.Content.Data.Services;
//using VirtoCommerce.Framework.Web.Settings;
//using VirtoCommerce.ThemeModule.Web.Controllers.Api;
//using VirtoCommerce.ThemeModule.Web.Models;
//using Xunit;

//namespace VirtoCommerce.Content.Tests
//{
//	public class ThemeControllerTests
//	{
//		private ThemeController GetController()
//		{
//			Func<string, IThemeService> factory = (x) =>
//			{
//				switch (x)
//				{
//					default:
//						return new ThemeServiceImpl(
//							new GitHubFileRepositoryImpl(
//							"EugeneOkhriemnko",
//							"MfZUbM2wSDCdDADBEGpo",
//							"Site-Theme",
//							"EugeneOkhriemnko",
//							"Site_Themes",
//							"Themes/"));
//				}
//			};

//			var mock = new Mock<ISettingsManager>();
//			mock.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>())).Returns("Github");

//			var controller = new ThemeController(factory, mock.Object, null, null);

//			return controller;
//		}

//		[Fact]
//		public void GitHubRepositoryGetThemesControllerTest()
//		{
//			var controller = GetController();

//			var themesResult = controller.GetThemes("Apple");

//			Assert.IsType<OkNegotiatedContentResult<ThemeModule.Web.Models.Theme[]>>(themesResult);

//			var items = themesResult as OkNegotiatedContentResult<ThemeModule.Web.Models.Theme[]>;

//			Assert.Equal(items.Content.Count(), 2);
//			Assert.Equal(items.Content.ElementAt(0).Path, "Apple/Simple");
//			Assert.Equal(items.Content.ElementAt(0).Name, "Simple");
//		}

//		[Fact]
//		public void GitHubRepositoryGetItemsControllerTest()
//		{
//			var controller = GetController();

//			var itemsResult = controller.GetThemeAssets("Apple", "Simple");

//			Assert.IsType<OkNegotiatedContentResult<ThemeModule.Web.Models.ThemeAsset[]>>(itemsResult);

//			var items = itemsResult as OkNegotiatedContentResult<ThemeModule.Web.Models.ThemeAsset[]>;

//			Assert.Equal(items.Content.Count(), 69);
//			Assert.Equal(items.Content.ElementAt(0).Id, "assets/apple-touch-icon-114x114.png");
//		}

//		[Fact]
//		public void GitHubRepositoryGetItemControllerTest()
//		{
//			var controller = GetController();

//			var itemResult = controller.GetThemeAsset("layout/theme.liquid", "Apple", "Simple");

//			Assert.IsType<OkNegotiatedContentResult<ThemeModule.Web.Models.ThemeAsset>>(itemResult);

//			var item = itemResult as OkNegotiatedContentResult<ThemeModule.Web.Models.ThemeAsset>;

//			Assert.Equal(item.Content.Id, "layout/theme.liquid");
//			Assert.True(item.Content.Content.Contains("<!DO"));
//		}

//		[Fact]
//		public void GitHubRepositorySaveAndDeleteItemControllerTest()
//		{
//			var controller = GetController();

//			var asset = new ThemeAsset();
//			asset.Id = "new/new123.liquid";
//			asset.Content = "Some new stuff";

//			controller.SaveItem(asset, "Apple", "Simple");

//			var itemResult = controller.GetThemeAsset("new/new123.liquid", "Apple", "Simple");
//			Assert.IsType<OkNegotiatedContentResult<ThemeModule.Web.Models.ThemeAsset>>(itemResult);
//			var item = itemResult as OkNegotiatedContentResult<ThemeModule.Web.Models.ThemeAsset>;
//			Assert.Equal(item.Content.Id, "new/new123.liquid");
//			Assert.True(item.Content.Content.Contains("Some"));

//			asset = new ThemeAsset();
//			asset.Id = "new/new123.liquid";
//			asset.Content = "Some new stuff. Changes";

//			controller.SaveItem(asset, "Apple", "Simple");

//			itemResult = controller.GetThemeAsset("new/new123.liquid", "Apple", "Simple");
//			Assert.IsType<OkNegotiatedContentResult<ThemeModule.Web.Models.ThemeAsset>>(itemResult);
//			item = itemResult as OkNegotiatedContentResult<ThemeModule.Web.Models.ThemeAsset>;

//			var itemsResult = controller.GetThemeAssets("Apple", "Simple");
//			Assert.IsType<OkNegotiatedContentResult<ThemeModule.Web.Models.ThemeAsset[]>>(itemsResult);
//			var items = itemsResult as OkNegotiatedContentResult<ThemeModule.Web.Models.ThemeAsset[]>;
//			Assert.Equal(items.Content.Count(), 70);

//			controller.DeleteAssets("Apple", "Simple", new string[] { "new/new123.liquid" });

//			itemsResult = controller.GetThemeAssets("Apple", "Simple");
//			Assert.IsType<OkNegotiatedContentResult<ThemeModule.Web.Models.ThemeAsset[]>>(itemsResult);
//			items = itemsResult as OkNegotiatedContentResult<ThemeModule.Web.Models.ThemeAsset[]>;
//			Assert.Equal(items.Content.Count(), 69);
//		}

//	}
//}
