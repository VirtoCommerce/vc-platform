//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VirtoCommerce.Content.Pages.Data.Models;
//using VirtoCommerce.Content.Pages.Data.Repositories;
//using Xunit;

//namespace VirtoCommerce.Content.Tests.PagesTests
//{
//	public class GitHubPagesRepositoryTests
//	{
//		private string _githubMainPath = "Pages/";

//		private GitHubPagesRepositoryImpl GetRepository()
//		{
//			var repository = new GitHubPagesRepositoryImpl(
//				"EugeneOkhriemnko",
//				"MfZUbM2wSDCdDADBEGpo",
//				"Site-Theme",
//				"EugeneOkhriemnko",
//				"Site_Themes",
//				_githubMainPath);

//			return repository;
//		}

//		[Fact]
//		public void GetPagesTest()
//		{
//			var repository = GetRepository();

//			var items = repository.GetPages("Apple");

//			Assert.Equal(items.Count(), 2);
//			Assert.Equal(items.Count(s => s.Name == "about_us"), 1);
//		}

//		[Fact]
//		public void GetPageTest()
//		{
//			var repository = GetRepository();

//			var item = repository.GetPage("Apple/about_us.liquid");

//			Assert.True(item.Content.Contains("<li>Who you are</li>"));
//			Assert.Equal(item.Name, "about_us");
//		}

//		[Fact]
//		public void SaveAndDeletePageTest()
//		{
//			var repository = GetRepository();

//			var page = new Page();
//			page.Content = "Some new stuff";
//			var path = "Apple/new123.liquid";

//			repository.SavePage(path, page);

//			var items = repository.GetPages("Apple");
//			Assert.Equal(items.Count(), 3);

//			var item = repository.GetPage("Apple/new123.liquid");
//			Assert.True(item.Content.Contains("Some"));

//			page = new Page();
//			page.Content = "Some new stuff. Changes";
//			path = "Apple/new123.liquid";

//			repository.SavePage(path, page);

//			items = repository.GetPages("Apple");
//			Assert.Equal(items.Count(), 3);

//			item = repository.GetPage("Apple/new123.liquid");
//			Assert.True(item.Content.Contains("Some") && item.Content.Contains("Changes"));

//			path = "Apple/new123.liquid";

//			repository.DeletePage(path);

//			items = repository.GetPages("Apple");

//			Assert.Equal(items.Count(), 2);
//		}
//	}
//}
