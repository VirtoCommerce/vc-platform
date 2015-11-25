using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Services
{
    public interface ICatalogService
    {
        Task<Product> GetProductAsync(string id, ItemResponseGroup responseGroup);

        Task<SearchResult> SearchAsync(SearchCriteria criteria);
    }
}
