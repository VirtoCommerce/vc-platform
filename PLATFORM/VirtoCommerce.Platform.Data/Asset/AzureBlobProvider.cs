using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using VirtoCommerce.Platform.Core.Asset;

namespace VirtoCommerce.Platform.Data.Asset
{
	public class AzureBlobProvider : IBlobStorageProvider, IBlobUrlResolver
	{
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
			if (container.Exists())
			{
				var blob = container.GetBlockBlobReference(request.FileName);
				blob.Properties.ContentType = ResolveContentType(request.FileName);

				using (var memoryStream = new MemoryStream())
				{
					// upload to MemoryStream
					memoryStream.SetLength(request.Length);
					request.FileByteStream.CopyTo(memoryStream);
					memoryStream.Position = 0;
					// fill blob
					blob.UploadFromStream(memoryStream);
				}

				result = blob.Uri.AbsolutePath.TrimStart('/');
			}

			return result;
		}


		public System.IO.Stream OpenReadOnly(string blobKey)
		{
			System.IO.Stream retVal = null;
			var cloudBlob = _cloudBlobClient.GetBlobReferenceFromServer(new Uri(_cloudBlobClient.BaseUri, blobKey));
			if(cloudBlob.Exists())
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
			var root = _cloudStorageAccount.BlobEndpoint.AbsoluteUri;
			return String.Format("{0}{1}", root.EndsWith("/") ? root : root + "/", blobKey);
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

		private static string ResolveContentType(string fileName)
		{
			string result;
			var mapping = new Dictionary<string, string>();
			mapping.Add("pdf", "application/pdf");
			mapping.Add("zip", "application/zip");
			mapping.Add("gz", "application/x-gzip");
			mapping.Add("gzip", "application/x-gzip");
			mapping.Add("m4a", "audio/mp4");
			mapping.Add("gif", "image/gif");
			mapping.Add("jpg", "image/jpeg");
			mapping.Add("jpeg", "image/jpeg");
			mapping.Add("png", "image/png");
			mapping.Add("svg", "image/svg+xml");
			mapping.Add("tif", "image/tiff");
			mapping.Add("tiff", "image/tiff");
			mapping.Add("csv", "text/csv");
			mapping.Add("html", "text/html");
			mapping.Add("mpg", "video/mpeg");
			mapping.Add("mpeg", "video/mpeg");
			mapping.Add("mp4", "video/mp4");
			mapping.Add("ogg", "video/ogg");
			mapping.Add("qt", "video/quicktime");
			mapping.Add("mov", "video/quicktime");

			var ext = Path.GetExtension(fileName).Substring(1).ToLower();
			if (mapping.ContainsKey(ext))
			{
				result = mapping[ext];
			}
			else
			{
				result = "application/octet-stream";
			}

			return result;
		}
	}
}
