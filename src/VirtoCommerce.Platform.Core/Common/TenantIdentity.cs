namespace VirtoCommerce.Platform.Core.Common
{

    public class TenantIdentity : ValueObject
    {
        public static TenantIdentity Empty { get; set; } = new TenantIdentity(null, null);

        public TenantIdentity(string id, string type)
        {
            Id = id;
            Type = type;
        }

        public string Id { get; set; }
        public string Type { get; set; }
        public bool IsEmpty => !IsValid;
        public bool IsValid => !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Type);

    }
}
