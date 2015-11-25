using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Services
{
    public interface ICatalogSearchService
    {
        Task<Product> GetProductAsync(string id, ItemResponseGroup responseGroup);

        Task<CatalogSearchResult> SearchAsync(CatalogSearchCriteria criteria);
    }
}
