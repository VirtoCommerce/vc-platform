using VirtoCommerce.Foundation.Assets.Model;
using System.IO;
using System.ServiceModel;

namespace VirtoCommerce.Foundation.Assets.Services
{
    /// <summary>
    /// Asset service used to communicate with storage systems.
    /// </summary>
	[ServiceContract]
	public interface IAssetService 
	{
		[OperationContract]
		Folder GetFolderById(string folderId);
		[OperationContract]
		FolderItem GetFolderItemById(string itemId);
		[OperationContract]
		Folder[] GetChildrenFolders(string folderId);
		[OperationContract]
		FolderItem[] GetChildrenFolderItems(string folderId);
        /// <summary>
        /// Uploads the specified stream.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <returns>path of an uploaded file</returns>
		[OperationContract]
		void Upload(UploadStreamInfo info);
		[OperationContract]
		byte[] GetImagePreview(string blobKey);
		[OperationContract]
		Stream OpenReadOnly(string blobKey);
		[OperationContract]
		bool Exists(string blobKey);
        [OperationContract]
        Folder CreateFolder(string folderName, string parentFolderId);
        [OperationContract]
        void  Rename(string id, string name);
        [OperationContract]
        void Delete(string id);
	}
}
