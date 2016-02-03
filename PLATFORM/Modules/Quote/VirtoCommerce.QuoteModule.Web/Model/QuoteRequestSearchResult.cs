using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.QuoteModule.Web.Model
{
    public class QuoteRequestSearchResult
    {
        public int TotalCount { get; set; }
        public ICollection<QuoteRequest> QuoteRequests { get; set; }
    }
}