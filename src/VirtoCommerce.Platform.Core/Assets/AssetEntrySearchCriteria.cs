using System;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Swagger;

namespace VirtoCommerce.Platform.Core.Assets
{
    [Obsolete("Deprecated. Use assets from Assets module.")]
    [SwaggerSchemaId("AssetEntrySearchCriteria")]
    public class AssetEntrySearchCriteria : SearchCriteriaBase
    {
        public TenantIdentity[] Tenants { get; set; }


        public string Group { get; set; }
    }
}
