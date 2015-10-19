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
            var containerName = request.FolderName;

            var container = _cloudBlobClient.GetContainerReference(containerName);
            if (!container.Exists())
            {
                container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            }

            var blob = container.GetBlockBlobReference(request.FileName);
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


        public System.IO.Stream OpenReadOnly(string blobKey)
        {
            if (string.IsNullOrEmpty(blobKey))
                throw new ArgumentNullException("blobKey");

            System.IO.Stream retVal = null;
            var cloudBlob = _cloudBlobClient.GetBlobReferenceFromServer(new Uri(_cloudBlobClient.BaseUri, blobKey));
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
                var blobDirectory = !String.IsNullOrEmpty(directoryPath) ? blobContainer.GetDirectoryReference(directoryPath) : null;

                if (blobDirectory == null)
                {
                    blobContainer.DeleteIfExists();
                }
                else
                {
                    foreach (var blob in blobDirectory.ListBlobs(true).OfType<CloudBlockBlob>())
                    {
                        blob.Delete();
                    }
    
                }
            }
        }


        public BlobSearchResult Search(string folderUrl)
        {
            var retVal = new BlobSearchResult();

            if (!String.IsNullOrEmpty(folderUrl))
            {
                var blobContainer = GetBlobContainer(GetContainerNameFromUrl(folderUrl));

                if (blobContainer != null)
                {
                    var directoryPath = GetDirectoryPathFromUrl(folderUrl);
                    var blobDirectory = !String.IsNullOrEmpty(directoryPath) ? blobContainer.GetDirectoryReference(directoryPath) : null;
                    // Loop over items within the container and output the length and URI.
                    foreach (IListBlobItem item in blobDirectory != null ? blobDirectory.ListBlobs() : blobContainer.ListBlobs())
                    {
                        var block = item as CloudBlockBlob;
                        var directory = item as CloudBlobDirectory;
                        if (block != null)
                        {
                            var blobInfo = new BlobInfo
                            {
                                Url = block.Uri.ToString(),
                                FileName = Path.GetFileName(block.Uri.ToString()),
                                ContentType = block.Properties.ContentType,
                                Size = block.Properties.Length
                            };
                            retVal.Items.Add(blobInfo);
                        }
                        if (directory != null)
                        {
                            var folder = new BlobFolder
                            {
                                Name = directory.Uri.AbsolutePath.Split(new[] { _cloudBlobClient.DefaultDelimiter }, StringSplitOptions.RemoveEmptyEntries).Last(),
                                Url = directory.Uri.ToString(),
                                ParentUrl = directory.Parent != null ? directory.Parent.Uri.ToString() : null
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
                        Url = container.Uri.ToString()
                    };
                    retVal.Folders.Add(folder);
                }
            }
            return retVal;
        }

        public void CreateFolder(BlobFolder folder)
        {
            if (String.IsNullOrEmpty(folder.ParentUrl))
            {
                var container = _cloudBlobClient.GetContainerReference(folder.Name.ToLower());
                container.CreateIfNotExists();
            }
            else
            {
                var containerName = GetContainerNameFromUrl(folder.ParentUrl);
                var blobContainer = GetBlobContainer(containerName);
                if (blobContainer == null)
                {
                    throw new DirectoryNotFoundException(containerName);
                }

                var directoryPath = GetDirectoryPathFromUrl(folder.ParentUrl);
                var blobDirectory = !String.IsNullOrEmpty(directoryPath) ? blobContainer.GetDirectoryReference(directoryPath) : null;
                var blobFolder = blobDirectory == null ? blobContainer.GetBlockBlobReference(folder.Name) : blobDirectory.GetBlockBlobReference(folder.Name);
                blobFolder.UploadText(String.Empty);
            }
        }

        #endregion

        #region IBlobUrlResolver Members

        public string GetAbsoluteUrl(string blobKey)
        {
            var retVal = blobKey;
            if (!Uri.IsWellFormedUriString(blobKey, UriKind.Absolute))
            {
                var root = _cloudStorageAccount.BlobEndpoint.AbsoluteUri;
                retVal = String.Format("{0}{1}", root.EndsWith("/") ? root : root + "/", blobKey);

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
                relativeUrl = new Uri(url).AbsolutePath;
            }
            return relativeUrl.Split(new[] { "/", "\\", _cloudBlobClient.DefaultDelimiter }, StringSplitOptions.RemoveEmptyEntries);
        }

        private string GetContainerNameFromUrl(string url)
        {
            return GetOutlineFromUrl(url).First();
        }

        private string GetDirectoryPathFromUrl(string url)
        {
            return String.Join(_cloudBlobClient.DefaultDelimiter, GetOutlineFromUrl(url).Skip(1).ToArray());
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
