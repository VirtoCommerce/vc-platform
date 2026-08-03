namespace VirtoCommerce.Platform.Core.Security
{
    public class AdminUIAccessResult
    {
        public bool IsAllowed { get; set; }

        public string DenyReason { get; set; }

        public static AdminUIAccessResult Allowed() => new() { IsAllowed = true };

        public static AdminUIAccessResult Denied(string reason) => new() { IsAllowed = false, DenyReason = reason };
    }
}
