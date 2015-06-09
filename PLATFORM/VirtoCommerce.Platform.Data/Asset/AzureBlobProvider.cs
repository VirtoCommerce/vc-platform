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

        private static CloudStorageAccount ParseConnectionString(string connectionString)
        {
            CloudStorageAccount cloudStorageAcount;
            if (!CloudStorageAccount.TryParse(connectionString, out cloudStorageAcount))
            {
                throw new InvalidOperationException("Failed to get valid connection string");
            }
            return cloudStorageAcount;
        }

    
    }
}
