using VirtoCommerce.Domain.Catalog.Model;
namespace VirtoCommerce.Domain.Catalog.Services
{
    public interface ICategoryService
    {
		Category GetById(string categoryId);
		Category Create(Category category);
		void Update(Category[] categories);
		void Delete(string[] categoryIds);
    }
}
