using System.Collections.Generic;

namespace VirtoCommerce.Platform.Web.Model.Asset
{
    public class AssetEntrySearchResult
    {
        public int TotalCount { get; set; }
        public AssetEntry[] Assets { get; set; }
    }
}
