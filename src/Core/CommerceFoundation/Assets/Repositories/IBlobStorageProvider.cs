using VirtoCommerce.Foundation.Assets.Services;
using System.IO;

namespace VirtoCommerce.Foundation.Assets.Repositories
{
	public interface IBlobStorageProvider
	{
		string Upload(UploadStreamInfo info);
		Stream OpenReadOnly(string blobKey);
		bool Exists(string blobKey);
		byte[] GetImagePreview(string blobKey);
	}
}
