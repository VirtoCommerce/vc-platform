using System;
using System.IO;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Web.Models.Services
{
    public class FileThemeService
    {
        private readonly string _themesFolder;

        public FileThemeService(string themesFolder)
        {
            this._themesFolder = themesFolder;
        }

        public DateTime GetLatestUpdate()
        {
            var directory = new DirectoryInfo(this.ThemeDirectory);

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

        public bool ApplyUpdates(ApiClient.DataContracts.Themes.ThemeAsset[] items)
        {
            var reloadCache = false;

            foreach (var themeAsset in items)
            {
                if (this.ApplyUpdate(themeAsset)) reloadCache = true;
            }

            //if (reloadCache) LoadThemeFiles(true);
            return reloadCache;
        }

        public bool ApplyUpdate(ApiClient.DataContracts.Themes.ThemeAsset item)
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
                var byteContent = item.ByteContent ?? Encoding.UTF8.GetBytes(item.Content);
                fs.Write(byteContent, 0, byteContent.Length);
                fs.Close();
            }

            // preserve the date
            File.SetLastWriteTimeUtc(fullPath, item.Updated);

            return true;
        }

        private string GetFullPath(string path)
        {
            return Path.Combine(this.ThemeDirectory, path).Replace("/", "\\");
        }              

        protected virtual string ThemeDirectory
        {
            get
            {
                var fileSystemMainPath = String.Format("{0}\\{1}", _themesFolder, SiteContext.Current.Theme);
                return fileSystemMainPath;
            }
        }
    }
}