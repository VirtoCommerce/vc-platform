using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class PaymentMethod
    {
        public string GatewayCode { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Group { get; set; }
        public int Priority { get; set; }
    }
}