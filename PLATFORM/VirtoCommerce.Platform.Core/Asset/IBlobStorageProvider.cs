using System.IO;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Asset
{
	public interface IBlobStorageProvider
	{
        BlobSearchResult Search(string folderUrl, string keyword);
        void CreateFolder(BlobFolder folder);

        string Upload(UploadStreamInfo request);
		Stream OpenReadOnly(string url);
        void Remove(string[] urls);
	}
}
