using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Model.Services
{
    public interface ICatalogService
    {
        Task<Product> GetProduct(string id, string currencyCode, ItemResponseGroup responseGroup);

        Task<SearchResult> Search(SearchCriteria criteria);
    }
}
