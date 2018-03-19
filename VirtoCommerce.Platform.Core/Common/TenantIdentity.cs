namespace VirtoCommerce.Platform.Core.Common
{
    public class TenantIdentity : ValueObject<TenantIdentity>
    {
        public string TenantId { get; set; }
        public string TenantType { get; set; }
    }
}
