using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Content.Data.Utility;
using VirtoCommerce.Foundation.Data.Infrastructure;

namespace VirtoCommerce.Content.Data
{
	public class SqlThemeDatabaseInitializer : SetupDatabaseInitializer<DatabaseFileRepositoryImpl, Migrations.Configuration>
	{
		private readonly string _path;

		public SqlThemeDatabaseInitializer(string path)
		{
			_path = path;
		}

		protected override void Seed(DatabaseFileRepositoryImpl repository)
		{
			CreateDefaultTheme(repository, "AppleStore");
			CreateDefaultTheme(repository, "SampleStore");
			CreateDefaultTheme(repository, "SonyStore");

			base.Seed(repository);
		}

		private void CreateDefaultTheme(DatabaseFileRepositoryImpl repository, string storeId)
		{
			var theme = new Theme
			{
				Id = Guid.NewGuid().ToString(),
				Name = "Default",
				ThemePath = string.Format("{0}/Default", storeId),
				CreatedDate = DateTime.UtcNow,
				CreatedBy = "initialize"
			};

			repository.Add(theme);
			repository.UnitOfWork.Commit();

			var files = Directory.GetFiles(_path , "*.*", SearchOption.AllDirectories);

			var items =
				files.Select(
					file => new ContentItem { Name = Path.GetFileName(file), Path = RemoveBaseDirectory(file), ModifiedDate = File.GetLastWriteTimeUtc(file) })
					.ToList();

			foreach (var contentItem in items)
			{
				var fullFile = GetContentItem(contentItem.Path, storeId);
				contentItem.Id = Guid.NewGuid().ToString();
				contentItem.ByteContent = fullFile.ByteContent;
				contentItem.Content = fullFile.Content;
				contentItem.ContentType = fullFile.ContentType;
				contentItem.Path = string.Format("{0}/{1}", storeId, contentItem.Path);
				contentItem.CreatedDate = DateTime.UtcNow;
				contentItem.ModifiedDate = DateTime.UtcNow;
				contentItem.CreatedBy = "initialize";


				repository.Add(contentItem);
				repository.UnitOfWork.Commit();
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
			return Path.Combine(_path, path).Replace("/", "\\");
		}

		private string RemoveBaseDirectory(string path)
		{
			return path.Replace(_path, string.Empty).Replace("\\", "/").TrimStart('/');
		}
	}
}
