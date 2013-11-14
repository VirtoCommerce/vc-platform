using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Assets.Factories;
using VirtoCommerce.ManagementClient.Asset.Model;
using VirtoCommerce.Foundation.Assets.Model;

namespace VirtoCommerce.ManagementClient.Asset.Factories
{
	public class AssetEntityFactoryExt : AssetEntityFactory
	{
		public AssetEntityFactoryExt()
        {
			RegisterStorageType(typeof(FolderExt), "FolderExt");
			RegisterStorageType(typeof(FolderItemExt), "FolderItemExt");

			OverrideType(typeof(Folder), typeof(FolderExt));
			OverrideType(typeof(FolderItem), typeof(FolderItemExt));
        }
	}
}
