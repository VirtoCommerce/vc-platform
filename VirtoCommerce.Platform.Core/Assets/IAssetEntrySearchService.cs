namespace VirtoCommerce.Platform.Core.Assets
{
    public interface IAssetEntrySearchService
    {
        AssetEntrySearchResult SearchAssetEntries(AssetEntrySearchCriteria criteria);
    }
}
