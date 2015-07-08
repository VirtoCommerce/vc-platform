using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Text;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Content.Data.Utility;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.Content.Data
{
	public class SqlContentDatabaseInitializer : SetupDatabaseInitializer<DatabaseContentRepositoryImpl, Migrations.Configuration>
	{
		private readonly string _themePath;

		public SqlContentDatabaseInitializer()
		{

		}

		public SqlContentDatabaseInitializer(string themePath)
		{
			_themePath = themePath;
		}

		protected override void Seed(DatabaseContentRepositoryImpl repository)
		{
			base.Seed(repository);

			CreateDefaultMenuLinkLists(repository, "SampleStore");
            CreateAppleMenuLinkLists(repository, "AppleStore");
            CreateSonyMenuLinkLists(repository, "SonyStore");

			CreateDefaultPages(repository, "AppleStore");
			CreateDefaultPages(repository, "SampleStore");
			CreateDefaultPages(repository, "SonyStore");

			CreateDefaultTheme(repository, "AppleStore");
			CreateDefaultTheme(repository, "SampleStore");
			CreateDefaultTheme(repository, "SonyStore");
		}

        #region Footer Menu
        private void CreateFooterMenu(DatabaseContentRepositoryImpl repository, string storeId)
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
                Url = "~/pages/about_us",
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
	    }
        #endregion

        private void CreateDefaultMenuLinkLists(DatabaseContentRepositoryImpl repository, string storeId)
		{
            // Create footer menu
		    CreateFooterMenu(repository, storeId);

			var mainMenuList = NewMenu("en-US", "main-menu", storeId);

            mainMenuList.MenuLinks.Add(NewLink(mainMenuList.Id, "Audio & MP3", "~/samplestore/vendorvirtual-audio-mp3", 30));
            mainMenuList.MenuLinks.Add(NewLink(mainMenuList.Id, "TV & Video", "~/samplestore/vendorvirtual-tv-video", 20));
            mainMenuList.MenuLinks.Add(NewLink(mainMenuList.Id, "Cameras", "~/samplestore/vendorvirtual-cameras", 10));
            mainMenuList.MenuLinks.Add(NewLink(mainMenuList.Id, "Computers & Tablets", "~/samplestore/vendorvirtual-computers-tablets", 5));
            mainMenuList.MenuLinks.Add(NewLink(mainMenuList.Id, "Accessories", "~/samplestore/vendorvirtual-accessories", 0));

			repository.Add(mainMenuList);

			var audioMp3List = NewMenu("en-US", "Audio & MP3", storeId);
            audioMp3List.MenuLinks.Add(NewLink(audioMp3List.Id, "Apple iPod", "~/samplestore/vendorvirtual-audio-mp3?tags=brand_apple", 0));
            audioMp3List.MenuLinks.Add(NewLink(audioMp3List.Id, "Sony Walkman", "~/samplestore/vendorvirtual-audio-mp3?tags=brand_sony", 0));

			repository.Add(audioMp3List);
			repository.UnitOfWork.Commit();
		}

        private void CreateSonyMenuLinkLists(DatabaseContentRepositoryImpl repository, string storeId)
        {
            // Create footer menu
            CreateFooterMenu(repository, storeId);

            var mainMenuList = NewMenu("en-US", "main-menu", storeId);

            mainMenuList.MenuLinks.Add(NewLink(mainMenuList.Id, "Walkmans", "~/sonystore/sony-audio-mp3", 30));
            mainMenuList.MenuLinks.Add(NewLink(mainMenuList.Id, "TV & Video", "~/sonystore/sony-tv-video", 20));
            mainMenuList.MenuLinks.Add(NewLink(mainMenuList.Id, "Cameras", "~/sonystore/sony-cameras", 10));
            mainMenuList.MenuLinks.Add(NewLink(mainMenuList.Id, "Computers & Tablets", "~/sonystore/sony-computers-tablets", 5));
            mainMenuList.MenuLinks.Add(NewLink(mainMenuList.Id, "Accessories", "~/sonystore/sony-accessories", 0));

            repository.Add(mainMenuList);
            repository.UnitOfWork.Commit();
        }

        private void CreateAppleMenuLinkLists(DatabaseContentRepositoryImpl repository, string storeId)
        {
            // Create footer menu
            CreateFooterMenu(repository, storeId);

            var mainMenuList = NewMenu("en-US", "main-menu", storeId);

            mainMenuList.MenuLinks.Add(NewLink(mainMenuList.Id, "iPods", "~/applestore/apple-audio-mp3", 30));
            mainMenuList.MenuLinks.Add(NewLink(mainMenuList.Id, "Computers & Tablets", "~/applestore/apple-computers-tablets", 5));
            mainMenuList.MenuLinks.Add(NewLink(mainMenuList.Id, "Accessories", "~/applestore/apple-accessories", 0));           

            repository.Add(mainMenuList);
            repository.UnitOfWork.Commit();
        }

        private MenuLinkList NewMenu(string language, string name, string store)
        {
            var mainMenuList = new MenuLinkList
            {
                Id = Guid.NewGuid().ToString(),
                Language = language,
                Name = name,
                StoreId = store,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "initialize"
            };

            return mainMenuList;
        }

	    private MenuLink NewLink(string listId, string title, string url, int priority)
	    {
	        var link = new MenuLink
	                   {
	                       Id = Guid.NewGuid().ToString(),
	                       IsActive = true,
                           MenuLinkListId = listId,
	                       Priority = priority,
	                       Title = title,
	                       Url = url,
	                       CreatedDate = DateTime.UtcNow,
	                       CreatedBy = "initialize"
	                   };

	        return link;
	    }

		public void CreateDefaultPages(DatabaseContentRepositoryImpl repository, string storeId)
		{
			var builder = new StringBuilder();
			builder.AppendLine("<p>A great About Us page helps builds trust between you and your customers. The more content you provide about you and your business, the more confident people will be when purchasing from your store.</p>");
			builder.AppendLine("<p>Your About Us page might include:</p>");
			builder.AppendLine("<ul>");
			builder.AppendLine("<li>Who you are</li>");
			builder.AppendLine("<li>Why you sell the items you sell</li>");
			builder.AppendLine("<li>Where you are located</li>");
			builder.AppendLine("<li>How long you have been in business</li>");
			builder.AppendLine("<li>How long you have been running your online shop</li>");
			builder.AppendLine("<li>Who are the people on your team</li>");
			builder.AppendLine("<li>Contact information</li>");
			builder.AppendLine("<li>Social links (Twitter, Facebook)</li>");
			builder.AppendLine("</ul>");
			builder.AppendLine("<p>To edit the content on this page, go to the <a href=\"http://virtocommerce.com/\">Pages</a> section of your Shopify admin.</p>");

			repository.Add(new ContentPage
			{
				Id = string.Format("{0}/en-US/pages/about_us.md", storeId),
				Path = string.Format("{0}/en-US/pages/about_us.md", storeId),
				Name = "pages/about_us.md",
				Language = "en-US",
				ByteContent = Encoding.UTF8.GetBytes(builder.ToString()),
				ContentType = "text/html",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			builder = new StringBuilder();
			builder.AppendLine("<p>Write a few sentences to tell people about your store (the kind of products you sell, your mission, etc). You can also add images and videos to help tell your story and generate more interest in your shop.</p>");
			builder.AppendLine("<p>To edit the content on this page, go to the <a href=\"http://virtocommerce.com/\">Pages</a> section of your Shopify admin.</p>");

			repository.Add(new ContentPage
			{
				Id = string.Format("{0}/en-US/pages/default.md", storeId),
				Path = string.Format("{0}/en-US/pages/default.md", storeId),
				Name = "pages/default.md",
				Language = "en-US",
				ByteContent = Encoding.UTF8.GetBytes(builder.ToString()),
				ContentType = "text/html",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			builder = new StringBuilder();
			builder.AppendLine("<p>Some include text.</p>");

			repository.Add(new ContentPage
			{
				Id = string.Format("{0}/en-US/includes/some_def.html", storeId),
				Path = string.Format("{0}/en-US/includes/some_def.html", storeId),
				Name = "includes/some_def.html",
				Language = "en-US",
				ByteContent = Encoding.UTF8.GetBytes(builder.ToString()),
				ContentType = "text/html",
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			});

			repository.UnitOfWork.Commit();
		}

		private void CreateDefaultTheme(DatabaseContentRepositoryImpl repository, string storeId)
		{
			var id = string.Format("{0}/Default", storeId);
			if (!repository.Themes.Any(s => s.Id.Equals(id)))
			{
				var theme = new Theme
				{
					Id = id,
					Name = "Default",
					ThemePath = id,
					CreatedDate = DateTime.UtcNow,
					CreatedBy = "initialize"
				};

				repository.Add(theme);
				repository.UnitOfWork.Commit();

			    if (String.IsNullOrEmpty(_themePath))
			        return;

				var files = Directory.GetFiles(_themePath, "*.*", SearchOption.AllDirectories);

				var items =
					files.Select(
						file => new ContentItem { Name = Path.GetFileName(file), Path = RemoveBaseDirectory(file), ModifiedDate = File.GetLastWriteTimeUtc(file) })
						.ToList();

				foreach (var contentItem in items)
				{
					var fullFile = GetContentItem(contentItem.Path, storeId);
					contentItem.Id = Guid.NewGuid().ToString();
					contentItem.ByteContent = fullFile.ByteContent;
					contentItem.ContentType = fullFile.ContentType;
					contentItem.Path = string.Format("{0}/{1}", storeId, contentItem.Path);
					contentItem.CreatedDate = DateTime.UtcNow;
					contentItem.ModifiedDate = DateTime.UtcNow;
					contentItem.CreatedBy = "initialize";

					repository.Add(contentItem);
					repository.UnitOfWork.Commit();
				}
			}
		}

		private ContentItem GetContentItem(string path, string storeId)
		{
			var retVal = new ContentItem();

			var fullPath = this.GetFullPath(path);

			var itemName = Path.GetFileName(fullPath);
			retVal.ByteContent = File.ReadAllBytes(fullPath);
			retVal.Name = itemName;
			retVal.ContentType = ContentTypeUtility.GetContentType(fullPath, retVal.ByteContent);

			return retVal;
		}

		private string GetFullPath(string path)
		{
			return Path.Combine(_themePath, path).Replace("/", "\\");
		}

		private string RemoveBaseDirectory(string path)
		{
			return path.Replace(_themePath, string.Empty).Replace("\\", "/").TrimStart('/');
		}
	}
}
