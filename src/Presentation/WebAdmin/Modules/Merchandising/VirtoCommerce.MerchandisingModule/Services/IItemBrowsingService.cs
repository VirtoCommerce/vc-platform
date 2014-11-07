using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.MerchandisingModule.Model;

namespace VirtoCommerce.MerchandisingModule.Services
{
    public interface IItemBrowsingService
    {
        ResponseCollection<T> SearchItems<T>(CatalogItemSearchCriteria criteria, ItemResponseGroups responseGroup) where T : CatalogItem;
        T GetItem<T>(string itemCode, ItemResponseGroups responseGroup) where T : CatalogItem;

        ResponseCollection<Category> GetCategories(string outline, string language);
    }
}
