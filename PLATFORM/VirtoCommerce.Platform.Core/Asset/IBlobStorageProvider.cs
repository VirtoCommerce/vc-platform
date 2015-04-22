using System.IO;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Asset
{
	public interface IBlobStorageProvider
	{
		string Upload(UploadStreamInfo request);
		Task<string> UploadAsync(UploadStreamInfo request);

		Stream OpenReadOnly(string blobKey);
		Task<Stream> OpenReadOnlyAsync(string blobKey);
	}
}
