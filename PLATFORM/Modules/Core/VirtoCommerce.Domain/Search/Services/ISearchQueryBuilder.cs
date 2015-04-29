using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Domain.Search.Services
{
    public interface ISearchQueryBuilder
    {
        object BuildQuery(ISearchCriteria criteria);
    }

}
