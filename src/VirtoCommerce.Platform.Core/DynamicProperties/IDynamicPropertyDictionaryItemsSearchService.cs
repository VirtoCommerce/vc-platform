using System;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public interface IDynamicPropertyDictionaryItemsSearchService : ISearchService<DynamicPropertyDictionaryItemSearchCriteria, DynamicPropertyDictionaryItemSearchResult, DynamicPropertyDictionaryItem>
    {
        [Obsolete("Use SearchAsync()", DiagnosticId = "VC0005", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions/")]
        Task<DynamicPropertyDictionaryItemSearchResult> SearchDictionaryItemsAsync(DynamicPropertyDictionaryItemSearchCriteria criteria);
    }
}
