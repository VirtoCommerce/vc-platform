using System;
using System.Collections.Generic;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;

namespace VirtoCommerce.Domain.Search
{
    public interface ISearchProviderManager
    {
        void RegisterSearchProvider(string name, Func<ISearchConnection, ISearchProvider> factory);
        IEnumerable<string> RegisteredProviders { get; }
        ISearchProvider CurrentProvider { get; }
        ISearchConnection CurrentConnection { get; }
    }
}
