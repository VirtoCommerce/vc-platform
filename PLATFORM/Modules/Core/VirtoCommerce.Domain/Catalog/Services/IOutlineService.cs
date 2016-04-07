using System.Collections.Generic;
using VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.Domain.Catalog.Services
{
    public interface IOutlineService
    {
        void FillOutlinesForCategories(IEnumerable<Category> categories, string catalogId);
        void FillOutlinesForProducts(IEnumerable<CatalogProduct> products, string catalogId);
    }
}
