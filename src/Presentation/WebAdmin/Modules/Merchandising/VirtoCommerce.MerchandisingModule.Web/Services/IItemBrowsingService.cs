using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MerchandisingModule.Web.Services
{
	using VirtoCommerce.Domain.Catalog.Model;
	using VirtoCommerce.Foundation.Catalogs.Search;
	using VirtoCommerce.MerchandisingModule.Web.Model;

    public interface IItemBrowsingService
    {
        ProductSearchResult SearchItems(CatalogItemSearchCriteria criteria, ItemResponseGroup responseGroup);

        //T GetItem<T>(string itemCode, ItemResponseGroups responseGroup) where T : CatalogItem;

        //ResponseCollection<Category> GetCategories(string outline, string language);
    }
}
