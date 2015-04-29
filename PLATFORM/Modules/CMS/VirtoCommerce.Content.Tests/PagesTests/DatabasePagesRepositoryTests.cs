//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VirtoCommerce.Content.Pages.Data.Models;
//using VirtoCommerce.Content.Pages.Data.Repositories;
//using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
//using Xunit;

//namespace VirtoCommerce.Content.Tests.PagesTests
//{
//	public class DatabasePagesRepositoryTests
//	{
//		private string _path = Environment.CurrentDirectory.Replace("\\bin\\Debug", string.Empty);

//		private DatabasePagesRepositoryImpl GetRepository()
//		{
//			var repository = new DatabasePagesRepositoryImpl("VirtoCommerce", new AuditableInterceptor(),
//															   new EntityPrimaryKeyGeneratorInterceptor());

//			return repository;
//		}

//		//[Fact]
//		//public void FirstTimeTestsInitialize()
//		//{
//		//	var fullPath = string.Format("{0}\\Pages\\", Environment.CurrentDirectory.Replace("\\bin\\Debug", string.Empty));

//		//	var fileSystemFileRepository = new FileSystemPagesRepositoryImpl(fullPath);
//		//	var item = fileSystemFileRepository.GetPage("Apple/about_us.liquid");
//		//	var item1 = fileSystemFileRepository.GetPage("Apple/contact_us.liquid");

//		//	var repository = GetRepository();

//		//	var page = new Page
//		//	{
//		//		Id = Guid.NewGuid().ToString(),
//		//		Path = "Apple/about_us.liquid",
//		//		Name = "about_us",
//		//		CreatedDate = DateTime.UtcNow,
//		//		Content = item.Content
//		//	};

//		//	var page1 = new Page
//		//	{
//		//		Id = Guid.NewGuid().ToString(),
//		//		Path = "Apple/contact_us.liquid",
//		//		Name = "contact_us",
//		//		CreatedDate = DateTime.UtcNow,
//		//		Content = item1.Content
//		//	};

//		//	repository.SavePage(page.Path, page);
//		//	repository.SavePage(page1.Path, page1);
//		//}

//		[Fact]
//		public void GetPagesTest()
//		{
//			var repository = GetRepository();

//			var items = repository.GetPages("Apple");

//			Assert.Equal(items.Count(), 2);
//			Assert.Equal(items.ElementAt(0).Name, "contact_us");
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
//			page.Name = "new123";
//			page.Path = "Apple/new123.liquid";
//			var path = "Apple/new123.liquid";

//			repository.SavePage(path, page);

//			var items = repository.GetPages("Apple");
//			Assert.Equal(items.Count(), 3);

//			var item = repository.GetPage("Apple/new123.liquid");
//			Assert.True(item.Content.Contains("Some"));

//			page = new Page();
//			page.Content = "Some new stuff. Changes";
//			page.Name = "new123";
//			page.Path = "Apple/new123.liquid";
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
