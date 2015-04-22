using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Asset;


namespace VirtoCommerce.Platform.Data.Asset
{
	public class BlobStorageMultipartProvider : MultipartFileStreamProvider
	{
		private readonly IBlobStorageProvider _blobProvider;
		private readonly string _folder;
		public BlobStorageMultipartProvider(IBlobStorageProvider blobProvider, string tempPath, string folder)
			: base(tempPath)
		{
			_folder = folder;
			_blobProvider = blobProvider;
			BlobInfos = new List<BlobInfo>();
		}

		public List<BlobInfo> BlobInfos { get; set; }

		public override Task ExecutePostProcessingAsync()
		{
			// Upload the files to  blob storage and remove them from local disk
			foreach (var fileData in this.FileData)
			{
				var fileInfo = new FileInfo(fileData.LocalFileName);
				using (Stream stream = fileInfo.OpenRead())
				{
					var uploadStreamInfo = new UploadStreamInfo
					{
						FileByteStream = stream,
						FileName = fileInfo.Name,
						Length = fileInfo.Length,
						FolderName = _folder
					};

					var blobKey = _blobProvider.Upload(uploadStreamInfo);

					BlobInfos.Add(new BlobInfo
					{
						ContentType = fileData.Headers.ContentType.MediaType,
						FileName = fileInfo.Name,
						Size = fileInfo.Length,
						Key = blobKey
					});
					
				}
				File.Delete(fileData.LocalFileName);
			}

			return base.ExecutePostProcessingAsync();
		}

		public override string GetLocalFileName(HttpContentHeaders headers)
		{
			// override the filename which is stored by the provider (by default is bodypart_x)
			string oldfileName = headers.ContentDisposition.FileName.Replace("\"", string.Empty);
			string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(oldfileName);

			return newFileName;
		}
	}

}
