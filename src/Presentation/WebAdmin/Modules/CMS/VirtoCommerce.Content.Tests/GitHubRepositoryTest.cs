namespace VirtoCommerce.Content.Tests
{
	#region

	using System;
	using System.Web.Http.Results;
	using System.Linq;

	using Moq;

	using VirtoCommerce.Content.Data.Models;
	using VirtoCommerce.Content.Data.Repositories;
	using VirtoCommerce.Framework.Web.Settings;
	using VirtoCommerce.ThemeModule.Web.Controllers.Api;

	using Xunit;

	#endregion

	public class GitHubRepositoryTest
	{
		private string _githubMainPath = "Themes/";

		private GitHubFileRepositoryImpl GetRepository()
		{
			var repository = new GitHubFileRepositoryImpl(
				"EugeneOkhriemnko",
				"MfZUbM2wSDCdDADBEGpo",
				"Site-Theme",
				"EugeneOkhriemnko",
				"Site_Themes",
				_githubMainPath);

			return repository;
		}

		#region Public Methods and Operators

		[Fact]
		public void GetRootCollectionRepositoryTest()
		{
			var repository = GetRepository();

			var items = repository.GetContentItems("Apple");

			Assert.Equal(items.Length, 2);
			Assert.Equal(items[0].Path, "Apple/Simple");
		}

		[Fact]
		public void GetCollectionRepositoryTest()
		{
			var repository = GetRepository();

			var items = repository.GetContentItems("Apple/Simple/templates");

			Assert.Equal(items.Length, 13);
			Assert.Equal(items[0].Path, "Apple/Simple/templates/404.liquid");
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

			items = repository.GetContentItems("Apple/Simple/");

			Assert.Equal(items.Where(i => i.Path.Contains("new")).Count(), 0);
		}

		#endregion
	}
}