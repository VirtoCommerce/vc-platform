using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Services
{
    public class FileSystemContentBlobProvider : IContentBlobProvider
    {
        private readonly string _basePath;
        private readonly FileSystemWatcher _fileSystemWatcher;
        public FileSystemContentBlobProvider(string basePath)
        {
            _basePath = basePath;
            _fileSystemWatcher = MonitorThemeFileSystemChanges(basePath);
        }

        #region IContentBlobProvider Members
        public event FileSystemEventHandler Changed;
        public event RenamedEventHandler Renamed;

        /// <summary>
        /// Open blob for read 
        /// </summary>
        /// <param name="path">blob relative path /folder/blob.md</param>
        /// <returns></returns>
        public virtual Stream OpenRead(string path)
        {
            path = NormalizePath(path);
            return File.OpenRead(path);
        }

        /// <summary>
        /// Check that blob or folder with passed path exist
        /// </summary>
        /// <param name="path">relative path /folder/blob.md</param>
        /// <returns></returns>
        public virtual bool PathExists(string path)
        {
            path = NormalizePath(path);
            var retVal = Directory.Exists(path);
            if(!retVal)
            {
                retVal = File.Exists(path);
            }
            return retVal;
        }

        /// <summary>
        /// Search blob content in specified folder
        /// </summary>
        /// <param name="path">relative folder path in which the search  Example: /folder</param>
        /// <param name="searchPattern">search blob name pattern can be used mask (*, ? symbols)</param>
        /// <param name="recursive"> recursive search</param>
        /// <returns>Returns relative path for all found blobs  example: /folder/blob.md </returns>
        public virtual IEnumerable<string> Search(string path, string searchPattern, bool recursive)
        {
            path = NormalizePath(path);
            return Directory.GetFiles(path, searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                            .Select(x=> GetRelativePath(x));
        }
        #endregion

        protected virtual string GetRelativePath(string path)
        {
            return path.Replace(_basePath, string.Empty).Replace('\\', '/');
        }

        protected virtual string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            path = path.Replace("/", "\\");
            path = path.Replace(_basePath, string.Empty);
            return Path.Combine(_basePath, path.TrimStart('\\'));
        }

        private FileSystemWatcher MonitorThemeFileSystemChanges(string path)
        {
            var fileSystemWatcher = new FileSystemWatcher();

            if (Directory.Exists(path))
            {
                fileSystemWatcher.Path = path;
                fileSystemWatcher.IncludeSubdirectories = true;

                FileSystemEventHandler handler = (sender, args) =>
                {
                    RaiseChangedEvent(args);
                };
                RenamedEventHandler renamedHandler = (sender, args) =>
                {
                    RaiseRenamedEvent(args);
                };
                var throttledHandler = handler.Throttle(TimeSpan.FromSeconds(5));
                // Add event handlers.
                fileSystemWatcher.Changed += throttledHandler;
                fileSystemWatcher.Created += throttledHandler;
                fileSystemWatcher.Deleted += throttledHandler;
                fileSystemWatcher.Renamed += renamedHandler;

                // Begin watching.
                fileSystemWatcher.EnableRaisingEvents = true;
            }
            return fileSystemWatcher;
        }

        protected virtual void RaiseChangedEvent(FileSystemEventArgs args)
        {
            var changedEvent = Changed;
            if(changedEvent != null)
            {
                changedEvent(this, args);
            }
        }

        protected virtual void RaiseRenamedEvent(RenamedEventArgs args)
        {
            var renamedEvent = Renamed;
            if (renamedEvent != null)
            {
                renamedEvent(this, args);
            }
        }
    }
}
