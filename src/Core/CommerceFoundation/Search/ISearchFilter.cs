using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Search
{
    public interface ISearchFilter
    {
        string Key { get; }

        string CacheKey { get; }
    }
}
