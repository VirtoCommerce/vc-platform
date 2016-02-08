using System.Collections.Generic;

namespace VirtoCommerce.StoreModule.Web.Model
{
    public class SendDynamicNotificationRequest
    {
        public string StoreId { get; set; }
        public string Type { get; set; }
        public IDictionary<string, string> Fields { get; set; }

        public string Language { get; set; }
    }
}
