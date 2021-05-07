using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Extensions;
using BlobInfo = VirtoCommerce.Platform.Core.Assets.BlobInfo;

namespace VirtoCommerce.Platform.Assets.AzureBlobStorage
{
    public class AzureBlobProvider : IBlobStorageProvider, IBlobUrlResolver
    {
        public const string ProviderName = "AzureBlobStorage";
        public const string BlobCacheControlPropertyValue = "public, max-age=604800";
        private const string Delimiter = "/";
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _cdnUrl;

        public AzureBlobProvider(IOptions<AzureBlobOptions> options)
        {
            _blobServiceClient = new BlobServiceClient(options.Value.ConnectionString);
            _cdnUrl = options.Value.CdnUrl;
        }

        #region IBlobStorageProvider Members

        /// <summary>
        /// Get blob info by URL
        /// </summary>
        /// <param name="blobUrl">Absolute or relative URL to get blob</param>
        public virtual async Task<BlobInfo> GetBlobInfoAsync(string blobUrl)
        {
            if (string.IsNullOrEmpty(blobUrl))
                throw new ArgumentNullException(nameof(blobUrl));

            var uri = blobUrl.IsAbsoluteUrl() ? new Uri(blobUrl) : new Uri(_blobServiceClient.Uri, blobUrl.TrimStart(Delimiter[0]));
            BlobInfo result = null;
            try
            {
                var blob = new BlobClient(new Uri(_blobServiceClient.Uri, uri.AbsolutePath.TrimStart('/')));
                var props = await blob.GetPropertiesAsync();
                result = ConvertBlobToBlobInfo(blob, props.Value);
            }
            catch
            {
                // Since the storage account is based on transaction volume, it is better to handle the 404 (BlobNotFound) exception because that is just one api call, as opposed to checking the BlobClient.ExistsAsync() first and then making the BlobClient.DownloadAsync() call (2 api transactions).
                //https://elcamino.cloud/articles/2020-03-30-azure-storage-blobs-net-sdk-v12-upgrade-guide-and-tips.html
            }

            return result;
        }

        /// <summary>
        /// Open stream for read blob by relative or absolute url
        /// </summary>
        /// <param name="blobUrl"></param>
        /// <returns>blob stream</returns>
        public virtual Stream OpenRead(string blobUrl)
        {
            return OpenReadAsync(blobUrl).GetAwaiter().GetResult();
        }

        public virtual Task<Stream> OpenReadAsync(string blobUrl)
        {
            if (string.IsNullOrEmpty(blobUrl))
            {
                throw new ArgumentNullException(nameof(blobUrl));
            }

            var uri = blobUrl.IsAbsoluteUrl() ? new Uri(blobUrl) : new Uri(_blobServiceClient.Uri, blobUrl.TrimStart(Delimiter[0]));
            var blob = new BlobClient(new Uri(_blobServiceClient.Uri, uri.AbsolutePath.TrimStart('/')));

            return blob.OpenReadAsync();
        }

        /// <summary>
        /// Open blob for write by relative or absolute url
        /// </summary>
        /// <param name="blobUrl"></param>
        /// <returns>blob stream</returns>
        public virtual Stream OpenWrite(string blobUrl)
        {
            return OpenWriteAsync(blobUrl).GetAwaiter().GetResult();
        }

        public virtual async Task<Stream> OpenWriteAsync(string blobUrl)
        {
            var filePath = GetFilePathFromUrl(blobUrl);
            if (filePath == null)
            {
                throw new ArgumentException(@"Cannot get file path from URL", nameof(blobUrl));
            }

            var container = _blobServiceClient.GetBlobContainerClient(GetContainerNameFromUrl(blobUrl));
            await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blob = container.GetBlockBlobClient(filePath);

            var options = new BlockBlobOpenWriteOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = MimeTypeResolver.ResolveContentType(Path.GetFileName(filePath)),
                    // Leverage Browser Caching - 7days
                    // Setting Cache-Control on Azure Blobs can help reduce bandwidth and improve the performance by preventing consumers from having to continuously download resources.
                    // More Info https://developers.google.com/speed/docs/insights/LeverageBrowserCaching
                    CacheControl = BlobCacheControlPropertyValue
                }
            };

