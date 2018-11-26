using System;

namespace VirtoCommerce.Platform.Core.Security
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; }
    }
}
