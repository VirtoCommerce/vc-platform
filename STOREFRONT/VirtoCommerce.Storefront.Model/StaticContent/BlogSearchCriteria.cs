using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.StaticContent
{
    public class BlogSearchCriteria
    {
        public BlogSearchCriteria()
        {
            PageNumber = 1;
            PageSize = 20;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
