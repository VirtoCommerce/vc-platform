using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Services
{
    public interface IItemBrowsingService
    {
        #region Public Methods and Operators

        ProductSearchResult SearchItems(CatalogIndexedSearchCriteria criteria, ItemResponseGroup responseGroup);

        #endregion

    }
}
