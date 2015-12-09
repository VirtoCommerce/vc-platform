using VirtoCommerce.Domain.Catalog.Model;
namespace VirtoCommerce.Domain.Catalog.Services
{
	public interface IPropertyService
	{

		Property GetById(string propertyId);
        Property[] GetByIds(string[] propertyIds);
    	Property Create(Property property);
		void Update(Property[] properties);
		void Delete(string[] propertyIds);
        Property[] GetAllCatalogProperties(string catalogId);
        Property[] GetAllProperties();
        PropertyDictionaryValue[] SearchDictionaryValues(string propertyId, string keyword);
	}
}
