using VirtoCommerce.CatalogModule.Model;
namespace VirtoCommerce.CatalogModule.Services
{
	public interface ICatalogService
	{
		Catalog GetById(string catalogId);
		Catalog Create(Catalog catalog);
		void Update(Catalog[] catalogs);
		void Delete(string[] catalogIds);
	}
}