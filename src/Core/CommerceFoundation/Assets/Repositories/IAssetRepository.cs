using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Assets.Model;
using System.ServiceModel;
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
