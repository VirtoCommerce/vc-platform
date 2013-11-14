namespace VirtoCommerce.Foundation.Data.Azure.Asset
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.Blob.Protocol;

    using System;
    using System.Text.RegularExpressions;

    public static class AzureBlobStorageHelper
	{
		public const string RootBlobContainerName = "$root";

		internal static bool IsBlobContainerNameValid(string name)
		{
			if (name.Equals(RootBlobContainerName))
			{
				return true;
			}
			string validBlobContainerNameRegex = @"^([a-z]|\d){1}([a-z]|-|\d){1,61}([a-z]|\d){1}$";
			Regex reg = new Regex(validBlobContainerNameRegex);
			return reg.IsMatch(name);
		}

		[CLSCompliant(false)]
		public static bool Exists(this CloudBlobDirectory directory)
		{
			var retVal = true;
			try
			{

				directory.Container.FetchAttributes();
			}
			catch (StorageException e)
			{
				switch (e.RequestInformation.ExtendedErrorInformation.ErrorCode)
				{
					case BlobErrorCodeStrings.ContainerNotFound:
					case BlobErrorCodeStrings.BlobNotFound:
						retVal = false;
						break;
					default:
						throw;
				}
			}
			return retVal;
		}

		[CLSCompliant(false)]
		public static bool Exists(this ICloudBlob blob)
		{
			var retVal = true;
			try
			{
				blob.FetchAttributes();
			}
			catch (StorageException e)
			{
				if (e.RequestInformation.ExtendedErrorInformation.ErrorCode == BlobErrorCodeStrings.BlobNotFound)
				{
					retVal = false;
				}
				else
				{
					throw;
				}
			}
			return retVal;
		}
	}
}
