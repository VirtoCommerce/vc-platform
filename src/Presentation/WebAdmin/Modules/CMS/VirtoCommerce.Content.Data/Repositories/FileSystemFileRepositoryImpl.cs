namespace VirtoCommerce.Content.Data.Repositories
{
	#region

	using System;
	using System.Collections.Generic;
	using System.IO;
	using VirtoCommerce.Content.Data.Models;

	#endregion

	public class FileSystemFileRepositoryImpl : IFileRepository
	{
		private string _mainPath;

		public FileSystemFileRepositoryImpl(string mainPath)
		{
			_mainPath = mainPath;
		}

		#region Public Methods and Operators

		public ContentItem GetContentItem(string themePath, string path)
        {
			var retVal = new ContentItem();

			var fullPath = GetFullPath(themePath, path);

			using (var sr = File.OpenText(fullPath))
			{
				var itemName = Path.GetFileName(fullPath);

				var content = sr.ReadToEnd();

				retVal.Content = content;
				retVal.ContentType = ContentType.File;
				retVal.Name = itemName;
				retVal.Path = path;
			}

			return retVal;
        }

		public ContentItem[] GetContentItems(string themePath, string path)
		{
			var fullPath = GetFullPath(themePath, path);

			List<ContentItem> items = new List<ContentItem>();

			var files = Directory.GetFiles(fullPath);
			var directories = Directory.GetDirectories(fullPath);

			foreach (var directory in directories)
			{
				var contentItem = new ContentItem();
				contentItem.ContentType = ContentType.Directory;
				contentItem.Name = FixName(directory, fullPath);
				contentItem.Path = FixPath(themePath, directory);

				items.Add(contentItem);
			}

			foreach (var file in files)
			{
				var contentItem = new ContentItem();
				contentItem.ContentType = ContentType.Directory;
				contentItem.Name = Path.GetFileName(file);
				contentItem.Path = FixPath(themePath, file);

				items.Add(contentItem);
			}

			return items.ToArray();
		}

		public void SaveContentItem(string themePath, ContentItem item)
		{
			var fullPath = GetFullPath(themePath, item.Path);

			var directoryPath = Path.GetDirectoryName(fullPath);
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}

			using(var fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write))
			{
				using (var sw = new StreamWriter(fs))
				{
					sw.Write(item.Content);
					sw.Close();
				}
				fs.Close();
			}
		}

		public void DeleteContentItem(string themePath, ContentItem item)
		{
			var fullPath = GetFullPath(themePath, item.Path);

			if (File.Exists(fullPath))
				File.Delete(fullPath);
		}

		#endregion

		private string GetFullPath(string themePath, string path)
		{
			return string.Format("{0}{1}{2}", _mainPath, themePath, path).Replace("/", "\\");
		}

		private string FixPath(string themePath, string path)
		{
			return path.Replace(_mainPath, string.Empty).Replace("\\", "/").Replace(themePath, string.Empty).TrimStart('/');
		}

		private string FixName(string path, string fullPath)
		{
			return path.Replace(fullPath, string.Empty).Trim('\\');
		}
	}
}