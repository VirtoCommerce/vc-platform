using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Quote
{
    public class QuoteSearchCriteria : PagedSearchCriteria
    {
        public QuoteSearchCriteria(NameValueCollection queryString)
            :base(queryString)
        {

        }
    }
}
