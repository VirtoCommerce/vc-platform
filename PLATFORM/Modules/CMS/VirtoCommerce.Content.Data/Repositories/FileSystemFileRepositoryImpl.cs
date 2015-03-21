using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Framework.Web.Common;

namespace VirtoCommerce.Content.Data.Repositories
{
    public class FileSystemFileRepositoryImpl : IFileRepository
    {
        #region Fields

        private readonly string _baseDirectoryPath;

        #endregion

        #region Constructors and Destructors

        public FileSystemFileRepositoryImpl(string baseDirectoryPath)
        {
            this._baseDirectoryPath = baseDirectoryPath;
        }

        #endregion

        #region Public Methods and Operators

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

        /// <summary>
        ///     Gets content item.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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
                    contentItem.Content = fullFile.Content;
                    contentItem.ContentType = fullFile.ContentType;
                }

                /*
				Parallel.ForEach(items, async file =>
				{
					var fullFile = await GetContentItem(file.Path);
					file.Content = fullFile.Content;
					file.ContentType = fullFile.ContentType;
				});
                 * */
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

        #endregion

        #region Methods

        private string FixName(string path, string fullPath)
        {
            return path.Replace(fullPath, string.Empty).Trim('\\');
        }

        private string GetFullPath(string path)
        {
            //return string.Format("{0}{2}", this._baseDirectoryPath, path).Replace("/", "\\");
            return Path.Combine(this._baseDirectoryPath, path).Replace("/", "\\");
        }

        private string RemoveBaseDirectory(string path)
        {
            return path.Replace(this._baseDirectoryPath, string.Empty).Replace("\\", "/").TrimStart('/');
        }

        #endregion
    }
}
