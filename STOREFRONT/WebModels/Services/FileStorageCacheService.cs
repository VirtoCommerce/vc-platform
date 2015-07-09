using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using VirtoCommerce.Web.Models.Storage;

namespace VirtoCommerce.Web.Services
{
    public class FileStorageCacheService
    {
        private readonly string _baseFolder;

        private DateTime? _lastUpdated;

        private FileMonitoringService _monitor;
        private static readonly object _LockObject = new object();

        public static FileStorageCacheService Create(string baseFolder)
        {
            var cacheKey = "filestorage-" + baseFolder;
            var fileStorage = HttpContext.Current.Cache[cacheKey] as FileStorageCacheService;

            if (fileStorage != null) return fileStorage;
            lock (_LockObject)
            {
                fileStorage = HttpContext.Current.Cache[cacheKey] as FileStorageCacheService;
                if (fileStorage == null)
                {
                    fileStorage = new FileStorageCacheService(baseFolder);
                    HttpRuntime.Cache.Insert(cacheKey, fileStorage, null);
                }
            }

            return fileStorage;
            //return new FileStorageCacheService(baseFolder);
        }

        private FileStorageCacheService(string baseFolder)
        {
            this._baseFolder = baseFolder;


            var directory = new DirectoryInfo(baseFolder);

            if (!directory.Exists)
            {
                directory.Create();
                _lastUpdated = null;
            }
            
            _monitor = new FileMonitoringService(baseFolder);
            _monitor.Changed += FileChanged;
        }

        void FileChanged(object sender, string e)
        {
            _lastUpdated = DateTime.UtcNow;
        }

        public DateTime? GetLatestUpdate()
        {
            if (_lastUpdated.HasValue) return _lastUpdated;

            var directory = new DirectoryInfo(BaseDirectory);

            var latest =
                directory.GetFiles("*.*", SearchOption.AllDirectories)
                    .OrderByDescending(f => f.LastWriteTimeUtc)
                    .FirstOrDefault();

            _lastUpdated = latest != null ? (DateTime?)latest.LastWriteTimeUtc : null;
            return _lastUpdated;
            /*
            var directory = new DirectoryInfo(BaseDirectory);

            if (!directory.Exists)
            {
                directory.Create();
                return null;
            }

            var latest =
    directory.GetFiles("*.*", SearchOption.AllDirectories)
        .OrderByDescending(f => f.LastWriteTimeUtc)
        .FirstOrDefault();

            return latest != null ? (DateTime?)latest.LastWriteTimeUtc : null;
             * */
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

            if (_lastUpdated.HasValue && item.Updated > _lastUpdated)
            {
                _lastUpdated = item.Updated;
            }

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
                return this._baseFolder;
            }
        }
    }
}