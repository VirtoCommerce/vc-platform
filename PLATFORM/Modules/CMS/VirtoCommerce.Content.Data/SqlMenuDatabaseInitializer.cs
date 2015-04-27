using System;
using System.Collections.ObjectModel;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Foundation.Data.Infrastructure;

namespace VirtoCommerce.Content.Data
{
	public class SqlMenuDatabaseInitializer : SetupDatabaseInitializer<DatabaseMenuRepositoryImpl, VirtoCommerce.Content.Menu.Data.Migrations.Configuration>
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
				Name = "footer",
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
				Title = "Search",
				Url = "~/search",
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
				Url = "#",
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
				Url = "#",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			repository.Add(footerList);

			var mainMenuList = new MenuLinkList
			{
				Id = Guid.NewGuid().ToString(),
				Language = "en-US",
				Name = "main-menu",
				StoreId = storeId,
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			};

			mainMenuList.MenuLinks = new Collection<MenuLink>();
			mainMenuList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = mainMenuList.Id,
				Priority = 30,
				Title = "Audio & MP3",
				Url = "~/collections/audio-mp3",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			mainMenuList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = mainMenuList.Id,
				Priority = 20,
				Title = "TV & Video",
				Url = "~/collections/tv-video",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			mainMenuList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = mainMenuList.Id,
				Priority = 10,
				Title = "Cameras",
				Url = "~/collections/cameras",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			mainMenuList.MenuLinks.Add(new MenuLink
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				MenuLinkListId = mainMenuList.Id,
				Priority = 0,
				Title = "Computers & Tablets",
				Url = "~/collections/computers-tablets",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

            mainMenuList.MenuLinks.Add(new MenuLink
            {
                Id = Guid.NewGuid().ToString(),
                IsActive = true,
                MenuLinkListId = mainMenuList.Id,
                Priority = 0,
                Title = "Accessories",
                Url = "~/collections/accessories",
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "initialize"
            });

			repository.Add(mainMenuList);

            var audioMP3List = new MenuLinkList
            {
                Id = Guid.NewGuid().ToString(),
                Language = "en-US",
                Name = "Audio & MP3",
                StoreId = storeId,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "initialize"
            };

            audioMP3List.MenuLinks.Add(new MenuLink
            {
                Id = Guid.NewGuid().ToString(),
                IsActive = true,
                MenuLinkListId = audioMP3List.Id,
                Priority = 0,
                Title = "Apple iPod",
                Url = "~/collections/audio-mp3/brand_Apple",
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "initialize"
            });

            audioMP3List.MenuLinks.Add(new MenuLink
            {
                Id = Guid.NewGuid().ToString(),
                IsActive = true,
                MenuLinkListId = audioMP3List.Id,
                Priority = 0,
                Title = "Sony Walkman",
                Url = "~/collections/audio-mp3/brand_Sony",
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "initialize"
            });

            repository.Add(audioMP3List);

			repository.UnitOfWork.Commit();
		}
	}
}
