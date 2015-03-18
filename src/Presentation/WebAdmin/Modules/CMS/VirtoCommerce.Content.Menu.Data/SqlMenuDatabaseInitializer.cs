using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Menu.Data.Models;
using VirtoCommerce.Content.Menu.Data.Repositories;
using VirtoCommerce.Foundation.Data.Infrastructure;

namespace VirtoCommerce.Content.Menu.Data
{
	public class SqlMenuDatabaseInitializer : SetupDatabaseInitializer<DatabaseMenuRepositoryImpl, Migrations.Configuration>
	{
		protected override void Seed(DatabaseMenuRepositoryImpl repository)
		{
			base.Seed(repository);

			CreateDefaultMenuLinkLists(repository, "SampleStore");
			CreateDefaultMenuLinkLists(repository, "AppleStore");
			CreateDefaultMenuLinkLists(repository, "SonyStore");
		}

		private void CreateDefaultMenuLinkLists(DatabaseMenuRepositoryImpl repository, string storeId)
		{
			var footerList = new MenuLinkList
			{
				Id = Guid.NewGuid().ToString(),
				Language = "en-US",
				Name = "Footer",
				StoreId = storeId,
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			};

			footerList.MenuLinks = new Collection<MenuLink>();

			footerList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = footerList.Id,
				Priority = 30,
				Title = "About Us",
				Url = "http://virtocommerce.com/about",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			footerList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = footerList.Id,
				Priority = 20,
				Title = "Delivery & Returns",
				Url = "http://demo.virtocommerce.com/#",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			footerList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = footerList.Id,
				Priority = 10,
				Title = "Terms & Conditions",
				Url = "http://virtocommerce.com/enterprise-edition",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			footerList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = footerList.Id,
				Priority = 0,
				Title = "Contact Us",
				Url = "http://virtocommerce.com/contact-us",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			repository.Add(footerList);

			var headerList = new MenuLinkList
			{
				Id = Guid.NewGuid().ToString(),
				Language = "en-US",
				Name = "Header",
				StoreId = storeId,
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			};

			headerList.MenuLinks = new Collection<MenuLink>();
			headerList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = headerList.Id,
				Priority = 30,
				Title = "My Account",
				Url = "http://demo.virtocommerce.com/en-us/account",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			headerList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = headerList.Id,
				Priority = 20,
				Title = "My Wishlist",
				Url = "http://demo.virtocommerce.com/en-us/account/wishlist",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			headerList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = headerList.Id,
				Priority = 10,
				Title = "My Cart",
				Url = "http://demo.virtocommerce.com/en-us/cart",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			headerList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = headerList.Id,
				Priority = 0,
				Title = "Checkout",
				Url = "http://demo.virtocommerce.com/en-us/checkout",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			repository.Add(headerList);

			var accountList = new MenuLinkList
			{
				Id = Guid.NewGuid().ToString(),
				Language = "en-US",
				Name = "My Account",
				StoreId = storeId,
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			};

			accountList.MenuLinks = new Collection<MenuLink>();
			accountList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = accountList.Id,
				Priority = 50,
				Title = "Account Dashboard",
				Url = "http://demo.virtocommerce.com/en-us/account",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			accountList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = accountList.Id,
				Priority = 40,
				Title = "Account Information",
				Url = "http://demo.virtocommerce.com/en-us/account/edit",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			accountList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = accountList.Id,
				Priority = 30,
				Title = "Address Book",
				Url = "http://demo.virtocommerce.com/en-us/account/addressbook",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			accountList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = accountList.Id,
				Priority = 20,
				Title = "My Orders",
				Url = "http://demo.virtocommerce.com/en-us/account/orders",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			accountList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = accountList.Id,
				Priority = 10,
				Title = "My Wishlist",
				Url = "http://demo.virtocommerce.com/en-us/account/wishlist",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			accountList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = accountList.Id,
				Priority = 0,
				Title = "My Returns",
				Url = "http://demo.virtocommerce.com/en-us/account/rmareturns",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			repository.Add(accountList);

			repository.UnitOfWork.Commit();
		}
	}
}
