using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Asset
{
    public class AzureBlobProvider : IBlobStorageProvider, IBlobUrlResolver
    {
        public const string ProviderName = "AzureBlobStorage";
        public const string DefaultBlobContainerName = "default-container";

        private readonly string _connectionString;
        private readonly CloudBlobClient _cloudBlobClient;
        private CloudStorageAccount _cloudStorageAccount;

        public AzureBlobProvider(string connectionString)
        {
            _connectionString = connectionString;
            _cloudStorageAccount = ParseConnectionString(connectionString);
            _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
        }

        #region IBlobStorageProvider Members
        /// <summary>
        /// Get blog info by url
        /// </summary>
        /// <param name="blobUrl"></param>
        /// <returns></returns>
        public virtual BlobInfo GetBlobInfo(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            var uri = url.IsAbsoluteUrl() ? new Uri(url) : new Uri(_cloudBlobClient.BaseUri, url.TrimStart('/'));
            var cloudBlob = _cloudBlobClient.GetBlobReferenceFromServer(uri);
            var retVal = new BlobInfo
            {
                Url = Uri.EscapeUriString(cloudBlob.Uri.ToString()),
                FileName = Path.GetFileName(Uri.UnescapeDataString(cloudBlob.Uri.ToString())),
                ContentType = cloudBlob.Properties.ContentType,
                Size = cloudBlob.Properties.Length,
                ModifiedDate = cloudBlob.Properties.LastModified != null ? cloudBlob.Properties.LastModified.Value.DateTime : (DateTime?)null
            };
            return retVal;
        }

        /// <summary>
        /// Open blob for read by relative or absolute url
        /// </summary>
        /// <param name="url"></param>
        /// <returns>blob stream</returns>
        public virtual Stream OpenRead(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            var uri = url.IsAbsoluteUrl() ? new Uri(url) : new Uri(_cloudBlobClient.BaseUri, url.TrimStart('/'));
            var cloudBlob = _cloudBlobClient.GetBlobReferenceFromServer(uri);
            return cloudBlob.OpenRead();
        }

        /// <summary>
        /// Open blob for write by relative or absolute url
        /// </summary>
        /// <param name="url"></param>
        /// <returns>blob stream</returns>
        public virtual Stream OpenWrite(string url)
        {
            //Container name
            var containerName = GetContainerNameFromUrl(url);
            //directory path
            var filePath = GetFilePathFromUrl(url);
            if (filePath == null)
            {
                throw new NullReferenceException("filePath");
            }
            var container = _cloudBlobClient.GetContainerReference(containerName);
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);

            var blob = container.GetBlockBlobReference(filePath);

            blob.Properties.ContentType = MimeTypeResolver.ResolveContentType(Path.GetFileName(filePath));

            return blob.OpenWrite();
        }


        public virtual void Remove(string[] urls)
        {
            foreach (var url in urls)
            {
                var absoluteUri = url.IsAbsoluteUrl() ? new Uri(url) : new Uri(_cloudBlobClient.BaseUri, url.TrimStart('/'));
                var blobContainer = GetBlobContainer(GetContainerNameFromUrl(absoluteUri.ToString()));
                var directoryPath = GetDirectoryPathFromUrl(absoluteUri.ToString());
                if (String.IsNullOrEmpty(directoryPath))
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
                    var blobBlock = blobContainer.GetBlockBlobReference(absoluteUri.ToString());
                    blobBlock.DeleteIfExists();
                }
            }
        }


        public virtual BlobSearchResult Search(string folderUrl, string keyword)
        {
            var retVal = new BlobSearchResult();

            if (!String.IsNullOrEmpty(folderUrl))
            {
                var blobContainer = GetBlobContainer(GetContainerNameFromUrl(folderUrl));

                if (blobContainer != null)
                {
                    var directoryPath = GetDirectoryPathFromUrl(folderUrl);
                    var blobDirectory = !String.IsNullOrEmpty(directoryPath) ? blobContainer.GetDirectoryReference(directoryPath) : null;
                    var listBlobs = blobDirectory != null ? blobDirectory.ListBlobs() : blobContainer.ListBlobs();
                    if (!String.IsNullOrEmpty(keyword))
                    {
                        listBlobs = blobContainer.ListBlobs(keyword, useFlatBlobListing: true);
                    }
                    // Loop over items within the container and output the length and URI.
                    foreach (IListBlobItem item in listBlobs)
                    {
                        var block = item as CloudBlockBlob;
                        var directory = item as CloudBlobDirectory;
                        if (block != null)
                        {
                            var blobInfo = new BlobInfo
                            {
                                Url = Uri.EscapeUriString(block.Uri.ToString()),
                                FileName = Path.GetFileName(Uri.UnescapeDataString(block.Uri.ToString())),
                                ContentType = block.Properties.ContentType,
                                Size = block.Properties.Length,
                                ModifiedDate = block.Properties.LastModified != null ? block.Properties.LastModified.Value.DateTime : (DateTime?)null
                            };
                            blobInfo.RelativeUrl = blobInfo.Url.Replace(_cloudBlobClient.BaseUri.ToString(), string.Empty);
                            //Do not return empty blob (created with directory because azure blob not support direct directory creation)
                            if (!String.IsNullOrEmpty(blobInfo.FileName))
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
            var path = (folder.ParentUrl != null ? folder.ParentUrl + "/" : String.Empty) + folder.Name;

            var containerName = GetContainerNameFromUrl(path);
            var blobContainer = _cloudBlobClient.GetContainerReference(containerName);
            blobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Blob);

            var directoryPath = GetDirectoryPathFromUrl(path);
            if (!String.IsNullOrEmpty(directoryPath))
            {
                //Need upload empty blob because azure blob storage not support direct directory creation
                blobContainer.GetBlockBlobReference(directoryPath).UploadText(String.Empty);
            }
        }

        #endregion

        #region IBlobUrlResolver Members

        public string GetAbsoluteUrl(string relativeUrl)
        {
            var retVal = relativeUrl;
            if (!relativeUrl.IsAbsoluteUrl())
            {
                var baseUrl = _cloudStorageAccount.BlobEndpoint.AbsoluteUri;
                retVal = baseUrl.TrimEnd('/') + "/" + relativeUrl;
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
            var retVal = String.Join(_cloudBlobClient.DefaultDelimiter, GetOutlineFromUrl(url).Skip(1).ToArray());
            return !String.IsNullOrEmpty(retVal) ? retVal + _cloudBlobClient.DefaultDelimiter : null;
        }
        private string GetFilePathFromUrl(string url)
        {
            var retVal = String.Join(_cloudBlobClient.DefaultDelimiter, GetOutlineFromUrl(url).Skip(1).ToArray());
            return !String.IsNullOrEmpty(retVal) ? retVal : null;
        }

        private CloudBlobContainer GetBlobContainer(string name)
        {
            CloudBlobContainer retVal = null;
            // Retrieve container reference.
            var container = _cloudBlobClient.GetContainerReference(name);
            if (container.Exists())
            {
                retVal = container;
            }
            return retVal;
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
