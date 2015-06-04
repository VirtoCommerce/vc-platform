using VirtoCommerce.Domain.Catalog.Model;
namespace VirtoCommerce.Domain.Catalog.Services
{
    public interface IItemService
    {
		CatalogProduct GetById(string itemId, ItemResponseGroup respGroup);
        CatalogProduct[] GetByIds(string[] itemIds, ItemResponseGroup respGroup);
		CatalogProduct Create(CatalogProduct item);
		void Update(CatalogProduct[] items);
		void Delete(string[] itemIds);
    }
}
