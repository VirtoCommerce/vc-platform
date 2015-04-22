using System;
using System.ServiceModel;

namespace VirtoCommerce.Platform.Core.Asset
{
	[MessageContract]
	public class UploadStreamInfo : IDisposable
	{
		/// <summary>
		/// The full file name including (relative) path on the server.
		/// </summary>
		[MessageHeader(MustUnderstand = true)]
		public string FileName;
		[MessageHeader(MustUnderstand = true)]
		public string FolderName;

		[MessageHeader(MustUnderstand = true)]
		public long Length;

		[MessageBodyMember(Order = 1)]
		public System.IO.Stream FileByteStream;

		public void Dispose()
		{
			if (FileByteStream != null)
			{
				FileByteStream.Close();
				FileByteStream = null;
			}
		}
	}
}
