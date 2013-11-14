using System;
using System.Globalization;
using VirtoCommerce.Foundation.Assets.Repositories;

namespace VirtoCommerce.ManagementClient.Asset.Repositories
{
    public class MockBlobStorageProvider : IBlobStorageProvider
    {

        public string Upload(Foundation.Assets.Services.UploadStreamInfo info)
        {
            System.Threading.Thread.Sleep(1000);

            return DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture); //  info.Length.ToString();
        }

        public System.IO.Stream OpenReadOnly(string blobKey)
        {
            throw new NotImplementedException();
        }

	    public bool Exists(string blobKey)
	    {
		    throw new NotImplementedException();
	    }

		#region IBlobStorageProvider Members


		public byte[] GetImagePreview(string blobKey)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
