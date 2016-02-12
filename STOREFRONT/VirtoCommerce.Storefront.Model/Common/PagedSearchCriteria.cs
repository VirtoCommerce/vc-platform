using System;
using System.Collections.Specialized;

namespace VirtoCommerce.Storefront.Model.Common
{
    public class PagedSearchCriteria
    {
        public PagedSearchCriteria(NameValueCollection queryString)
        {
            PageNumber = Convert.ToInt32(queryString.Get("page") ?? 1.ToString());
            PageSize = Convert.ToInt32(queryString.Get("count") ?? 10.ToString());
        }

        public int Start
        {
            get
            {
                return (PageNumber - 1) * PageSize;
            }
        }
                
        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
