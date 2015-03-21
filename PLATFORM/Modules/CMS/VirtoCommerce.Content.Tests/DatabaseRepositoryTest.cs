//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VirtoCommerce.Content.Data.Models;
//using VirtoCommerce.Content.Data.Repositories;
//using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
//using Xunit;

//namespace VirtoCommerce.Content.Tests
//{
//	public class DatabaseRepositoryTest
//	{
//		private DatabaseFileRepositoryImpl GetRepository()
//		{
//			var repository = new DatabaseFileRepositoryImpl("VirtoCommerce", new AuditableInterceptor(),
//															   new EntityPrimaryKeyGeneratorInterceptor());

//			return repository;
//		}

//		//Uncomment this test and run it first and 1 time
//		//[Fact]
//		//public void FirstTimeThemeTestsInitialize()
//		//{
//		//	var repository = GetRepository();

//		//	var theme1 = new Theme
//		//	{
//		//		ThemePath = "Apple/Simple",
//		//		Name = "Simple",
//		//		Id = "Apple/Simple"
//		//	};

//		//	var theme2 = new Theme
//		//	{
//		//		ThemePath = "Apple/Timber",
//		//		Name = "Timber",
//		//		Id = "Apple/Timber"
//		//	};

//		//	repository.Add(theme1);
//		//	repository.Add(theme2);
//		//	repository.UnitOfWork.Commit();
//		//}

//		//[Fact]
//		//public void FirstTimeTestsInitialize()
//		//{
//		//	var fullPath = string.Format("{0}\\Themes\\", Environment.CurrentDirectory.Replace("\\bin\\Debug", string.Empty));

//		//	var fileSystemFileRepository = new FileSystemFileRepositoryImpl(fullPath);
//		//	var item = fileSystemFileRepository.GetContentItem("Apple/Simple/layout/theme.liquid");

//		//	var repository = GetRepository();

//		//	var file1 = new ContentItem
//		//	{
//		//		Id = Guid.NewGuid().ToString(),
//		//		Path = "Apple/Simple/layout/theme.liquid",
//		//		Name = "theme.liquid",
//		//		CreatedDate = DateTime.UtcNow,
//		//		Content = item.Content
//		//	};

//		//	repository.Add(file1);

//		//	var file2 = new ContentItem
//		//	{
//		//		Id = Guid.NewGuid().ToString(),
//		//		Path = "Apple/Simple/templates/404.liquid",
//		//		Name = "404.liquid",
//		//		CreatedDate = DateTime.UtcNow,
//		//		Content = item.Content
//		//	};

//		//	repository.Add(file2);

//		//	var file3 = new ContentItem
//		//	{
//		//		Id = Guid.NewGuid().ToString(),
//		//		Path = "Apple/Timber/templates/404.liquid",
//		//		Name = "404.liquid",
//		//		CreatedDate = DateTime.UtcNow,
//		//		Content = item.Content
//		//	};

//		//	repository.Add(file3);
//		//	repository.UnitOfWork.Commit();
//		//}

//		[Fact]
//		public void GetThemesRepositoryTest()
//		{
//			var repository = GetRepository();

//			var items = repository.GetThemes("Apple");

//			Assert.Equal(items.Count(), 2);
//			Assert.Equal(items.Count(i => i.ThemePath == "Apple/Simple"), 1);
//		}

//		[Fact]
//		public void GetContentItemsRepositoryTest()
//		{
//			var repository = GetRepository();

//			var items = repository.GetContentItems("Apple/Simple");

//			Assert.Equal(items.Count(), 2);
//			Assert.Equal(items.Count(i => i.Path == "Apple/Simple/templates/404.liquid"), 1);
//		}

//		[Fact]
//		public void GetItemRepositoryTest()
//		{
//			var repository = GetRepository();

//			var item = repository.GetContentItem("Apple/Simple/layout/theme.liquid");

//			Assert.Equal(item.Path, "Apple/Simple/layout/theme.liquid");
//			Assert.True(item.Content.Contains("<!DO"));
//		}

//		[Fact]
//		public void SaveAndDeleteItemRepositoryTest()
//		{
//			var repository = GetRepository();

//			var content = new ContentItem();
//			content.Content = "Some new stuff";
//			content.Path = "new/new123.liquid";
//			content.CreatedDate = DateTime.UtcNow;
//			content.Id = Guid.NewGuid().ToString();
//			content.Name = "new123.liquid";

//			var path = "Apple/Simple/new/new123.liquid";

//			repository.SaveContentItem(path, content);

//			var items = repository.GetContentItems("Apple/Simple");

//			Assert.Equal(items.Count(), 3);

//			var item = repository.GetContentItem("Apple/Simple/new/new123.liquid");

//			Assert.True(item.Content.Contains("Some"));

//			content = new ContentItem();
//			content.Content = "Some new stuff. Changes";
//			path = "Apple/Simple/new/new123.liquid";

//			repository.SaveContentItem(path, content);

//			items = repository.GetContentItems("Apple/Simple");

//			Assert.Equal(items.Count(), 3);

//			item = repository.GetContentItem("Apple/Simple/new/new123.liquid");

//			Assert.True(item.Content.Contains("Some") && item.Content.Contains("Changes"));

//			path = "Apple/Simple/new/new123.liquid";

//			repository.DeleteContentItem(path);

//			items = repository.GetContentItems("Apple/Simple");

//			Assert.Equal(items.Count(), 2);
//		}
//	}
//}
