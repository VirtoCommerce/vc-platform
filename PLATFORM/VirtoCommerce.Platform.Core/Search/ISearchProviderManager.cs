using System;
using System.Collections.Generic;
using VirtoCommerce.Foundation.Search;

namespace VirtoCommerce.Framework.Web.Search
{
    public interface ISearchProviderManager
    {
        void RegisterSearchProvider(string name, Func<ISearchConnection, ISearchProvider> factory);
        IEnumerable<string> RegisteredProviders { get; }
        ISearchProvider CurrentProvider { get; }
        ISearchConnection CurrentConnection { get; }
    }
}
