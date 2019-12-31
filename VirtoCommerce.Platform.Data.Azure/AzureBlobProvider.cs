using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Azure
{
    public class AzureBlobProvider : IBlobStorageProvider, IBlobUrlResolver
    {
        public const string ProviderName = "AzureBlobStorage";
        private const string BlobCacheControlPropertyValue = "public, max-age=604800";
        private readonly CloudBlobClient _cloudBlobClient;
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly string _cdnUrl;

        public AzureBlobProvider(string connectionString)
            : this(connectionString, null)
        {
        }

        public AzureBlobProvider(string connectionString, string cdnUrl)
        {
            _cloudStorageAccount = ParseConnectionString(connectionString);
            _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            _cdnUrl = cdnUrl;
        }

        #region IBlobStorageProvider Members
        /// <summary>
        /// Get blob info by url
        /// </summary>
        /// <param name="blobUrl"></param>
        /// <returns></returns>
        public virtual BlobInfo GetBlobInfo(string blobUrl)
        {
            if (string.IsNullOrEmpty(blobUrl))
            {
                throw new ArgumentNullException(nameof(blobUrl));
            }
            var uri = blobUrl.IsAbsoluteUrl() ? new Uri(blobUrl) : new Uri(_cloudBlobClient.BaseUri, blobUrl.TrimStart('/'));
            BlobInfo retVal = null;
            try
            {
                var cloudBlob = _cloudBlobClient.GetBlobReferenceFromServer(uri);
                retVal = ConvertBlobToBlobInfo(cloudBlob);
            }
            catch (Exception)
            {
                //Azure blob storage client does not provide method to check blob url exist without throwing exception
            }
            return retVal;
        }

        /// <summary>
        /// Open stream for read blob by relative or absolute url
        /// </summary>
        /// <param name="blobUrl"></param>
        /// <returns>blob stream</returns>
        public virtual Stream OpenRead(string blobUrl)
        {
            if (string.IsNullOrEmpty(blobUrl))
            {
                throw new ArgumentNullException(nameof(blobUrl));
            }
            var uri = blobUrl.IsAbsoluteUrl() ? new Uri(blobUrl) : new Uri(_cloudBlobClient.BaseUri, blobUrl.TrimStart('/'));
            var cloudBlob = _cloudBlobClient.GetBlobReferenceFromServer(new Uri(_cloudBlobClient.BaseUri, uri.AbsolutePath.TrimStart('/')));
            return cloudBlob.OpenRead();
        }

        /// <summary>
        /// Open blob for write by relative or absolute url
        /// </summary>
        /// <param name="blobUrl"></param>
        /// <returns>blob stream</returns>
        public virtual Stream OpenWrite(string blobUrl)
        {
            //Container name
            var containerName = GetContainerNameFromUrl(blobUrl);
            //directory path
            var filePath = GetFilePathFromUrl(blobUrl);
            if (filePath == null)
            {
                throw new ArgumentException(@"Cannot get file path from URL", nameof(blobUrl));
            }
            var container = _cloudBlobClient.GetContainerReference(containerName);
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            var blob = container.GetBlockBlobReference(filePath);
            blob.Properties.ContentType = MimeTypeResolver.ResolveContentType(Path.GetFileName(filePath));

            // Leverage Browser Caching - 7days
            // Setting Cache-Control on Azure Blobs can help reduce bandwidth and improve the performance by preventing consumers from having to continuously download resources. 
            // More Info https://developers.google.com/speed/docs/insights/LeverageBrowserCaching
            blob.Properties.CacheControl = BlobCacheControlPropertyValue;

            return blob.OpenWrite();
        }


        public virtual void Remove(string[] urls)
        {
            foreach (var url in urls)
            {
                var absoluteUri = url.IsAbsoluteUrl() ? new Uri(url) : new Uri(_cloudBlobClient.BaseUri, url.TrimStart('/'));
                var blobContainer = GetBlobContainer(GetContainerNameFromUrl(absoluteUri.ToString()));
                var directoryPath = GetDirectoryPathFromUrl(absoluteUri.ToString());
                if (string.IsNullOrEmpty(directoryPath))
                {
                    blobContainer.DeleteIfExists();
                }
                else
                {
                    var blobDirectory = blobContainer.GetDirectoryReference(directoryPath);
                    //Remove all nested directory blobs
                    foreach (var directoryBlob in blobDirectory.ListBlobs(true).OfType<CloudBlockBlob>())
                    {
                        directoryBlob.DeleteIfExists();
                    }
                    //Remove blockBlobs if url not directory
                    /* http://stackoverflow.com/questions/29285239/delete-a-blob-from-windows-azure-in-c-sharp
                     * In Azure Storage Client Library 4.0, we changed Get*Reference methods to accept relative addresses only. */
                    var filePath = GetFilePathFromUrl(url);
                    var blobBlock = blobContainer.GetBlockBlobReference(filePath);
                    blobBlock.DeleteIfExists();
                }
            }
        }


        public virtual BlobSearchResult Search(string folderUrl, string keyword)
        {
            var retVal = new BlobSearchResult();

            if (!string.IsNullOrEmpty(folderUrl))
            {
                var blobContainer = GetBlobContainer(GetContainerNameFromUrl(folderUrl));

                if (blobContainer != null)
                {
                    var directoryPath = GetDirectoryPathFromUrl(folderUrl);
                    var blobDirectory = !string.IsNullOrEmpty(directoryPath) ? blobContainer.GetDirectoryReference(directoryPath) : null;
                    var listBlobs = blobDirectory != null ? blobDirectory.ListBlobs() : blobContainer.ListBlobs();
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        if (blobDirectory != null)
                        {
                            keyword = blobDirectory.Prefix + keyword;
                        }
                        //Only whole container list allow search by prefix
                        listBlobs = blobContainer.ListBlobs(keyword, useFlatBlobListing: true);
                    }
                    // Loop over items within the container and output the length and URI.
                    foreach (var item in listBlobs)
                    {
                        var block = item as CloudBlockBlob;
                        var directory = item as CloudBlobDirectory;
                        if (block != null)
                        {
                            var blobInfo = ConvertBlobToBlobInfo(block);
                            //Do not return empty blob (created with directory because azure blob not support direct directory creation)
                            if (!string.IsNullOrEmpty(blobInfo.FileName))
                            {
                                retVal.Items.Add(blobInfo);
                            }
                        }
                        if (directory != null)
                        {
                            var folder = new BlobFolder
                            {
                                Name = Uri.UnescapeDataString(directory.Uri.AbsolutePath).Split(new[] { _cloudBlobClient.DefaultDelimiter }, StringSplitOptions.RemoveEmptyEntries).Last(),
                                Url = Uri.EscapeUriString(directory.Uri.ToString()),
                                ParentUrl = directory.Parent != null ? Uri.EscapeUriString(directory.Parent.Uri.ToString()) : null
                            };
                            folder.RelativeUrl = folder.Url.Replace(_cloudBlobClient.BaseUri.ToString(), string.Empty);
                            retVal.Folders.Add(folder);
                        }
                    }
                }
            }
            else
            {
                foreach (var container in _cloudBlobClient.ListContainers())
                {
                    var folder = new BlobFolder
                    {
                        Name = container.Uri.AbsolutePath.Split('/').Last(),
                        Url = Uri.EscapeUriString(container.Uri.ToString())
                    };
                    retVal.Folders.Add(folder);
                }
            }
            return retVal;
        }

        public virtual void CreateFolder(BlobFolder folder)
        {
            var path = (folder.ParentUrl != null ? $"{folder.ParentUrl}/" : string.Empty) + folder.Name;
            var containerName = GetContainerNameFromUrl(path);
            var blobContainer = _cloudBlobClient.GetContainerReference(containerName);
            blobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            var directoryPath = GetDirectoryPathFromUrl(path);
            if (!string.IsNullOrEmpty(directoryPath))
            {
                //Need upload empty blob because azure blob storage not support direct directory creation
                blobContainer.GetBlockBlobReference(directoryPath).UploadText(string.Empty);
            }
        }

        public virtual void Move(string oldUrl, string newUrl)
        {
            Task.Factory.StartNew(() => MoveAsync(oldUrl, newUrl), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
        }

        public virtual void Copy(string oldUrl, string newUrl)
        {
            Task.Factory.StartNew(() => MoveAsync(oldUrl, newUrl, true), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
        }

        protected virtual async Task MoveAsync(string oldUrl, string newUrl, bool isCopy = false)
        {
            string oldPath, newPath;
            var isFolderRename = string.IsNullOrEmpty(Path.GetFileName(oldUrl));

            var moveItems = new Dictionary<string, string>();

            var containerName = GetContainerNameFromUrl(oldUrl);

            //if rename file
            if (!isFolderRename)
            {
                oldPath = GetFilePathFromUrl(oldUrl);
                newPath = GetFilePathFromUrl(newUrl);
            }
            else
            {
                oldPath = GetDirectoryPathFromUrl(oldUrl);
                newPath = GetDirectoryPathFromUrl(newUrl);
            }

            var blobContainer = _cloudBlobClient.GetContainerReference(containerName);

            var items = blobContainer.ListBlobs(oldPath, true, BlobListingDetails.All);

            foreach (var listBlobItem in items)
            {
                var blobName = isFolderRename ? listBlobItem.Uri.AbsoluteUri : listBlobItem.StorageUri.PrimaryUri.ToString();

                moveItems.Add(blobName, blobName.Replace(oldPath, newPath));
            }

            foreach (var item in moveItems)
            {
                await MoveBlob(blobContainer, item.Key, item.Value, isCopy);
            }
        }

        /// <summary>
        /// Move blob new url and remove old blob
        /// </summary>
        /// <param name="container"></param>
        /// <param name="oldUrl"></param>
        /// <param name="newUrl"></param>
        /// <param name="isCopy"></param>
        private async Task MoveBlob(CloudBlobContainer container, string oldUrl, string newUrl, bool isCopy)
        {
            var target = container.GetBlockBlobReference(GetFilePathFromUrl(newUrl));

            await container.CreateIfNotExistsAsync();

            if (!await target.ExistsAsync())
            {
                var sourse = container.GetBlockBlobReference(GetFilePathFromUrl(oldUrl));

                if (await sourse.ExistsAsync())
                {
                    await target.StartCopyAsync(sourse);
                    if (!isCopy)
                    {
                        await sourse.DeleteIfExistsAsync();
                    }
                }
            }
        }

        #endregion

        #region IBlobUrlResolver Members

        public string GetAbsoluteUrl(string blobKey)
        {
            var retVal = blobKey;
            if (!blobKey.IsAbsoluteUrl())
            {
                var baseUrl = _cloudStorageAccount.BlobEndpoint.AbsoluteUri;

                if (!string.IsNullOrWhiteSpace(_cdnUrl))
                {
                    var cdnUriBuilder = new UriBuilder(_cloudStorageAccount.BlobEndpoint.Scheme, _cdnUrl);
                    baseUrl = cdnUriBuilder.Uri.AbsoluteUri;
                }

                retVal = baseUrl.TrimEnd('/') + "/" + blobKey.TrimStart('/');
            }
            return retVal;
        }

        #endregion
        /// <summary>
        /// Return outline folder from absolute or relative url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string[] GetOutlineFromUrl(string url)
        {
            var relativeUrl = url;
            if (url.IsAbsoluteUrl())
            {
                relativeUrl = Uri.UnescapeDataString(new Uri(url).AbsolutePath);
            }
            return relativeUrl.Split(new[] { "/", "\\", _cloudBlobClient.DefaultDelimiter }, StringSplitOptions.RemoveEmptyEntries);
        }

        private string GetContainerNameFromUrl(string url)
        {
            return GetOutlineFromUrl(url).First();
        }

        private string GetDirectoryPathFromUrl(string url)
        {
            var retVal = string.Join(_cloudBlobClient.DefaultDelimiter, GetOutlineFromUrl(url).Skip(1).ToArray());
            return !string.IsNullOrEmpty(retVal) ? retVal + _cloudBlobClient.DefaultDelimiter : null;
        }
        private string GetFilePathFromUrl(string url)
        {
            var retVal = string.Join(_cloudBlobClient.DefaultDelimiter, GetOutlineFromUrl(url).Skip(1).ToArray());
            return !string.IsNullOrEmpty(retVal) ? retVal : null;
        }

        private CloudBlobContainer GetBlobContainer(string name)
        {
            CloudBlobContainer retVal = null;
            var container = _cloudBlobClient.GetContainerReference(name);
            if (container.Exists())
            {
                retVal = container;
            }
            return retVal;
        }

        private BlobInfo ConvertBlobToBlobInfo(ICloudBlob cloudBlob)
        {
            var relativeUrl = cloudBlob.Uri.LocalPath;
            var absoluteUrl = GetAbsoluteUrl(cloudBlob.Uri.PathAndQuery);
            var fileName = Path.GetFileName(Uri.UnescapeDataString(cloudBlob.Uri.ToString()));
            var contentType = MimeTypeResolver.ResolveContentType(fileName);

            return new BlobInfo
            {
                Url = absoluteUrl,
                FileName = fileName,
                ContentType = contentType,
                Size = cloudBlob.Properties.Length,
                ModifiedDate = cloudBlob.Properties.LastModified?.DateTime,
                RelativeUrl = relativeUrl
            };
        }

        private static CloudStorageAccount ParseConnectionString(string connectionString)
        {
            CloudStorageAccount cloudStorageAccount;
            if (!CloudStorageAccount.TryParse(connectionString, out cloudStorageAccount))
            {
                throw new InvalidOperationException("Failed to get valid connection string");
            }
            return cloudStorageAccount;
        }

    }
}