            return await blob.OpenWriteAsync(true, options);
        }

        public virtual async Task RemoveAsync(string[] urls)
        {
            foreach (var url in urls)
            {
                var absoluteUri = url.IsAbsoluteUrl()
                    ? new Uri(url).ToString()
                    : UrlHelperExtensions.Combine(_blobServiceClient.Uri.ToString(), url);
                var blobContainer = GetBlobContainer(GetContainerNameFromUrl(absoluteUri));

                var isFolder = string.IsNullOrEmpty(Path.GetFileName(absoluteUri));
                var blobSearchPrefix = isFolder ? GetDirectoryPathFromUrl(absoluteUri)
                                             : GetFilePathFromUrl(absoluteUri);

                if (string.IsNullOrEmpty(blobSearchPrefix))
                {
                    await blobContainer.DeleteIfExistsAsync();
                }
                else
                {
                    var blobItems = blobContainer.GetBlobsAsync(prefix: blobSearchPrefix);
                    await foreach (var blobItem in blobItems)
                    {
                        var blobClient = blobContainer.GetBlobClient(blobItem.Name);
                        await blobClient.DeleteIfExistsAsync();
                    }
                }
            }
        }

        public virtual async Task<BlobEntrySearchResult> SearchAsync(string folderUrl, string keyword)
        {
            var result = AbstractTypeFactory<BlobEntrySearchResult>.TryCreateInstance();

            if (!string.IsNullOrEmpty(folderUrl))
            {
                var container = GetBlobContainer(GetContainerNameFromUrl(folderUrl));

                if (container != null)
                {
                    var baseUriEscaped = EscapeUri(container.Uri.AbsoluteUri);
                    var prefix = GetDirectoryPathFromUrl(folderUrl);
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        //Only whole container list allow search by prefix
                        prefix += keyword;
                    }

                    // Call the listing operation and return pages of the specified size.
                    var resultSegment = container.GetBlobsByHierarchyAsync(prefix: prefix, delimiter: Delimiter)
                        .AsPages();

                    // Enumerate the blobs returned for each page.
                    await foreach (var blobPage in resultSegment)
                    {
                        // A hierarchical listing may return both virtual directories and blobs.
                        foreach (var blobhierarchyItem in blobPage.Values)
                        {
                            if (blobhierarchyItem.IsPrefix)
                            {
                                var folder = AbstractTypeFactory<BlobFolder>.TryCreateInstance();

                                // No Unescaping for Name. Unescaping a string that has been previously unescaped can lead to ambiguities and errors.
                                folder.Name = blobhierarchyItem.Prefix
                                   .Split(new[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries)
                                   .Last();

                                folder.Url = UrlHelperExtensions.Combine(baseUriEscaped, EscapeUri(blobhierarchyItem.Prefix));
                                folder.ParentUrl = GetParentUrl(baseUriEscaped, blobhierarchyItem.Prefix);
                                folder.RelativeUrl = folder.Url.Replace(EscapeUri(_blobServiceClient.Uri.ToString()), string.Empty);
                                result.Results.Add(folder);
                            }
                            else
                            {
                                var blobInfo = ConvertBlobToBlobInfo(blobhierarchyItem.Blob, baseUriEscaped);
                                //Do not return empty blob (created with directory because azure blob not support direct directory creation)
                                if (!string.IsNullOrEmpty(blobInfo.Name))
                                {
                                    result.Results.Add(blobInfo);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                // Call the listing operation and enumerate the result segment.
                var resultSegment = _blobServiceClient.GetBlobContainersAsync().AsPages();

                await foreach (var containerPage in resultSegment)
                {
                    foreach (var item in containerPage.Values)
                    {
                        var folder = AbstractTypeFactory<BlobFolder>.TryCreateInstance();
                        folder.Name = item.Name.Split(Delimiter).Last();
                        folder.Url = EscapeUri(UrlHelperExtensions.Combine(_blobServiceClient.Uri.ToString(), item.Name));
                        result.Results.Add(folder);
                    }
                }
            }

            result.TotalCount = result.Results.Count();
            return result;
        }

        public virtual async Task CreateFolderAsync(BlobFolder folder)
        {
            var path = folder.ParentUrl == null ?
                        folder.Name :
                        UrlHelperExtensions.Combine(folder.ParentUrl, folder.Name);

            var containerName = GetContainerNameFromUrl(path);
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var directoryPath = GetDirectoryPathFromUrl(path);
            if (!string.IsNullOrEmpty(directoryPath))
            {
                //Need upload empty blob because azure blob storage not support direct directory creation
                using var stream = new MemoryStream(new byte[0]);
                await container.GetBlockBlobClient(directoryPath).UploadAsync(stream);
            }
        }

        public virtual void Move(string srcUrl, string destUrl)
        {
            MoveAsync(srcUrl, destUrl).GetAwaiter().GetResult();
        }

        public virtual Task MoveAsyncPublic(string srcUrl, string destUrl)
        {
            return MoveAsync(srcUrl, destUrl);
        }

        public virtual void Copy(string srcUrl, string destUrl)
        {
            MoveAsync(srcUrl, destUrl, true).GetAwaiter().GetResult();
        }

        public virtual Task CopyAsync(string srcUrl, string destUrl)
        {
            return MoveAsync(srcUrl, destUrl, true);
        }

        protected virtual async Task MoveAsync(string oldUrl, string newUrl, bool isCopy = false)
        {
            string oldPath, newPath;
            var isFolderRename = string.IsNullOrEmpty(Path.GetFileName(oldUrl));

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

            var taskList = new List<Task>();
            var blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobItems = blobContainer.GetBlobsAsync(prefix: oldPath);

            await foreach (var blobItem in blobItems)
            {
                var blobName = UrlHelperExtensions.Combine(containerName, blobItem.Name);
                var newBlobName = blobName.Replace(oldPath, newPath);

                taskList.Add(MoveBlob(blobContainer, blobName, newBlobName, isCopy));
            }

            await Task.WhenAll(taskList);
        }

        /// <summary>
        /// Move blob new URL and remove old blob
        /// </summary>
        /// <param name="container"></param>
        /// <param name="oldUrl"></param>
        /// <param name="newUrl"></param>
        /// <param name="isCopy"></param>
        private async Task MoveBlob(BlobContainerClient container, string oldUrl, string newUrl, bool isCopy)
        {
            var targetPath = newUrl.EndsWith(Delimiter)
                ? GetDirectoryPathFromUrl(newUrl)
                : GetFilePathFromUrl(newUrl);

            var target = container.GetBlockBlobClient(targetPath);

            if (!await target.ExistsAsync())
            {
                var soursePath = oldUrl.EndsWith(Delimiter)
                    ? GetDirectoryPathFromUrl(oldUrl)
                    : GetFilePathFromUrl(oldUrl);

                var sourceBlob = container.GetBlockBlobClient(soursePath);

                if (await sourceBlob.ExistsAsync())
                {
                    await target.StartCopyFromUri(sourceBlob.Uri).WaitForCompletionAsync();

                    if (!isCopy)
                    {
                        await sourceBlob.DeleteIfExistsAsync();
                    }
                }
            }
        }

        #endregion IBlobStorageProvider Members

        #region IBlobUrlResolver Members

        public string GetAbsoluteUrl(string blobKey)
        {
            var result = blobKey;
            if (!blobKey.IsAbsoluteUrl())
            {
                var baseUrl = _blobServiceClient.Uri.AbsoluteUri;

                if (!string.IsNullOrWhiteSpace(_cdnUrl))
                {
                    var cdnUriBuilder = new UriBuilder(_blobServiceClient.Uri.Scheme, _cdnUrl);
                    baseUrl = cdnUriBuilder.Uri.AbsoluteUri;
                }

                result = UrlHelperExtensions.Combine(baseUrl, EscapeUri(blobKey));
            }

            return result;
        }

        #endregion IBlobUrlResolver Members

        /// <summary>
        /// Return outline folder from absolute or relative URL
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

            return relativeUrl.Split(new[] { Delimiter, "\\" },
                StringSplitOptions.RemoveEmptyEntries);
        }

        private string GetContainerNameFromUrl(string url)
        {
            return GetOutlineFromUrl(url).First();
        }

        private string GetDirectoryPathFromUrl(string url)
        {
            var result = string.Join(Delimiter, GetOutlineFromUrl(url).Skip(1).ToArray());
            return !string.IsNullOrEmpty(result) ? result + Delimiter : null;
        }

        private string GetFilePathFromUrl(string url)
        {
            var result = string.Join(Delimiter, GetOutlineFromUrl(url).Skip(1).ToArray());
            return !string.IsNullOrEmpty(result) ? result : null;
        }

        private string GetParentUrl(string baseUri, string blobPrefix)
        {
            var segments = GetOutlineFromUrl(blobPrefix);
            var parentPath = string.Join(Delimiter, segments.Take(segments.Length - 1));
            return UrlHelperExtensions.Combine(baseUri, EscapeUri(parentPath));
        }

        private static string EscapeUri(string stringToEscape)
        {
            return Uri.EscapeUriString(stringToEscape);
        }

        private BlobContainerClient GetBlobContainer(string name)
        {
            BlobContainerClient result = null;
            // Retrieve container reference.
            var container = _blobServiceClient.GetBlobContainerClient(name);
            if (container.ExistsAsync().Result)
            {
                result = container;
            }

            return result;
        }

        private BlobInfo ConvertBlobToBlobInfo(BlobClient blob, BlobProperties props)
        {
            var absoluteUrl = blob.Uri;
            var relativeUrl = UrlHelperExtensions.Combine(GetContainerNameFromUrl(blob.Uri.ToString()), EscapeUri(blob.Name));
            var fileName = Path.GetFileName(Uri.UnescapeDataString(blob.Name));
            var contentType = MimeTypeResolver.ResolveContentType(fileName);

            return new BlobInfo
            {
                Url = absoluteUrl.ToString(),
                Name = fileName,
                ContentType = contentType,
                Size = props.ContentLength,
                ModifiedDate = props.LastModified.DateTime,
                RelativeUrl = relativeUrl
            };
        }

        private BlobInfo ConvertBlobToBlobInfo(BlobItem blob, string baseUri)
        {
            var absoluteUrl = UrlHelperExtensions.Combine(baseUri, EscapeUri(blob.Name));
            var relativeUrl = absoluteUrl.Replace(EscapeUri(_blobServiceClient.Uri.ToString()), string.Empty);
            var fileName = Path.GetFileName(blob.Name);
            var contentType = MimeTypeResolver.ResolveContentType(fileName);

            return new BlobInfo
            {
                Url = absoluteUrl,
                Name = fileName,
                ContentType = contentType,
                Size = blob.Properties.ContentLength ?? 0,
                ModifiedDate = blob.Properties.LastModified?.DateTime,
                RelativeUrl = relativeUrl
            };
        }
    }
}
