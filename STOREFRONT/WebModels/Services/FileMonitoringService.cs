using System;
using System.IO;

namespace VirtoCommerce.Web.Services
{
    public class FileMonitoringService
    {
        private readonly string _folder;

        public event EventHandler<string> Changed;

        public FileMonitoringService(string folder)
        {
            this._folder = folder;
        }

        public void StartWatching()
        {
            var watcher = new FileSystemWatcher();
            watcher.Path = _folder;
            watcher.IncludeSubdirectories = true;

            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
           | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            watcher.EnableRaisingEvents = true;
        }

        public void OnChanged(object source, FileSystemEventArgs e)
        {
            Changed.Invoke(this, e.FullPath);
        }

        public void OnRenamed(object source, RenamedEventArgs e)
        {
            Changed.Invoke(this, e.FullPath);
        }
    }
}
