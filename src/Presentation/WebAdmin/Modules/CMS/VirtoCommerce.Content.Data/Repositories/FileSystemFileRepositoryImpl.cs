using System.Linq;

namespace VirtoCommerce.Content.Data.Repositories
{
	#region

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Threading.Tasks;
	using VirtoCommerce.Content.Data.Models;

	#endregion

	public class FileSystemFileRepositoryImpl : IFileRepository
	{
		private readonly string _baseDirectoryPath;

		public FileSystemFileRepositoryImpl(string baseDirectoryPath)
		{
			this._baseDirectoryPath = baseDirectoryPath;
		}

		#region Public Methods and Operators


		/// <summary>
		/// Gets content item.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public ContentItem GetContentItem(string path)
		{
			var retVal = new ContentItem();

			var fullPath = GetFullPath(path);

			using (var sr = File.OpenText(fullPath))
			{
				var itemName = Path.GetFileName(fullPath);

				var content = sr.ReadToEnd();

				retVal.Content = content;
				retVal.Name = itemName;
				retVal.Path = path;
			}

			return retVal;
		}


		public IEnumerable<Theme> GetThemes(string storePath)
		{
			var fullPath = GetFullPath(storePath);

            if (!Directory.Exists(fullPath)) return Enumerable.Empty<Theme>();

			var directories = Directory.GetDirectories(fullPath);

			return directories.Select(dir => new Theme { Name = FixName(dir, fullPath), ThemePath = RemoveBaseDirectory(dir) });
		}

		public IEnumerable<ContentItem> GetContentItems(string path, bool loadContent = false)
		{
			var fullPath = GetFullPath(path);

			var directoriesQueue = new Queue<string>();

			var files = Directory.GetFiles(fullPath);
			var directories = Directory.GetDirectories(fullPath);

			foreach (var directory in directories)
			{
				directoriesQueue.Enqueue(directory);
			}

			var items = files.Select(file => new ContentItem { Name = Path.GetFileName(file), Path = this.RemoveBaseDirectory(file) }).ToList();

			while (directoriesQueue.Count > 0)
			{
				var directory = directoriesQueue.Dequeue();
				var newDirectories = Directory.GetDirectories(directory);
				var newFiles = Directory.GetFiles(directory);
				items.AddRange(newFiles.Select(file => new ContentItem { Name = Path.GetFileName(file), Path = this.RemoveBaseDirectory(file) }));
				foreach (var newDirectory in newDirectories)
				{
					directoriesQueue.Enqueue(newDirectory);
				}
			}

			if (loadContent)
			{
				Parallel.ForEach(items, file =>
				{
					var fullFile = GetContentItem(file.Path);
					file.Content = fullFile.Content;
				});
			}

			return items;
		}

		public void SaveContentItem(string path, ContentItem item)
		{
			var fullPath = GetFullPath(path);

			var directoryPath = Path.GetDirectoryName(fullPath);
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}

			using (var fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write))
			{
				using (var sw = new StreamWriter(fs))
				{
					sw.Write(item.Content);
					sw.Close();
				}
				fs.Close();
			}
		}

		public void DeleteContentItem(string path)
		{
			var fullPath = GetFullPath(path);

			if (File.Exists(fullPath))
				File.Delete(fullPath);
		}

		#endregion

		private string GetFullPath(string path)
		{
			//return string.Format("{0}{2}", this._baseDirectoryPath, path).Replace("/", "\\");
			return Path.Combine(this._baseDirectoryPath, path).Replace("/", "\\");
		}

		private string RemoveBaseDirectory(string path)
		{
			return path.Replace(this._baseDirectoryPath, string.Empty).Replace("\\", "/").TrimStart('/');
		}

		private string FixName(string path, string fullPath)
		{
			return path.Replace(fullPath, string.Empty).Trim('\\');
		}
	}
}