using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Assets
{
    public class AssetEntrySearchCriteria : SearchCriteriaBase
    {

        public TenantIdentity[] Tenants { get; set; }


        public string Group { get; set; }
    }
}
