using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.StaticContent
{
    public class BlogSearchCriteria : PagedSearchCriteria
    {
        public BlogSearchCriteria(NameValueCollection queryString)
            : base(queryString)
        {
        }
    }
}
