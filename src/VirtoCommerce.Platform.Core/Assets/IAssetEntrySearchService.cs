using System;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Assets
{
    [Obsolete("Deprecated. Use assets from Assets module.")]
    public interface IAssetEntrySearchService
    {
        Task<AssetEntrySearchResult> SearchAssetEntriesAsync(AssetEntrySearchCriteria criteria);
    }
}
