using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class MarketingEvent : IMarketingEvent
    {
        public string Type { get; set; }
        public string Data { get; set; }
 
    }
}