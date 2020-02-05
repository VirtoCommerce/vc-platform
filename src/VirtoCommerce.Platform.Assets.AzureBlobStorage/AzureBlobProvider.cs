using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Assets.AzureBlobStorage
{
    public class AzureBlobProvider : IBlobStorageProvider, IBlobUrlResolver
    {

        public const string ProviderName = "AzureBlobStorage";
        public const string BlobCacheControlPropertyValue = "public, max-age=604800";

        private readonly CloudBlobClient _cloudBlobClient;
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly string _cdnUrl;

        public AzureBlobProvider(IOptions<AzureBlobOptions> options)
        {
            _cloudStorageAccount = ParseConnectionString(options.Value.ConnectionString);
            _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            _cdnUrl = options.Value.CdnUrl;
        }

        #region IBlobStorageProvider Members

        /// <summary>
        /// Get blob info by url
        /// </summary>
        /// <param name="blobUrl"></param>
        /// <returns></returns>
        public virtual async Task<BlobInfo> GetBlobInfoAsync(string blobUrl)
        {
            if (string.IsNullOrEmpty(blobUrl))
                throw new ArgumentNullException(nameof(blobUrl));

            var uri = blobUrl.IsAbsoluteUrl() ? new Uri(blobUrl) : new Uri(_cloudBlobClient.BaseUri, blobUrl.TrimStart('/'));
            BlobInfo retVal = null;
            try
            {
                var cloudBlob = await _cloudBlobClient.GetBlobReferenceFromServerAsync(uri);
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
            var cloudBlob = _cloudBlobClient
                .GetBlobReferenceFromServerAsync(new Uri(_cloudBlobClient.BaseUri, uri.AbsolutePath.TrimStart('/')))
                .Result;
            return cloudBlob.OpenReadAsync(null, null, null).Result;
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
            container.CreateIfNotExistsAsync();

            var blob = container.GetBlockBlobReference(filePath);

            blob.Properties.ContentType = MimeTypeResolver.ResolveContentType(Path.GetFileName(filePath));

            // Leverage Browser Caching - 7days
            // Setting Cache-Control on Azure Blobs can help reduce bandwidth and improve the performance by preventing consumers from having to continuously download resources. 
            // More Info https://developers.google.com/speed/docs/insights/LeverageBrowserCaching
            blob.Properties.CacheControl = BlobCacheControlPropertyValue;

            return blob.OpenWriteAsync().Result;
        }

        public virtual async Task RemoveAsync(string[] urls)
        {
            foreach (var url in urls)
            {
                var absoluteUri = url.IsAbsoluteUrl()
                    ? new Uri(url)
                    : new Uri(_cloudBlobClient.BaseUri, url.TrimStart('/'));
                var blobContainer = GetBlobContainer(GetContainerNameFromUrl(absoluteUri.ToString()));
                var directoryPath = GetDirectoryPathFromUrl(absoluteUri.ToString());
                if (string.IsNullOrEmpty(directoryPath))
                {
                    await blobContainer.DeleteIfExistsAsync();
                }
                else
                {
                    var blobDirectory = blobContainer.GetDirectoryReference(directoryPath);
                    //Remove all nested directory blobs
                    foreach (var directoryBlob in blobDirectory.ListBlobsSegmentedAsync(null).Result.Results)
                    {
                        await directoryBlob.Container.DeleteIfExistsAsync();
                    }

                    //Remove blockBlobs if url not directory
                    /* http://stackoverflow.com/questions/29285239/delete-a-blob-from-windows-azure-in-c-sharp
                     * In Azure Storage Client Library 4.0, we changed Get*Reference methods to accept relative addresses only. */
                    var filePath = GetFilePathFromUrl(url);
                    var blobBlock = blobContainer.GetBlockBlobReference(filePath);
                    await blobBlock.DeleteIfExistsAsync();
                }
            }
        }

        public virtual async Task<BlobEntrySearchResult> SearchAsync(string folderUrl, string keyword)
        {
            var retVal = AbstractTypeFactory<BlobEntrySearchResult>.TryCreateInstance();

            if (!string.IsNullOrEmpty(folderUrl))
            {
                var blobContainer = GetBlobContainer(GetContainerNameFromUrl(folderUrl));

                if (blobContainer != null)
                {
                    BlobContinuationToken blobContinuationToken = null;

                    var directoryPath = GetDirectoryPathFromUrl(folderUrl);
                    var blobDirectory = !string.IsNullOrEmpty(directoryPath)
                        ? blobContainer.GetDirectoryReference(directoryPath)
                        : null;
                    var listBlobs = blobDirectory != null
                        ? await blobDirectory.ListBlobsSegmentedAsync(blobContinuationToken)
                        : await blobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        if (blobDirectory != null)
                        {
                            keyword = blobDirectory.Prefix + keyword;
                        }

                        BlobContinuationToken prefixBlobContinuationToken = null;
                        //Only whole container list allow search by prefix
                        listBlobs = await blobContainer.ListBlobsSegmentedAsync(keyword, prefixBlobContinuationToken);
                    }

                    // Loop over items within the container and output the length and URI.
                    foreach (IListBlobItem item in listBlobs.Results)
                    {
                        var block = item as CloudBlockBlob;
                        var directory = item as CloudBlobDirectory;
                        if (block != null)
                        {
                            var blobInfo = ConvertBlobToBlobInfo(block);
                            //Do not return empty blob (created with directory because azure blob not support direct directory creation)
                            if (!string.IsNullOrEmpty(blobInfo.Name))
                            {
                                retVal.Results.Add(blobInfo);
                            }
                        }

                        if (directory != null)
                        {
                            var folder = AbstractTypeFactory<BlobFolder>.TryCreateInstance();

                            folder.Name = Uri.UnescapeDataString(directory.Uri.AbsolutePath)
                                .Split(new[] { _cloudBlobClient.DefaultDelimiter },
                                    StringSplitOptions.RemoveEmptyEntries).Last();
                            folder.Url = Uri.EscapeUriString(directory.Uri.ToString());
                            folder.ParentUrl = directory.Parent != null
                                ? Uri.EscapeUriString(directory.Parent.Uri.ToString())
                                : null;
                            folder.RelativeUrl = folder.Url.Replace(_cloudBlobClient.BaseUri.ToString(), string.Empty);
                            retVal.Results.Add(folder);
                        }
                    }
                }
            }
            else
            {
                BlobContinuationToken listbContinuationToken = null;

                do
                {
                    var results = await _cloudBlobClient.ListContainersSegmentedAsync(null, listbContinuationToken);

                    listbContinuationToken = results.ContinuationToken;
                    foreach (var item in results.Results)
                    {
                        var folder = AbstractTypeFactory<BlobFolder>.TryCreateInstance();

                        folder.Name = item.Uri.AbsolutePath.Split('/').Last();
                        folder.Url = Uri.EscapeUriString(item.Uri.ToString());

                        retVal.Results.Add(folder);
                    }
                } while (listbContinuationToken != null);

            }

            retVal.TotalCount = retVal.Results.Count();

            return retVal;
        }

        public virtual async Task CreateFolderAsync(BlobFolder folder)
        {
            var path = (folder.ParentUrl != null ? $"{folder.ParentUrl}/" : string.Empty) + folder.Name;

            var containerName = GetContainerNameFromUrl(path);
            var blobContainer = _cloudBlobClient.GetContainerReference(containerName);
            await blobContainer.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Blob, null, null);

            var directoryPath = GetDirectoryPathFromUrl(path);
            if (!string.IsNullOrEmpty(directoryPath))
            {
                //Need upload empty blob because azure blob storage not support direct directory creation
                await blobContainer.GetBlockBlobReference(directoryPath).UploadFromByteArrayAsync(new byte[0], 0, 0);
            }

        }

        public virtual void Move(string oldUrl, string newUrl)
        {
            MoveAsync(oldUrl, newUrl).GetAwaiter().GetResult();
        }

        public virtual void Copy(string oldUrl, string newUrl)
        {
            MoveAsync(oldUrl, newUrl, true).GetAwaiter().GetResult();
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

            var items = blobContainer
                .ListBlobsSegmentedAsync(oldPath, true, BlobListingDetails.All, null, null, null, null).Result;

            foreach (var listBlobItem in items.Results)
            {
                var blobName = isFolderRename
                    ? listBlobItem.Uri.AbsoluteUri
                    : listBlobItem.StorageUri.PrimaryUri.ToString();

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

            return relativeUrl.Split(new[] { "/", "\\", _cloudBlobClient.DefaultDelimiter },
                StringSplitOptions.RemoveEmptyEntries);
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
            // Retrieve container reference.
            var container = _cloudBlobClient.GetContainerReference(name);
            if (container.ExistsAsync().Result)
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
                Name = fileName,
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
