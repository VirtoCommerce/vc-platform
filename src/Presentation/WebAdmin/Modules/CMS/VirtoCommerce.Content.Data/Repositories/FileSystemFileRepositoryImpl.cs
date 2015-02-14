using System.Linq;

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
				retVal.ContentType = ContentType.File;
				retVal.Name = itemName;
				retVal.Path = path;
			}

			return retVal;
        }

		public ContentItem[] GetContentItems(string path)
		{
            var fullPath = GetFullPath(path);

		    var files = Directory.GetFiles(fullPath);
			var directories = Directory.GetDirectories(fullPath);

		    var items = directories.Select(directory => new ContentItem { ContentType = ContentType.Directory, Name = this.FixName(directory, fullPath), Path = this.RemoveBaseDirectory(directory) }).ToList();
		    
            items.AddRange(files.Select(file => new ContentItem { ContentType = ContentType.Directory, Name = Path.GetFileName(file), Path = this.RemoveBaseDirectory(file) }));

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

		public void DeleteContentItem(ContentItem item)
		{
			var fullPath = GetFullPath(item.Path);

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