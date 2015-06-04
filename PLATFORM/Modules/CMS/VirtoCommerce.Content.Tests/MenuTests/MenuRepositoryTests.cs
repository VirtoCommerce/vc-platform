//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VirtoCommerce.Content.Menu.Data.Models;
//using VirtoCommerce.Content.Menu.Data.Repositories;
//using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
//using Xunit;

//namespace VirtoCommerce.Content.Tests.MenuTests
//{
//	public class MenuRepositoryTests
//	{
//		private DatabaseMenuRepositoryImpl GetRepository()
//		{
//			var repository = new DatabaseMenuRepositoryImpl("VirtoCommerce", new AuditableInterceptor(),
//															   new EntityPrimaryKeyGeneratorInterceptor());

//			return repository;
//		}

//		//[Fact]
//		//public void InitializeTest()
//		//{
//		//	var repository = GetRepository();

//		//	var list = new MenuLinkList
//		//	{
//		//		Id = "a7b76a68-c00b-4c23-b77e-a021742c7f16",
//		//		StoreId = "Apple",
//		//		Name = "First_List"
//		//	};

//		//	list.MenuLinks.Add(new MenuLink
//		//	{
//		//		Id = Guid.NewGuid().ToString(),
//		//		Name = "First_Link",
//		//		Link = "http://virtocommerce.com/"
//		//	});
//		//	list.MenuLinks.Add(new MenuLink
//		//	{
//		//		Id = Guid.NewGuid().ToString(),
//		//		Name = "Second_Link",
//		//		Link = "http://virtocommerce.com/"
//		//	});

//		//	var list2 = new MenuLinkList
//		//	{
//		//		Id = "2d261f7d-d644-4beb-b078-3ceed95930ce",
//		//		StoreId = "Apple",
//		//		Name = "Second_List"
//		//	};

//		//	list2.MenuLinks.Add(new MenuLink
//		//	{
//		//		Id = Guid.NewGuid().ToString(),
//		//		Name = "First_Link",
//		//		Link = "http://virtocommerce.com/"
//		//	});
//		//	list2.MenuLinks.Add(new MenuLink
//		//	{
//		//		Id = Guid.NewGuid().ToString(),
//		//		Name = "Second_Link",
//		//		Link = "http://virtocommerce.com/"
//		//	});

//		//	repository.UpdateList(list);
//		//	repository.UpdateList(list2);
//		//}

//		[Fact]
//		public void GetListsTest()
//		{
//			var repository = GetRepository();

//			var items = repository.GetListsByStoreId("Apple");

//			Assert.Equal(2, items.Count());
//		}

//		[Fact]
//		public void GetListTest()
//		{
//			var repository = GetRepository();

//			var item = repository.GetListById("a7b76a68-c00b-4c23-b77e-a021742c7f16");

//			Assert.Equal("First_List", item.Name);
//			Assert.Equal(2, item.MenuLinks.Count);
//		}

//		[Fact]
//		public void SaveAndDeleteTest()
//		{
//			var repository = GetRepository();

//			var list = new MenuLinkList
//			{
//				Id = Guid.NewGuid().ToString(),
//				Name = "Added_List",
//				StoreId = "Apple"
//			};

//			list.MenuLinks.Add(new MenuLink
//			{
//				Id = Guid.NewGuid().ToString(),
//				Url = "http://test.com/",
//				Title = "First_Added_Link"
//			});

//			repository.UpdateList(list);

//			list = repository.GetListById(list.Id);

//			Assert.Equal(1, list.MenuLinks.Count);
//			Assert.Equal("Added_List", list.Name);
//			Assert.Equal("First_Added_Link", list.MenuLinks.First().Title);

//			var items = repository.GetListsByStoreId("Apple");

//			Assert.Equal(3, items.Count());

//			list.MenuLinks.Add(new MenuLink
//			{
//				Id = Guid.NewGuid().ToString(),
//				Url = "http://test.com/",
//				Title = "Second_Added_Link"
//			});

//			repository.UpdateList(list);

//			list = repository.GetListById(list.Id);

//			Assert.Equal(2, list.MenuLinks.Count);
//			Assert.Equal("Added_List", list.Name);
//			Assert.Equal(1, list.MenuLinks.Count(l => l.Title == "Second_Added_Link"));

//			repository.DeleteList(list.Id);

//			items = repository.GetListsByStoreId("Apple");

//			Assert.Equal(2, items.Count());
//		}

//	}
//}
