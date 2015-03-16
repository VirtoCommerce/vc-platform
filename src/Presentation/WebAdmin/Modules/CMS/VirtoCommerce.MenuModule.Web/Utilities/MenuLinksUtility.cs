using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using VirtoCommerce.MenuModule.Web.Models;

namespace VirtoCommerce.MenuModule.Web.Utilities
{
	public static class MenuLinksUtility
	{
		public static IEnumerable<MenuLinkList> GetDefaultLists(string storeId)
		{
			var lists = new List<MenuLinkList>();

			var footerList = new MenuLinkList
			{
				Id = Guid.NewGuid().ToString(),
				Language = "en-US",
				Name = "Footer",
				StoreId = storeId
            };

            footerList.MenuLinks = new Collection<MenuLink>();

			footerList.MenuLinks.Add(new MenuLink
									{
										Id = Guid.NewGuid().ToString(),
										IsActive = true,
										MenuLinkListId = footerList.Id,
										Priority = 30,
										Title = "About Us",
										Url = "http://virtocommerce.com/about"
									});

			footerList.MenuLinks.Add(new MenuLink
									{
										Id = Guid.NewGuid().ToString(),
										IsActive = true,
										MenuLinkListId = footerList.Id,
										Priority = 20,
										Title = "Delivery & Returns",
										Url = "http://demo.virtocommerce.com/#"
									});

			footerList.MenuLinks.Add(new MenuLink
									{
										Id = Guid.NewGuid().ToString(),
										IsActive = true,
										MenuLinkListId = footerList.Id,
										Priority = 10,
										Title = "Terms & Conditions",
										Url = "http://virtocommerce.com/enterprise-edition"
									});

			footerList.MenuLinks.Add(new MenuLink
									{
										Id = Guid.NewGuid().ToString(),
										IsActive = true,
										MenuLinkListId = footerList.Id,
										Priority = 0,
										Title = "Contact Us",
										Url = "http://virtocommerce.com/contact-us"
									});

			lists.Add(footerList);

			var headerList = new MenuLinkList
			{
				Id = Guid.NewGuid().ToString(),
				Language = "en-US",
				Name = "Header",
				StoreId = storeId
			};

            headerList.MenuLinks = new Collection<MenuLink>();
			headerList.MenuLinks.Add(new MenuLink
									{
										Id = Guid.NewGuid().ToString(),
										IsActive = true,
										MenuLinkListId = headerList.Id,
										Priority = 30,
										Title = "My Account",
										Url = "http://demo.virtocommerce.com/en-us/account"
									});

			headerList.MenuLinks.Add(new MenuLink
									{
										Id = Guid.NewGuid().ToString(),
										IsActive = true,
										MenuLinkListId = headerList.Id,
										Priority = 20,
										Title = "My Wishlist",
										Url = "http://demo.virtocommerce.com/en-us/account/wishlist"
									});

			headerList.MenuLinks.Add(new MenuLink
									{
										Id = Guid.NewGuid().ToString(),
										IsActive = true,
										MenuLinkListId = headerList.Id,
										Priority = 10,
										Title = "My Cart",
										Url = "http://demo.virtocommerce.com/en-us/cart"
									});

			headerList.MenuLinks.Add(new MenuLink
									{
										Id = Guid.NewGuid().ToString(),
										IsActive = true,
										MenuLinkListId = headerList.Id,
										Priority = 0,
										Title = "Checkout",
										Url = "http://demo.virtocommerce.com/en-us/checkout"
									});

			lists.Add(headerList);

			var accountList = new MenuLinkList
			{
				Id = Guid.NewGuid().ToString(),
				Language = "en-US",
				Name = "My Account",
				StoreId = storeId
			};

            accountList.MenuLinks = new Collection<MenuLink>();
			accountList.MenuLinks.Add(new MenuLink
									{
										Id = Guid.NewGuid().ToString(),
										IsActive = true,
										MenuLinkListId = accountList.Id,
										Priority = 50,
										Title = "Account Dashboard",
										Url = "http://demo.virtocommerce.com/en-us/account"
									});

			accountList.MenuLinks.Add(new MenuLink
									{
										Id = Guid.NewGuid().ToString(),
										IsActive = true,
										MenuLinkListId = accountList.Id,
										Priority = 40,
										Title = "Account Information",
										Url = "http://demo.virtocommerce.com/en-us/account/edit"
									});

			accountList.MenuLinks.Add(new MenuLink
									{
										Id = Guid.NewGuid().ToString(),
										IsActive = true,
										MenuLinkListId = accountList.Id,
										Priority = 30,
										Title = "Address Book",
										Url = "http://demo.virtocommerce.com/en-us/account/addressbook"
									});

			accountList.MenuLinks.Add(new MenuLink
									{
										Id = Guid.NewGuid().ToString(),
										IsActive = true,
										MenuLinkListId = accountList.Id,
										Priority = 20,
										Title = "My Orders",
										Url = "http://demo.virtocommerce.com/en-us/account/orders"
									});

			accountList.MenuLinks.Add(new MenuLink
									{
										Id = Guid.NewGuid().ToString(),
										IsActive = true,
										MenuLinkListId = accountList.Id,
										Priority = 10,
										Title = "My Wishlist",
										Url = "http://demo.virtocommerce.com/en-us/account/wishlist"
									});

			accountList.MenuLinks.Add(new MenuLink
									{
										Id = Guid.NewGuid().ToString(),
										IsActive = true,
										MenuLinkListId = accountList.Id,
										Priority = 0,
										Title = "My Returns",
										Url = "http://demo.virtocommerce.com/en-us/account/rmareturns"
									});

			lists.Add(accountList);

			return lists;
		}
	}
}