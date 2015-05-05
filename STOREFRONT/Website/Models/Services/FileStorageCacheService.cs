using System;
using System.IO;
using System.Linq;
using System.Text;
using VirtoCommerce.Web.Models.Storage;

namespace VirtoCommerce.Web.Models.Services
{
    public class FileStorageCacheService
    {
        private readonly string _baseFolder;

        public FileStorageCacheService(string baseFolder)
        {
            this._baseFolder = baseFolder;
        }

        public DateTime GetLatestUpdate()
        {
            var directory = new DirectoryInfo(this.BaseDirectory);

            if (!directory.Exists)
            {
                directory.Create();
            }

            var latest =
                directory.GetFiles("*.*", SearchOption.AllDirectories)
                    .OrderByDescending(f => f.LastWriteTimeUtc)
                    .FirstOrDefault();

            if (latest == null) return DateTime.MinValue;

            return latest.LastWriteTimeUtc;
        }

        public bool ApplyUpdates(FileAsset[] items)
        {
            var reloadCache = false;

            foreach (var themeAsset in items)
            {
                if (this.ApplyUpdate(themeAsset)) reloadCache = true;
            }

            return reloadCache;
        }

        public bool ApplyUpdate(FileAsset item)
        {
            var fullPath = this.GetFullPath(item.Id);

            var directoryPath = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // check if file exists and if it's date is higher than the one on the server, dont update
            if (File.Exists(fullPath))
            {
                var lastUpdated = File.GetLastWriteTimeUtc(fullPath);
                if (lastUpdated >= item.Updated) return false;
            }

            using (var fs = new FileStream(fullPath, FileMode.Create))
            {
                var byteContent = item.ByteContent;

                if (byteContent == null && !String.IsNullOrEmpty(item.Content))
                    byteContent = Encoding.UTF8.GetBytes(item.Content);

                if(byteContent != null)
                    fs.Write(byteContent, 0, byteContent.Length);

                fs.Close();
            }

            // preserve the date
            File.SetLastWriteTimeUtc(fullPath, item.Updated);

            return true;
        }

        private string GetFullPath(string path)
        {
            return Path.Combine(this.BaseDirectory, path).Replace("/", "\\");
        }              

        protected virtual string BaseDirectory
        {
            get
            {
                return _baseFolder;
            }
        }
    }
}