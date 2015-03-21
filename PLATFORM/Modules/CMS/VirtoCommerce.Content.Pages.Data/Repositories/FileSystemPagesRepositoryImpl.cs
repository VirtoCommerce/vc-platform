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

			if (File.Exists(fullPath))
			{
				using (var sr = File.OpenText(fullPath))
				{
					var itemName = Path.GetFileNameWithoutExtension(fullPath);

					var content = sr.ReadToEnd();

					retVal.Language = GetLanguageFromFullPath(fullPath);
					retVal.Content = content;
					retVal.Name = itemName;
				}
			}
			else
			{
				retVal = null;
			}

			return retVal;
		}

		public IEnumerable<Models.ShortPageInfo> GetPages(string path)
		{
			var list = new List<Models.ShortPageInfo>();

			var retVal = new List<Models.ShortPageInfo>();

			var fullPath = GetFullPath(path);

			if (!Directory.Exists(fullPath))
			{
				Directory.CreateDirectory(fullPath);
			}

			var languages = Directory.GetDirectories(fullPath);

			foreach (var language in languages)
			{
				var files = Directory.GetFiles(language); ;

				list.AddRange(files.Select(f => new Models.ShortPageInfo
								{
									Name = Path.GetFileNameWithoutExtension(f),
									LastModified = Directory.GetLastWriteTimeUtc(f),
									Language = GetLanguageFromFullPath(f)
								}));
			}

			return list.ToArray();
		}

		public void SavePage(string path, Models.Page page)
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

		private string GetLanguageFromFullPath(string fullPath)
		{
			var steps = fullPath.Split('\\');
			var language = steps[steps.Length - 2];

			return language;
		}
	}
}
