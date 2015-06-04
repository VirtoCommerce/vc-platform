using VirtoCommerce.Domain.Catalog.Model;
namespace VirtoCommerce.Domain.Catalog.Services
{
	public interface IPropertyService
	{

		Property GetById(string propertyId);
		Property[] GetCatalogProperties(string catalogId);
		Property[] GetCategoryProperties(string categoryId);
		Property Create(Property property);
		void Update(Property[] properties);
		void Delete(string[] propertyIds);

		PropertyDictionaryValue[] SearchDictionaryValues(string propertyId, string keyword);
	}
}
