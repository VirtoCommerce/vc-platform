using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Catalogs.Repositories
{
	public interface ICatalogRepository : IRepository
	{
		IQueryable<CategoryBase> Categories { get; }
		IQueryable<CatalogBase> Catalogs { get; }
		IQueryable<Item> Items { get; }
		IQueryable<Property> Properties { get; }
		IQueryable<PropertySet> PropertySets { get; }
		IQueryable<ItemRelation> ItemRelations { get; }
		IQueryable<CategoryItemRelation> CategoryItemRelations { get; }
		IQueryable<Packaging> Packagings { get; }
		IQueryable<TaxCategory> TaxCategories { get; }
		IQueryable<Association> Associations { get; }
	}
}
