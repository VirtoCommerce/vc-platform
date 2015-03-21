using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VirtoCommerce.Foundation.Frameworks.Common
{
	public static class StreamHelper
	{
		private const long MaxBufferSize = 65443;

		public static byte[] ToByteArray(this Stream inputStream)
		{
			if (!inputStream.CanRead)
			{
				throw new ArgumentException();
			}
			var retval = new byte[] { };
			using (var memoryStream = new MemoryStream())
			{
				StreamHelper.WriteToStream(memoryStream, inputStream, 0, inputStream.Length);
				retval = memoryStream.ToArray();
			}
			return retval;
		}


		public static void WriteToStream(Stream targetStream, Stream sourceStream, long startOffset, long length)
		{
			if (targetStream == null)
				throw new ArgumentNullException("targetStream");

			if (sourceStream == null)
				throw new ArgumentNullException("sourceStream");

			int bufferSize = MaxBufferSize < length ? (int)MaxBufferSize : (int)length;
			byte[] tmpBuffer = new byte[bufferSize];
			int readCount = 0;
			long allBytes = 0;
			sourceStream.Seek(startOffset, SeekOrigin.Begin);
			do
			{
				readCount = sourceStream.Read(tmpBuffer, 0, bufferSize);
				allBytes += readCount;
				readCount = allBytes > length ? readCount - (int)(allBytes - length)
											  : readCount;
				targetStream.Write(tmpBuffer, 0, readCount);

			}
			while ((readCount != 0) && (allBytes < length));

			targetStream.Flush();
		}
	}
}
