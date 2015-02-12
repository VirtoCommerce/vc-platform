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

		public ContentItem GetContentItem(string path)
        {
			var retVal = new ContentItem();

			var fullPath = GetFullPath(path);

			var item = File.OpenText(fullPath);
			var itemName = Path.GetFileName(fullPath);

			var content = item.ReadToEnd();

			retVal.Content = content;
			retVal.ContentType = ContentType.File;
			retVal.Name = itemName;
			retVal.Path = path;

			return retVal;
        }

		public ContentItem[] GetContentItems(string path)
		{
			var fullPath = GetFullPath(path);

			List<ContentItem> items = new List<ContentItem>();

			var files = Directory.GetFiles(fullPath);
			var directories = Directory.GetDirectories(fullPath);

			foreach (var directory in directories)
			{
				var contentItem = new ContentItem();
				contentItem.ContentType = ContentType.Directory;
				contentItem.Name = FixName(directory, fullPath);
				contentItem.Path = FixPath(directory);

				items.Add(contentItem);
			}

			foreach (var file in files)
			{
				var contentItem = new ContentItem();
				contentItem.ContentType = ContentType.Directory;
				contentItem.Name = Path.GetFileName(file);
				contentItem.Path = FixPath(file);

				items.Add(contentItem);
			}

			return items.ToArray();
		}

		public void SaveContentItem(ContentItem item)
		{
			var fullPath = GetFullPath(item.Path);

			var directoryPath = Path.GetDirectoryName(fullPath);
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}

			using(FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write))
			{
				using (StreamWriter sw = new StreamWriter(fullPath))
				{
					sw.Write(item.Content);
					sw.Close();
				}
				fs.Close();
			}
		}

		public void DeleteContentItem(ContentItem item)
		{
			var fullPath = GetFullPath(item.Path);

			if (File.Exists(fullPath))
				File.Delete(fullPath);
		}

		#endregion

		private string GetFullPath(string path)
		{
			path = path.Replace("/", "\\");

			return string.Join(string.Empty, _mainPath, path);
		}

		private string FixPath(string path)
		{
			return path.Replace(_mainPath, string.Empty).Replace("\\", "/");
		}

		private string FixName(string path, string fullPath)
		{
			return path.Replace(fullPath, string.Empty).Trim('\\');
		}
	}
}