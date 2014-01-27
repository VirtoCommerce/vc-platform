using System.Linq;
using VirtoCommerce.Foundation.Assets.Model;
using VirtoCommerce.Foundation.Assets.Repositories;
using System.IO;
using VirtoCommerce.Foundation.Frameworks;


namespace VirtoCommerce.Foundation.Assets.Services
{

	[UnityInstanceProviderServiceBehaviorAttribute]
	public class AssetService : IAssetService
	{
		protected IAssetRepository AssetRepository;
		protected IBlobStorageProvider BlobStorageProvider;

		public AssetService(IAssetRepository assetRepository, IBlobStorageProvider blobStorageProvider)
		{
			AssetRepository = assetRepository;
			BlobStorageProvider = blobStorageProvider;
		}

		#region IAssetService Members
		public Model.Folder GetFolderById(string folderId)
		{
			return AssetRepository.GetFolderById(folderId);
		}

		public Model.Folder[] GetChildrenFolders(string folderId)
		{
			return AssetRepository.GetChildrenFolders(folderId).ToArray();
		}

		public Model.FolderItem[] GetChildrenFolderItems(string folderId)
		{
			return AssetRepository.GetChildrenFolderItems(folderId).ToArray();
		}

		public Model.FolderItem GetFolderItemById(string itemId)
		{
			return AssetRepository.GetFolderItemById(itemId);
		}

        /// <summary>
        /// Uploads the specified stream.
        /// </summary>
        /// <param name="info">The info.</param>
		public void Upload(UploadStreamInfo info)
		{
			BlobStorageProvider.Upload(info);
		}

		public Stream OpenReadOnly(string blobKey)
		{
			return BlobStorageProvider.OpenReadOnly(blobKey);
		}

		public byte[] GetImagePreview(string blobKey)
		{
			return BlobStorageProvider.GetImagePreview(blobKey);
		}

		public bool Exists(string blobKey)
		{
			return BlobStorageProvider.Exists(blobKey);
		}

	    public void Delete(string id)
	    {
            object item = GetFolderItemById(id) ?? (object)GetFolderById(id);

	        if (item != null)
	        {
                AssetRepository.Remove(item);
	            AssetRepository.UnitOfWork.Commit();
	        }
	    }

        public Folder CreateFolder(string folderName, string parentFolderId)
	    {
	        return AssetRepository.CreateFolder(folderName, parentFolderId);
	    }

	    public void Rename(string id, string name)
	    {
	        object item = GetFolderItemById(id);

	        if (item != null)
	        {
	            ((FolderItem) item).Name = name;
                AssetRepository.Update(item);
                AssetRepository.UnitOfWork.Commit();
                return;
	        }

            item = GetFolderById(id);
            if (item != null)
            {
                ((Folder)item).Name = name;
                AssetRepository.Update(item);
                AssetRepository.UnitOfWork.Commit();
            }
	    }

	    #endregion
	}
}
