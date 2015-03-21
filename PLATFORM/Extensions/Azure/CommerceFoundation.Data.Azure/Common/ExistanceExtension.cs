namespace VirtoCommerce.Foundation.Data.Azure.Common
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.Blob.Protocol;

    using System;

    public static class ExistenceExtensions
	{
        /// <summary>
        /// Existses the specified BLOB.
        /// </summary>
        /// <param name="blob">The BLOB.</param>
        /// <returns></returns>
		[CLSCompliant(false)]
		public static bool Exists(this ICloudBlob blob)
		{
			try
			{
				blob.FetchAttributes();
				return true;
			}
			catch (StorageException e)
			{
				if (e.RequestInformation.ExtendedErrorInformation.ErrorCode == BlobErrorCodeStrings.BlobNotFound)
				{
					return false;
				}
				else
				{
					throw;
				}
			}
		}
        /// <summary>
        /// Existses the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns></returns>
		[CLSCompliant(false)]
		public static bool Exists(this CloudBlobContainer container)
		{
			try
			{
				container.FetchAttributes();
				return true;
			}
			catch (StorageException e)
			{
				if (e.RequestInformation.ExtendedErrorInformation.ErrorCode == BlobErrorCodeStrings.BlobNotFound)
				{
					return false;
				}
				else
				{
					throw;
				}
			}
		}
	}
}
