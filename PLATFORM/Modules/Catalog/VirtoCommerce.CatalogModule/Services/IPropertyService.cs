using VirtoCommerce.CatalogModule.Model;
namespace VirtoCommerce.CatalogModule.Services
{
	public interface IPropertyService
	{

		Property GetById(string propertyId);
		Property[] GetCategoryProperties(string categoryId);
		Property Create(Property property);
		void Update(Property[] properties);
		void Delete(string[] propertyIds);

		PropertyDictionaryValue[] SearchDictionaryValues(string propertyId, string keyword);
	}
}
