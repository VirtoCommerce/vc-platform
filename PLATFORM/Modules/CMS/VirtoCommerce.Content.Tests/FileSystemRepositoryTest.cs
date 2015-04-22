//namespace VirtoCommerce.Content.Tests
//{
//	#region

//	using System;
//	using System.Linq;
//	using System.Web.Http.Results;

//	using Moq;

//	using VirtoCommerce.Content.Data.Models;
//	using VirtoCommerce.Content.Data.Repositories;
//	using VirtoCommerce.Platform.Core.Settings;
//	using VirtoCommerce.ThemeModule.Web.Controllers.Api;

//	using Xunit;

//	#endregion

//	public class FileSystemRepositoryTest
//	{
//		private string _path = Environment.CurrentDirectory.Replace("\\bin\\Debug", string.Empty);

//		private FileSystemFileRepositoryImpl GetRepository()
//		{
//			var fullPath = string.Format("{0}\\Themes\\", _path);

//			var repository = new FileSystemFileRepositoryImpl(fullPath);

//			return repository;
//		}

//		#region Public Methods and Operators

//		[Fact]
//		public void GetThemesRepositoryTest()
//		{
//			var repository = GetRepository();

//			var items = repository.GetThemes("Apple");

//			Assert.Equal(items.Count(), 2);
//			Assert.Equal(items.ElementAt(0).ThemePath, "Apple/Simple");
//			Assert.Equal(items.ElementAt(0).Name, "Simple");
//		}

//		[Fact]
//		public void GetContentItemsRepositoryTest()
//		{
//			var repository = GetRepository();

//			var items = repository.GetContentItems("Apple/Simple");

//			Assert.Equal(items.Count(), 69);
//			Assert.Equal(items.ElementAt(0).Path, "Apple/Simple/assets/apple-touch-icon-114x114.png");
//			Assert.Equal(items.ElementAt(0).Name, "apple-touch-icon-114x114.png");
//		}

//		[Fact]
//		public void GetContentItemsWithFullContentRepositoryTest()
//		{
//			var repository = GetRepository();

//			var items = repository.GetContentItems("Apple/Simple", true);

//			Assert.Equal(items.Count(), 69);
//			Assert.Equal(items.ElementAt(0).Path, "Apple/Simple/assets/apple-touch-icon-114x114.png");
//			Assert.Equal(items.ElementAt(0).Name, "apple-touch-icon-114x114.png");
//			Assert.Equal(items.Count(i => string.IsNullOrEmpty(i.Content)), 0);
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
//			var path = "Apple/Simple/new/new123.liquid";

//			repository.SaveContentItem(path, content);

//			var items = repository.GetContentItems("Apple/Simple");

//			Assert.Equal(items.Count(), 70);

//			var item = repository.GetContentItem("Apple/Simple/new/new123.liquid");

//			Assert.True(item.Content.Contains("Some"));

//			content = new ContentItem();
//			content.Content = "Some new stuff. Changes";
//			path = "Apple/Simple/new/new123.liquid";

//			repository.SaveContentItem(path, content);

//			items = repository.GetContentItems("Apple/Simple");

//			Assert.Equal(items.Count(), 70);

//			item = repository.GetContentItem("Apple/Simple/new/new123.liquid");

//			Assert.True(item.Content.Contains("Some") && item.Content.Contains("Changes"));

//			path = "Apple/Simple/new/new123.liquid";

//			repository.DeleteContentItem(path);

//			items = repository.GetContentItems("Apple/Simple");

//			Assert.Equal(items.Count(), 69);
//		}

//		#endregion
//	}
//}