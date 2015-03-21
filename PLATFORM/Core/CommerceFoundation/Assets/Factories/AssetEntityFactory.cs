using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Assets.Model;


namespace VirtoCommerce.Foundation.Assets.Factories
{
	public class AssetEntityFactory : FactoryBase, IAssetEntityFactory
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetEntityFactory" /> class.
        /// </summary>
		public AssetEntityFactory()
		{
			RegisterStorageType(typeof(Folder), "Folder");
			RegisterStorageType(typeof(FolderItem), "FolderItem");
		}
	}
}
