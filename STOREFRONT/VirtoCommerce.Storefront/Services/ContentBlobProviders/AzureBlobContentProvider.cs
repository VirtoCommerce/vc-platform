using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CacheManager.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Services
{
    public class AzureBlobContentProvider : IContentBlobProvider
    {
        private readonly CloudBlobClient _cloudBlobClient;
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly CloudBlobContainer _container;
        private readonly CloudBlobDirectory _directory;
        private readonly string _containerName;
        private readonly string _baseDirectoryPath;
        private readonly ICacheManager<object> _cacheManager;

        public AzureBlobContentProvider(string connectionString, string basePath, ICacheManager<object> cacheManager)
        {
            _cacheManager = cacheManager;
            var parts = basePath.Split(new[] { "/", "\\" }, StringSplitOptions.RemoveEmptyEntries);

            _containerName = parts.FirstOrDefault();
            _baseDirectoryPath = string.Join("/", parts.Skip(1));

            if (!CloudStorageAccount.TryParse(connectionString, out _cloudStorageAccount))
            {
                throw new InvalidOperationException("Failed to get valid connection string");
            }
            _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            _container = _cloudBlobClient.GetContainerReference(_containerName);
            if (_baseDirectoryPath != null)
            {
                _directory = _container.GetDirectoryReference(_baseDirectoryPath);
            }
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
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            path = NormalizePath(path);
            if (_directory != null)
            {
                return _directory.GetBlockBlobReference(path).OpenRead();
            }

            return _container.GetBlobReference(path).OpenRead();
        }

        /// <summary>
        /// Check that blob or folder with passed path exist
        /// </summary>
        /// <param name="path">path /folder/blob.md</param>
        /// <returns></returns>
        public virtual bool PathExists(string path)
        {
            path = NormalizePath(path);

            var result = _cacheManager.Get("AzureBlobContentProvider.PathExists:" + path.GetHashCode(), "ContentRegion", () =>
            {
                // If requested path is a directory we should always return true because Azure blob storage does not support checking if directories exist
                var retVal = string.IsNullOrEmpty(Path.GetExtension(path));
                if (!retVal)
                {
                    var url = GetAbsoluteUrl(path);
                    try
                    {
                        retVal = _cloudBlobClient.GetBlobReferenceFromServer(new Uri(url)).Exists();
                    }
                    catch (Exception)
                    {
                        //Azure blob storage client does not provide method to check blob url exist without throwing exception
                    }
                }

                return (object)retVal;
            });

            return (bool)result;
        }

        /// <summary>
        /// Search blob content in specified folder
        /// </summary>
        /// <param name="path">folder path in which the search will be processed</param>
        /// <param name="searchPattern">search blob name pattern can be used mask (*, ? symbols)</param>
        /// <param name="recursive"> recursive search</param>
        /// <returns>Returns relative path for all found blobs  example: /folder/blob.md </returns>
        public virtual IEnumerable<string> Search(string path, string searchPattern, bool recursive)
        {
            var retVal = new List<string>();
            path = NormalizePath(path);
            IEnumerable<IListBlobItem> blobItems;
            if (_directory != null)
            {
                var directoryBlob = _directory;
                if (!string.IsNullOrEmpty(path))
                {
                    directoryBlob = _directory.GetDirectoryReference(path);
                }
                blobItems = directoryBlob.ListBlobs(useFlatBlobListing: recursive);
            }
            else
            {
                blobItems = _container.ListBlobs(useFlatBlobListing: recursive);
            }
            // Loop over items within the container and output the length and URI.
            foreach (IListBlobItem item in blobItems)
            {
                var block = item as CloudBlockBlob;
                if (block != null)
                {
                    var blobRelativePath = GetRelativeUrl(block.Uri.ToString());
                    var fileName = Path.GetFileName(Uri.UnescapeDataString(block.Uri.ToString()));
                    if (fileName.FitsMask(searchPattern))
                    {
                        retVal.Add(blobRelativePath);
                    }
                }
            }
            return retVal;
        }

        #endregion

        protected virtual string NormalizePath(string path)
        {
            return path.Replace('\\', '/').TrimStart('/');
        }

        protected virtual string GetRelativeUrl(string url)
        {
            var absoluteUrl = GetAbsoluteUrl("").ToString();
            return url.Replace(absoluteUrl, string.Empty);
        }

        protected virtual string GetAbsoluteUrl(string path)
        {
            path = NormalizePath(path);
            return string.Join("/", _cloudBlobClient.BaseUri.ToString().TrimEnd('/'), _containerName, _baseDirectoryPath, path);
        }


        protected virtual void RaiseChangedEvent(FileSystemEventArgs args)
        {
            var changedEvent = Changed;
            if (changedEvent != null)
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
