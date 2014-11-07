using VirtoCommerce.CatalogModule.Model;
namespace VirtoCommerce.CatalogModule.Services
{
    public interface IItemService
    {
		CatalogProduct GetById(string itemId);
		CatalogProduct Create(CatalogProduct item);
		void Update(CatalogProduct[] items);
		void Delete(string[] itemIds);

    }
}
