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

        public string Upload(UploadStreamInfo request)
        {
            string result = null;
            var directoryPath = DefaultBlobContainerName;
            if (!string.IsNullOrEmpty(request.FolderName))
            {
                directoryPath = request.FolderName;
            }
            //Container name
            var containerName = GetContainerNameFromUrl(directoryPath);
            //directory path
            directoryPath = GetDirectoryPathFromUrl(directoryPath);

            var container = _cloudBlobClient.GetContainerReference(containerName);
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);

            ICloudBlob blob = null;
            if (String.IsNullOrEmpty(directoryPath))
            {
                blob = container.GetBlockBlobReference(request.FileName);
            }
            else
            {
                directoryPath += directoryPath.EndsWith(_cloudBlobClient.DefaultDelimiter) ? request.FileName : _cloudBlobClient.DefaultDelimiter + request.FileName;
                blob = container.GetBlockBlobReference(directoryPath);
            }

            blob.Properties.ContentType = MimeTypeResolver.ResolveContentType(request.FileName);

            using (var memoryStream = new MemoryStream())
            {
                // upload to MemoryStream
                //memoryStream.SetLength(request.Length);
                request.FileByteStream.CopyTo(memoryStream);
                memoryStream.Position = 0;
                // fill blob
                blob.UploadFromStream(memoryStream);
            }
            result = blob.Uri.AbsolutePath.TrimStart('/');
            return result;
        }


        public System.IO.Stream OpenReadOnly(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            System.IO.Stream retVal = null;
            var uri = url.IsAbsoluteUrl() ? new Uri(url) : new Uri(_cloudBlobClient.BaseUri, url.TrimStart('/'));
            var cloudBlob = _cloudBlobClient.GetBlobReferenceFromServer(uri);
            if (cloudBlob.Exists())
            {
                var stream = new MemoryStream();
                cloudBlob.DownloadToStream(stream);
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }
                retVal = stream;
            }
            return retVal;
        }

        public void Remove(string[] urls)
        {
            foreach (var url in urls)
            {
                var blobContainer = GetBlobContainer(GetContainerNameFromUrl(url));
                var directoryPath = GetDirectoryPathFromUrl(url);
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
                    var blobBlock = blobContainer.GetBlockBlobReference(url);
                    blobBlock.DeleteIfExists();
                }
            }
        }


        public BlobSearchResult Search(string folderUrl, string keyword)
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
                        listBlobs =  blobContainer.ListBlobs(keyword, useFlatBlobListing: true);
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
                                ModifiedDate = block.Properties.LastModified != null ? block.Properties.LastModified.Value.DateTime : (DateTime?) null
                            };
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

        public void CreateFolder(BlobFolder folder)
        {
            var path = (folder.ParentUrl != null ? folder.ParentUrl + "/" : String.Empty ) + folder.Name;

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
