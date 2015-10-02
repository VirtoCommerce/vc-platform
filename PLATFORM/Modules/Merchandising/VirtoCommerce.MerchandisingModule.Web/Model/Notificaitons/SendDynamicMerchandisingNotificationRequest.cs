using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.MerchandisingModule.Web.Model.Notificaitons
{
    public class SendDynamicMerchandisingNotificationRequest
    {
        public string Type { get; set; }
        public IDictionary Fields { get; set; }
        public string StoreId { get; set; }
        public string Language { get; set; }
    }
}