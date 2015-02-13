using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Content.Data.Services;
using Xunit;

namespace VirtoCommerce.Content.Tests
{
	public class ThemeServiceTests
	{
		#region Service Test With GitHub Repository

		private ThemeServiceImpl GetServiceWithGitHubRepository()
		{
			var githubMainPath = "Themes/";

			var repository = new GitHubFileRepositoryImpl(
				"EugeneOkhriemnko",
				"MfZUbM2wSDCdDADBEGpo",
				"Site-Theme",
				"EugeneOkhriemnko",
				"Site_Themes",
				githubMainPath);

			var service = new ThemeServiceImpl(repository);

			return service;
		}

		[Fact]
		public void GitHubRepositoryThemeServiceGetRootCollectionTest()
		{
			var service = GetServiceWithGitHubRepository();

			var items = service.GetContentItems("Apple", string.Empty, string.Empty);

			Assert.Equal(items.Length, 2);
			Assert.Equal(items[0].Path, "Simple");
		}

		[Fact]
		public void GitHubRepositoryThemeServiceGetCollectionTest()
		{
			var service = GetServiceWithGitHubRepository();

			var items = service.GetContentItems("Apple", "Simple", "templates");

			Assert.Equal(items.Length, 13);
			Assert.Equal(items[0].Path, "templates/404.liquid");
		}

		[Fact]
		public void GitHubRepositoryThemeServiceGetItemTest()
		{
			var service = GetServiceWithGitHubRepository();

			var item = service.GetContentItem("Apple", "Simple", "layout/theme.liquid");

			Assert.Equal(item.Path, "layout/theme.liquid");
			Assert.True(item.Content.Contains("<!DO"));
		}

		[Fact]
		public void GitHubRepositoryThemeServiceSaveAndDeleteItemTest()
		{
			var service = GetServiceWithGitHubRepository();

			var content = new ContentItem();
			content.Content = "Some new stuff";
			content.Path = "new/new123.liquid";

			service.SaveContentItem("Apple", "Simple", content);

			var items = service.GetContentItems("Apple", "Simple", "new");

			Assert.Equal(items.Length, 1);

			var item = service.GetContentItem("Apple", "Simple", "new/new123.liquid");

			Assert.True(item.Content.Contains("Some"));

			content = new ContentItem();
			content.Content = "Some new stuff. Changes";
			content.Path = "new/new123.liquid";

			service.SaveContentItem("Apple", "Simple", content);

			items = service.GetContentItems("Apple", "Simple", "new");

			Assert.Equal(items.Length, 1);

			item = service.GetContentItem("Apple", "Simple", "new/new123.liquid");

			Assert.True(item.Content.Contains("Some") && item.Content.Contains("Changes"));

			content = new ContentItem();
			content.Path = "new/new123.liquid";

			service.DeleteContentItem("Apple", "Simple", content);

			items = service.GetContentItems("Apple", "Simple", string.Empty);

			Assert.Equal(items.Where(i => i.Path.Contains("new")).Count(), 0);
		}

		#endregion

		#region Service Test With File System Repository

		private ThemeServiceImpl GetServiceWithFileSystemRepository()
		{
			var githubMainPath = string.Format("{0}\\Themes\\", Environment.CurrentDirectory.Replace("\\bin\\Debug", string.Empty));

			var repository = new FileSystemFileRepositoryImpl(githubMainPath);

			var service = new ThemeServiceImpl(repository);

			return service;
		}

		[Fact]
		public void FileSystemRepositoryThemeServiceGetRootCollectionTest()
		{
			var service = GetServiceWithFileSystemRepository();

			var items = service.GetContentItems("Apple", string.Empty, string.Empty);

			Assert.Equal(items.Length, 2);
			Assert.Equal(items[0].Path, "Simple");
		}

		[Fact]
		public void FileSystemRepositoryThemeServiceGetCollectionTest()
		{
			var service = GetServiceWithFileSystemRepository();

			var items = service.GetContentItems("Apple", "Simple", "templates");

			Assert.Equal(items.Length, 13);
			Assert.Equal(items[0].Path, "templates/customers");
		}

		[Fact]
		public void FileSystemRepositoryThemeServiceGetItemTest()
		{
			var service = GetServiceWithFileSystemRepository();

			var item = service.GetContentItem("Apple", "Simple", "layout/theme.liquid");

			Assert.Equal(item.Path, "layout/theme.liquid");
			Assert.True(item.Content.Contains("<!DO"));
		}

		[Fact]
		public void FileSystemRepositoryThemeServiceSaveAndDeleteItemTest()
		{
			var service = GetServiceWithFileSystemRepository();

			var content = new ContentItem();
			content.Content = "Some new stuff";
			content.Path = "new/new123.liquid";

			service.SaveContentItem("Apple", "Simple", content);

			var items = service.GetContentItems("Apple", "Simple", "new");

			Assert.Equal(items.Length, 1);

			var item = service.GetContentItem("Apple", "Simple", "new/new123.liquid");

			Assert.True(item.Content.Contains("Some"));

			content = new ContentItem();
			content.Content = "Some new stuff. Changes";
			content.Path = "new/new123.liquid";

			service.SaveContentItem("Apple", "Simple", content);

			items = service.GetContentItems("Apple", "Simple", "new");

			Assert.Equal(items.Length, 1);

			item = service.GetContentItem("Apple", "Simple", "new/new123.liquid");

			Assert.True(item.Content.Contains("Some") && item.Content.Contains("Changes"));

			content = new ContentItem();
			content.Path = "new/new123.liquid";

			service.DeleteContentItem("Apple", "Simple", content);

			items = service.GetContentItems("Apple", "Simple", "new");

			Assert.Equal(items.Length, 0);
		}

		#endregion
	}
}
