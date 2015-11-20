using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Model.Services
{
    public interface ICategoryService
    {
        Task<Category> GetCategoryById(string id, string catalogId, string language, string currencyCode);

        Task<Category[]> GetCategoriesByCatalog(string catalogId);
    }
}
