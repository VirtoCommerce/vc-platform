using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Content.Data.Repositories
{
	public class FileSystemContentRepositoryImpl : IContentRepository
	{
		private readonly string _baseDirectoryPath;

		public FileSystemContentRepositoryImpl(string baseDirectoryPath)
		{
			this._baseDirectoryPath = baseDirectoryPath;
		}

		public Task<bool> DeleteContentItem(string path)
		{
			var fullPath = this.GetFullPath(path);

			if (File.Exists(fullPath))
			{
				File.Delete(fullPath);
			}

			var directoryPath = Path.GetDirectoryName(fullPath);
			while (Directory.GetFiles(directoryPath).Length == 0 && Directory.GetDirectories(directoryPath).Length == 0
				&& directoryPath.Contains(this._baseDirectoryPath))
			{
				var newDirectoryPath = Directory.GetParent(directoryPath).FullName;
				Directory.Delete(directoryPath, false);
				directoryPath = newDirectoryPath;
			}

			return Task.FromResult(true);
		}

		public Task<ContentItem> GetContentItem(string path)
		{
			var retVal = new ContentItem();

			var fullPath = this.GetFullPath(path);

			var itemName = Path.GetFileName(fullPath);
			retVal.ByteContent = File.ReadAllBytes(fullPath);
			retVal.Name = itemName;
			retVal.Path = path;

			return Task.FromResult(retVal);
		}

		public async Task<IEnumerable<ContentItem>> GetContentItems(string path, GetThemeAssetsCriteria criteria)
		{
			var fullPath = this.GetFullPath(path);

			if (!Directory.Exists(fullPath))
				return Enumerable.Empty<ContentItem>();

			var files = Directory.GetFiles(fullPath, "*.*", SearchOption.AllDirectories);

			if (criteria.LastUpdateDate.HasValue)
			{
				var filterDate = criteria.LastUpdateDate.Value;
				if (filterDate.Kind == DateTimeKind.Unspecified)
					filterDate = DateTime.SpecifyKind(filterDate, DateTimeKind.Utc);

				files = files.Where(i => File.GetLastWriteTimeUtc(i).Truncate(TimeSpan.FromSeconds(1)) > filterDate.Truncate(TimeSpan.FromSeconds(1))).ToArray();
			}

			var items =
				files.Select(
					file => new ContentItem { Name = Path.GetFileName(file), Path = this.RemoveBaseDirectory(file), ModifiedDate = File.GetLastWriteTimeUtc(file).Truncate(TimeSpan.FromSeconds(1)) })
					.ToList();

			if (criteria.LoadContent)
			{
				foreach (var contentItem in items)
				{
					var fullFile = await this.GetContentItem(contentItem.Path);
					contentItem.ByteContent = fullFile.ByteContent;
					contentItem.ContentType = fullFile.ContentType;
				}
			}

			return await Task.FromResult(items.AsEnumerable());
		}

		public Task<IEnumerable<Theme>> GetThemes(string storePath)
		{
			var fullPath = this.GetFullPath(storePath);

			if (!Directory.Exists(fullPath))
			{
				return Task.FromResult(Enumerable.Empty<Theme>());
			}

			var directories = Directory.GetDirectories(fullPath);

			var themes = new List<Theme>();

			foreach (var directory in directories)
			{
				var maxModified = Directory.GetLastWriteTimeUtc(directory);
				var directoriesQueue = new Queue<string>();

				var subDirectories = Directory.GetDirectories(directory);

				foreach (var subDirectory in subDirectories)
				{
					directoriesQueue.Enqueue(subDirectory);
					maxModified = Directory.GetLastWriteTimeUtc(subDirectory) < maxModified
						? maxModified
						: Directory.GetLastWriteTimeUtc(subDirectory);
				}

				themes.Add(
					new Theme
					{
						Name = this.FixName(directory, fullPath),
						ThemePath = this.RemoveBaseDirectory(directory),
						ModifiedDate = maxModified
					});
			}

			return Task.FromResult(themes.AsEnumerable());
		}

		public Task<bool> SaveContentItem(string path, ContentItem item)
		{
			var fullPath = this.GetFullPath(path);

			var directoryPath = Path.GetDirectoryName(fullPath);
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}

			using (var fs = new FileStream(fullPath, FileMode.Create))
			{
				fs.Write(item.ByteContent, 0, item.ByteContent.Length);
				fs.Close();
			}

			return Task.FromResult(true);
		}

		private string FixName(string path, string fullPath)
		{
			return path.Replace(fullPath, string.Empty).Trim('\\');
		}

		private string GetFullPath(string path)
		{
			return Path.Combine(this._baseDirectoryPath, path).Replace("/", "\\");
		}

		private string RemoveBaseDirectory(string path)
		{
			return path.Replace(this._baseDirectoryPath, string.Empty).Replace("\\", "/").TrimStart('/');
		}

		public Task<bool> DeleteTheme(string path)
		{
			throw new NotImplementedException();
		}


		public Models.ContentPage GetPage(string path)
		{
			var retVal = new Models.ContentPage();

			var fullPath = GetFullPath(path);

			if (File.Exists(fullPath))
			{
				var itemName = Path.GetFileNameWithoutExtension(fullPath);

				var content = File.ReadAllBytes(fullPath);

				retVal.Language = GetLanguageFromFullPath(fullPath);
				retVal.ByteContent = content;
				retVal.Name = itemName;
			}
			else
			{
				retVal = null;
			}

			return retVal;
		}

		public IEnumerable<Models.ContentPage> GetPages(string path)
		{
			var list = new List<Models.ContentPage>();

			var retVal = new List<Models.ContentPage>();

			var fullPath = GetFullPath(path);

			if (!Directory.Exists(fullPath))
			{
				Directory.CreateDirectory(fullPath);
			}

			var languages = Directory.GetDirectories(fullPath);

			foreach (var language in languages)
			{
				var files = Directory.GetFiles(language); ;

				list.AddRange(files.Select(f => new Models.ContentPage
				{
					Name = Path.GetFileNameWithoutExtension(f),
					ModifiedDate = Directory.GetLastWriteTimeUtc(f),
					Language = GetLanguageFromFullPath(f)
				}));
			}

			return list.ToArray();
		}

		public void SavePage(string path, Models.ContentPage page)
		{
			var fullPath = GetFullPath(path);

			if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
			}

			using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
			{
				using (var sw = new StreamWriter(fs))
				{
					sw.Write(page.ByteContent);
					sw.Close();
				}
				fs.Close();
			}
		}

		public void DeletePage(string path)
		{
			var fullPath = GetFullPath(path);

			if (File.Exists(fullPath))
			{
				File.Delete(fullPath);
			}
		}

		private string GetLanguageFromFullPath(string fullPath)
		{
			var steps = fullPath.Split('\\');
			var language = steps[steps.Length - 2];

			return language;
		}


		public IEnumerable<ContentPage> GetPages(string path, GetPagesCriteria criteria)
		{
			throw new NotImplementedException();
		}
	}
}
