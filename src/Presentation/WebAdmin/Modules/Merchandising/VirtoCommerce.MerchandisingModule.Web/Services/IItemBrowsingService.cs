using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Services
{
    public interface IItemBrowsingService
    {
        #region Public Methods and Operators

        ProductSearchResult SearchItems(CatalogItemSearchCriteria criteria, ItemResponseGroup responseGroup);

        #endregion

        //T GetItem<T>(string itemCode, ItemResponseGroups responseGroup) where T : CatalogItem;

        //ResponseCollection<Category> GetCategories(string outline, string language);
    }
}
