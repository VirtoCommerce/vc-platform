//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Http.Results;
//using VirtoCommerce.Content.Pages.Data.Repositories;
//using VirtoCommerce.Content.Pages.Data.Services;
//using VirtoCommerce.Platform.Core.Settings;
//using VirtoCommerce.PagesModule.Web.Controllers.Api;
//using VirtoCommerce.PagesModule.Web.Models;
//using Xunit;

//namespace VirtoCommerce.Content.Tests.PagesTests
//{
//	public class PagesControllerTests
//	{
//		private PagesController GetController()
//		{
//			Func<string, IPagesService> factory = (x) =>
//			{
//				switch (x)
//				{
//					default:
//						return new PagesServiceImpl(
//							new GitHubPagesRepositoryImpl(
//							"EugeneOkhriemnko",
//							"MfZUbM2wSDCdDADBEGpo",
//							"Site-Theme",
//							"EugeneOkhriemnko",
//							"Site_Themes",
//							"Pages/"));
//				}
//			};

//			var mock = new Mock<ISettingsManager>();
//			mock.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>())).Returns("Github");

//			var controller = new PagesController(factory, mock.Object);

//			return controller;
//		}

//		[Fact]
//		public void GetPagesTest()
//		{
//			var controller = GetController();

//			var pagesResult = controller.GetPages("Apple");

//			Assert.IsType<OkNegotiatedContentResult<IEnumerable<ShortPageInfo>>>(pagesResult);

//			var items = pagesResult as OkNegotiatedContentResult<IEnumerable<ShortPageInfo>>;

//			Assert.Equal(items.Content.Count(), 2);
//			Assert.Equal(items.Content.Count(s => s.Name == "about_us"), 1);
//		}

//		[Fact]
//		public void GetPageTest()
//		{
//			var controller = GetController();

//			var pageResult = controller.GetPage("Apple", "about_us");

//			Assert.IsType<OkNegotiatedContentResult<Page>>(pageResult);

//			var item = pageResult as OkNegotiatedContentResult<Page>;

//			Assert.True(item.Content.Content.Contains("<li>Who you are</li>"));
//			Assert.Equal(item.Content.Name, "about_us");
//		}

//		[Fact]
//		public void SaveAndDeletePageTest()
//		{
//			var controller = GetController();

//			var page = new Page();
//			page.Content = "Some new stuff";
//			page.Name = "new123";
//			controller.SaveItem("Apple", page);

//			var items = controller.GetPages("Apple") as OkNegotiatedContentResult<IEnumerable<ShortPageInfo>>;
//			Assert.Equal(items.Content.Count(), 3);

//			var item = controller.GetPage("Apple", "new123") as OkNegotiatedContentResult<Page>;
//			Assert.True(item.Content.Content.Contains("Some"));

//			page = new Page();
//			page.Content = "Some new stuff. Changes";
//			page.Name = "new123";

//			controller.SaveItem("Apple", page);

//			items = controller.GetPages("Apple") as OkNegotiatedContentResult<IEnumerable<ShortPageInfo>>;
//			Assert.Equal(items.Content.Count(), 3);

//			item = controller.GetPage("Apple", "new123") as OkNegotiatedContentResult<Page>;
//			Assert.True(item.Content.Content.Contains("Some") && item.Content.Content.Contains("Changes"));

//			controller.DeleteItem("Apple", new string[] { "new123" });

//			items = controller.GetPages("Apple") as OkNegotiatedContentResult<IEnumerable<ShortPageInfo>>;
//			Assert.Equal(items.Content.Count(), 2);
//		}
//	}
//}
