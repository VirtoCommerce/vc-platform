using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Assets.Model;
using VirtoCommerce.Foundation.Assets.Repositories;
using VirtoCommerce.Foundation.Assets.Services;

namespace VirtoCommerce.Platform.Core.Asset
{
	public class BlobStorageMultipartProvider : MultipartFileStreamProvider
	{
		private readonly IBlobStorageProvider _blobProvider;
		private readonly string _container;
		public BlobStorageMultipartProvider(IBlobStorageProvider blobProvider, string tempPath, string container)
			: base(tempPath)
		{
			_container = container;
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
						FileName = _container + "/" + fileInfo.Name,
						Length = fileInfo.Length
					};

					var link = _blobProvider.Upload(uploadStreamInfo);

					BlobInfos.Add(new BlobInfo
					{
						ContentType = fileData.Headers.ContentType.MediaType,
						Name = fileInfo.Name,
						Size = fileInfo.Length,
						Location = link
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
