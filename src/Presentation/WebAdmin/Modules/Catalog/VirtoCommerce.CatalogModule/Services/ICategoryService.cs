using VirtoCommerce.CatalogModule.Model;
namespace VirtoCommerce.CatalogModule.Services
{
    public interface ICategoryService
    {
		Category GetById(string categoryId);
		Category Create(Category category);
		void Update(Category[] categories);
		void Delete(string[] categoryIds);
    }
}
