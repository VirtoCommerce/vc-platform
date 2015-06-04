using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using Xunit;

namespace VirtoCommerce.Content.Tests.PagesTests
{
	public class DatabasePagesTests
	{
		private DatabaseContentRepositoryImpl GetRepository()
		{
			var repository = new DatabaseContentRepositoryImpl("VirtoCommerce", new AuditableInterceptor(),
															   new EntityPrimaryKeyGeneratorInterceptor());

			return repository;
		}

		[Fact]
		public void MainTest()
		{
			var repository = GetRepository();

			//create pages
			repository.SavePage("Content_Test_Store/en-US/test_pages/test_page_1.html", new ContentPage
			{
				ByteContent = Encoding.UTF8.GetBytes("<a></a>"),
				ContentType = "text/html",
				CreatedDate = DateTime.UtcNow,
				Language = "en-US",
				Name = "test_page_1.html",
				Path = "Content_Test_Store/en-US/test_pages/test_page_1.html",
				Id = "Content_Test_Store/en-US/test_pages/test_page_1.html"
			});

			repository.SavePage("Content_Test_Store/en-US/test_pages/test_page_2.html", new ContentPage
			{
				ByteContent = Encoding.UTF8.GetBytes("<div></div>"),
				ContentType = "text/html",
				CreatedDate = DateTime.UtcNow,
				Language = "en-US",
				Name = "test_page_2.html",
				Path = "Content_Test_Store/en-US/test_pages/test_page_2.html",
				Id = "Content_Test_Store/en-US/test_pages/test_page_2.html"
			});

			//get page
			var page = repository.GetPage("Content_Test_Store/en-US/test_pages/test_page_1.html");
			Assert.NotNull(page);
			Assert.Equal(Encoding.UTF8.GetBytes("<a></a>"), page.ByteContent);
			Assert.Equal("test_page_1.html", page.Name);

			//get pages
			var pages = repository.GetPages("Content_Test_Store/", null);
			Assert.NotNull(pages);
			Assert.Equal(2, pages.Count());
			Assert.True(pages.First().Path.StartsWith("Content_Test_Store/en-US/test_pages/test_page_"));

			//delete pages
			repository.DeletePage("Content_Test_Store/en-US/test_pages/test_page_1.html");
			repository.DeletePage("Content_Test_Store/en-US/test_pages/test_page_2.html");
			pages = repository.GetPages("Content_Test_Store/", null);
			Assert.Equal(0, pages.ToArray().Length);
		}
	}
}
