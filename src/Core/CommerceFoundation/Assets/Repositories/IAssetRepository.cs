using VirtoCommerce.Foundation.Assets.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Assets.Repositories
{
	public interface IAssetRepository : IRepository
	{
		Folder GetFolderById(string folderId);
		FolderItem GetFolderItemById(string itemId);
		Folder[] GetChildrenFolders(string folderId);
		FolderItem[] GetChildrenFolderItems(string folderId);
	}
}
