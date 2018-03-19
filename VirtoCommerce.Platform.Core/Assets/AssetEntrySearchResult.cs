using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Assets
{
    public class AssetEntrySearchResult
    {
        public AssetEntrySearchResult()
        {
            Assets = new List<AssetEntry>();
        }
        public int TotalCount { get; set; }
        public IList<AssetEntry> Assets { get; set; }
    }
}
