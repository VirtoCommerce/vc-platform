using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Content.Pages.Data.Repositories
{
	public class FileSystemPagesRepositoryImpl : IPagesRepository
	{
		private readonly string _baseDirectoryPath;

		public FileSystemPagesRepositoryImpl(string baseDirectoryPath)
		{
			if (string.IsNullOrEmpty(baseDirectoryPath))
				throw new ArgumentNullException("baseDirectoryPath");

			_baseDirectoryPath = baseDirectoryPath;
		}

		public Models.Page GetPage(string path)
		{
			var retVal = new Models.Page();

			var fullPath = GetFullPath(path);

			using (var sr = File.OpenText(fullPath))
			{
				var itemName = Path.GetFileNameWithoutExtension(fullPath);

				var content = sr.ReadToEnd();

				retVal.Content = content;
				retVal.Name = itemName;
			}

			return retVal;
		}

		public IEnumerable<Models.ShortPageInfo> GetPages(string path)
		{
			var fullPath = GetFullPath(path);

			if (!Directory.Exists(fullPath))
			{
				Directory.CreateDirectory(fullPath);
			}

			var files = Directory.GetFiles(fullPath);

			return files.Select(f => new Models.ShortPageInfo { Name = Path.GetFileNameWithoutExtension(f), LastModified = Directory.GetLastWriteTimeUtc(f) });
		}

		public void SavePage(string path, Models.Page page)
		{
			var fullPath = GetFullPath(path);

			using (var fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write))
			{
				using (var sw = new StreamWriter(fs))
				{
					sw.Write(page.Content);
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

		private string GetFullPath(string path)
		{
			return string.Format("{0}{1}", _baseDirectoryPath, path).Replace("/", "\\");
		}
	}
}
