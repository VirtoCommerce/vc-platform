using System;
using System.Collections.Generic;
namespace VirtoCommerce.Domain.Search.Services
{
   public interface IBrowseFilterService
    {
        ISearchFilter[] GetFilters(IDictionary<string, object> context);
    }
}
