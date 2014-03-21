using System.Collections.Generic;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Importing.Model
{
	public interface IEntityImporter
	{
		string Name { get; set; }
		string[] SystemPropertyNames { get; }
		List<ImportProperty> SystemProperties { get; set; }
		string Import(string catalogId, string propertySetId, ImportItem[] systemValues, ImportItem[] customValues, IRepository repository);
	}
}
