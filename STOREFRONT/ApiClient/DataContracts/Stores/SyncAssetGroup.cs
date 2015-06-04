using System;

namespace VirtoCommerce.ApiClient.DataContracts.Stores
{
    public class SyncAssetGroup
    {
        public string Type { get; set; }

        public SyncAsset[] Assets { get; set; }
    }
}
