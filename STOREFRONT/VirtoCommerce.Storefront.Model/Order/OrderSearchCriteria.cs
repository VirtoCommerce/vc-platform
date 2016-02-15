using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Order
{
    public class OrderSearchCriteria : PagedSearchCriteria
    {
        public OrderSearchCriteria(NameValueCollection queryString)
            : base(queryString)
        {
        }
    }
}
