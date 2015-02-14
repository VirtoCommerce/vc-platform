using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using Xunit;

namespace VirtoCommerce.Content.Tests
{
	public class DatabaseRepositoryTest
	{
		private DatabaseFileRepositoryImpl GetRepository()
		{
			var repository = new DatabaseFileRepositoryImpl("VirtoCommerce", new AuditableInterceptor(),
															   new EntityPrimaryKeyGeneratorInterceptor());

			return repository;
		}

		//Uncomment this test and run it first and 1 time
		//[Fact]
		//public void FirstTimeTestsInitialize()
		//{
		//	var fullPath = string.Format("{0}\\Themes\\", Environment.CurrentDirectory.Replace("\\bin\\Debug", string.Empty));

		//	var fileSystemFileRepository = new FileSystemFileRepositoryImpl(fullPath);
		//	var item = fileSystemFileRepository.GetContentItem("Apple/Simple/layout/theme.liquid");

		//	var repository = GetRepository();

		//	var contentItem = new ContentItem
		//	{
		//		Id = Guid.NewGuid().ToString(),
		//		ContentType = ContentType.Directory,
		//		Path = "Apple",
		//		Name = "Apple",
		//		CreatedDate = DateTime.UtcNow
		//	};

		//	repository.Add(contentItem);
		//	repository.UnitOfWork.Commit();

		//	var themeOne = new ContentItem
		//	{
		//		Id = Guid.NewGuid().ToString(),
		//		ContentType = ContentType.Directory,
		//		Path = "Apple/Simple",
		//		Name = "Simple",
		//		CreatedDate = DateTime.UtcNow
		//	};

		//	var themeTwo = new ContentItem
		//	{
		//		Id = Guid.NewGuid().ToString(),
		//		ContentType = ContentType.Directory,
		//		Path = "Apple/Timber",
		//		Name = "Timber",
		//		CreatedDate = DateTime.UtcNow
		//	};

		//	contentItem.ChildContentItems.Add(themeOne);
		//	contentItem.ChildContentItems.Add(themeTwo);
		//	repository.Update(contentItem);
		//	repository.UnitOfWork.Commit();

		//	var themeOneInnerFolder1 = new ContentItem
		//	{
		//		Id = Guid.NewGuid().ToString(),
		//		ContentType = ContentType.Directory,
		//		Path = "Apple/Simple/layout",
		//		Name = "layout",
		//		CreatedDate = DateTime.UtcNow
		//	};

		//	var themeOneInnerFolder2 = new ContentItem
		//	{
		//		Id = Guid.NewGuid().ToString(),
		//		ContentType = ContentType.Directory,
		//		Path = "Apple/Simple/templates",
		//		Name = "templates",
		//		CreatedDate = DateTime.UtcNow
		//	};

		//	themeOne.ChildContentItems.Add(themeOneInnerFolder1);
		//	themeOne.ChildContentItems.Add(themeOneInnerFolder2);
		//	repository.Update(themeOne);
		//	repository.UnitOfWork.Commit();

		//	var themeOneInnerFolder1InnerFile = new ContentItem
		//	{
		//		Id = Guid.NewGuid().ToString(),
		//		ContentType = ContentType.File,
		//		Path = "Apple/Simple/layout/theme.liquid",
		//		Name = "theme.liquid",
		//		CreatedDate = DateTime.UtcNow,
		//		Content = item.Content
		//	};

		//	themeOneInnerFolder1.ChildContentItems.Add(themeOneInnerFolder1InnerFile);
		//	repository.Update(themeOneInnerFolder1);
		//	repository.UnitOfWork.Commit();

		//	var themeOneInnerFolder2InnerFolder = new ContentItem
		//	{
		//		Id = Guid.NewGuid().ToString(),
		//		ContentType = ContentType.Directory,
		//		Path = "Apple/Simple/templates/customer",
		//		Name = "customer",
		//		CreatedDate = DateTime.UtcNow
		//	};

		//	var themeOneInnerFolder2InnerFile = new ContentItem
		//	{
		//		Id = Guid.NewGuid().ToString(),
		//		ContentType = ContentType.File,
		//		Path = "Apple/Simple/templates/404.liquid",
		//		Name = "404.liquid",
		//		CreatedDate = DateTime.UtcNow,
		//		Content = item.Content
		//	};

		//	themeOneInnerFolder2.ChildContentItems.Add(themeOneInnerFolder2InnerFolder);
		//	themeOneInnerFolder2.ChildContentItems.Add(themeOneInnerFolder2InnerFile);
		//	repository.Update(themeOneInnerFolder2);
		//	repository.UnitOfWork.Commit();
		//}

		[Fact]
		public void GetRootCollectionRepositoryTest()
		{
			var repository = GetRepository();

			var items = repository.GetContentItems("Apple");

			Assert.Equal(items.Length, 2);
			Assert.Equal(items.Count(i => i.Path == "Apple/Simple"), 1);
		}

		[Fact]
		public void GetCollectionRepositoryTest()
		{
			var repository = GetRepository();

			var items = repository.GetContentItems("Apple/Simple/templates");

			Assert.Equal(items.Length, 2);
			Assert.Equal(items.Count(i => i.Path == "Apple/Simple/templates/customer"), 1);
		}

		[Fact]
		public void GetItemRepositoryTest()
		{
			var repository = GetRepository();

			var item = repository.GetContentItem("Apple/Simple/layout/theme.liquid");

			Assert.Equal(item.Path, "Apple/Simple/layout/theme.liquid");
			Assert.True(item.Content.Contains("<!DO"));
		}

		[Fact]
		public void SaveAndDeleteItemRepositoryTest()
		{
			var repository = GetRepository();

			var content = new ContentItem();
			content.Content = "Some new stuff";
			content.Path = "Apple/Simple/new/new123.liquid";
			content.CreatedDate = DateTime.UtcNow;
			content.Id = Guid.NewGuid().ToString();
			content.Name = "new123.liquid";

			repository.SaveContentItem(content);

			var items = repository.GetContentItems("Apple/Simple/new");

			Assert.Equal(items.Length, 1);

			var item = repository.GetContentItem("Apple/Simple/new/new123.liquid");

			Assert.True(item.Content.Contains("Some"));

			content = new ContentItem();
			content.Content = "Some new stuff. Changes";
			content.Path = "Apple/Simple/new/new123.liquid";

			repository.SaveContentItem(content);

			items = repository.GetContentItems("Apple/Simple/new");

			Assert.Equal(items.Length, 1);

			item = repository.GetContentItem("Apple/Simple/new/new123.liquid");

			Assert.True(item.Content.Contains("Some") && item.Content.Contains("Changes"));

			content = new ContentItem();
			content.Path = "Apple/Simple/new/new123.liquid";

			repository.DeleteContentItem(content);

			items = repository.GetContentItems("Apple/Simple/new");

			Assert.Equal(items.Length, 0);
		}
	}
}
