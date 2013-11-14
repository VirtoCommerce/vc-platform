using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Search
{
    public interface ISearchQueryBuilder
    {
        object BuildQuery(ISearchCriteria criteria);
    }

}
