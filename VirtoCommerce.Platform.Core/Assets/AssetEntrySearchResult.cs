using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Assets
{
    public class AssetEntrySearchResult
    {
        public int TotalCount { get; set; }
        public IList<AssetEntry> Results { get; set; }
    }
}
