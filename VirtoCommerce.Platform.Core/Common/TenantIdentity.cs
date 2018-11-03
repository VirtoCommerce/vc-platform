namespace VirtoCommerce.Platform.Core.Common
{
    public class TenantIdentity : ValueObject
    {
        public string TenantId { get; set; }
        public string TenantType { get; set; }

        public bool IsValid => !string.IsNullOrEmpty(TenantId) && !string.IsNullOrEmpty(TenantType);

        public override string ToString()
        {
            return $"{TenantId}_{TenantType}";
        }
    }
}
