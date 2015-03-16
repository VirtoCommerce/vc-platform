//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VirtoCommerce.Content.Pages.Data.Models;
//using VirtoCommerce.Content.Pages.Data.Repositories;
//using VirtoCommerce.Content.Pages.Data.Services;
//using Xunit;

//namespace VirtoCommerce.Content.Tests.PagesTests
//{
//	public class PagesServiceTests
//	{
//		private PagesServiceImpl GetServiceWithGitHubRepository()
//		{
//			var githubMainPath = "Pages/";

//			var repository = new GitHubPagesRepositoryImpl(
//				"EugeneOkhriemnko",
//				"MfZUbM2wSDCdDADBEGpo",
//				"Site-Theme",
//				"EugeneOkhriemnko",
//				"Site_Themes",
//				githubMainPath);

//			var service = new PagesServiceImpl(repository);

//			return service;
//		}

//		[Fact]
//		public void GetPagesTest()
//		{
//			var service = GetServiceWithGitHubRepository();

//			var items = service.GetPages("Apple");

//			Assert.Equal(items.Count(), 2);
//			Assert.Equal(items.Count(s => s.Name == "about_us"), 1);
//		}

//		[Fact]
//		public void GetPage()
//		{
//			var service = GetServiceWithGitHubRepository();

//			var page = service.GetPage("Apple", "about_us");

//			Assert.True(page.Content.Contains("<li>Who you are</li>"));
//			Assert.Equal(page.Name, "about_us");
//		}

//		[Fact]
//		public void SaveAndDeletePageTest()
//		{
//			var service = GetServiceWithGitHubRepository();

//			var page = new Page();
//			page.Content = "Some new stuff";
//			page.Name = "new123";

//			service.SavePage("Apple", page);

//			var items = service.GetPages("Apple");
//			Assert.Equal(items.Count(), 3);

//			var item = service.GetPage("Apple", "new123");
//			Assert.True(item.Content.Contains("Some"));

//			page = new Page();
//			page.Content = "Some new stuff. Changes";
//			page.Name = "new123";

//			service.SavePage("Apple", page);

//			items = service.GetPages("Apple");
//			Assert.Equal(items.Count(), 3);

//			item = service.GetPage("Apple", "new123");
//			Assert.True(item.Content.Contains("Some") && item.Content.Contains("Changes"));

//			service.DeletePage("Apple", new string[] { "new123" });

//			items = service.GetPages("Apple");

//			Assert.Equal(items.Count(), 2);
//		}
//	}
//}
