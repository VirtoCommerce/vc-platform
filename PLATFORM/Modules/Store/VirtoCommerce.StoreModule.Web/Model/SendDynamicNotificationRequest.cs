using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.StoreModule.Web.Model
{
    public class SendDynamicNotificationRequest
    {
        public string StoreId { get; set; }
        public string Type { get; set; }
        public IDictionary Fields { get; set; }
        
        public string Language { get; set; }
    }
}