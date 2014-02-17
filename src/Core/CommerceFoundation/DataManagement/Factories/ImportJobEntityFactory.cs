using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Importing.Model;

namespace VirtoCommerce.Foundation.Importing.Factories
{
	public class ImportJobEntityFactory : FactoryBase, IImportJobEntityFactory
	{
		public ImportJobEntityFactory()
		{
			RegisterStorageType(typeof(ImportJob), "ImportJob");
			RegisterStorageType(typeof(MappingItem), "MappingItem");
			RegisterStorageType(typeof(ItemImporter), "ItemImporter");
		}
	}
}
