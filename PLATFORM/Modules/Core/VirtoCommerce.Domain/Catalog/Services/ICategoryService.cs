using VirtoCommerce.Domain.Catalog.Model;
namespace VirtoCommerce.Domain.Catalog.Services
{
    public interface ICategoryService
    {
        Category[] GetByIds(string[] categoryIds, CategoryResponseGroup responseGroup, string catalogId = null);
        Category GetById(string categoryId, CategoryResponseGroup responseGroup, string catalogId = null);
        void Create(Category[] categories);
        Category Create(Category category);
		void Update(Category[] categories);
		void Delete(string[] categoryIds);
    }
}
