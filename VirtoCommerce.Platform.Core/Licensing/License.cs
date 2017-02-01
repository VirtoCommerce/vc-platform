using System;

namespace VirtoCommerce.Platform.Core.Licensing
{
    public class License
    {
        public string Type { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
