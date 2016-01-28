using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.StoreModule.Web.Model
{
    public class SearchResult
    {
        public int TotalCount { get; set; }
        public Store[] Stores { get; set; }
    }
}