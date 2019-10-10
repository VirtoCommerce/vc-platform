using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Assets
{
    public interface IAssetEntrySearchService
    {
        Task<AssetEntrySearchResult> SearchAssetEntriesAsync(AssetEntrySearchCriteria criteria);
    }
}
