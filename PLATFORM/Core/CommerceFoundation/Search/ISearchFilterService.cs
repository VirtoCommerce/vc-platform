namespace VirtoCommerce.Foundation.Search
{
    using System;
    using System.Collections.Generic;

    [Obsolete("Use IBrowseFilterService instead")]
    public interface ISearchFilterService
    {
        ISearchFilter[] Filters { get; }
    }

    public interface IBrowseFilterService
    {
        ISearchFilter[] GetFilters(IDictionary<string, object> context);
    }
}
