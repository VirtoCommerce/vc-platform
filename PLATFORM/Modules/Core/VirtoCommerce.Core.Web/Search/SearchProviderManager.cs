using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Framework.Web.Search;
using VirtoCommerce.Framework.Web.Settings;

namespace VirtoCommerce.CoreModule.Web.Search
{
    public class SearchProviderManager : ISearchProviderManager, ISearchProvider, ISearchConnection
    {
        private readonly ISettingsManager _settingsManager;
        private readonly CacheHelper _cache;
        private readonly ConcurrentDictionary<string, Func<ISearchConnection, ISearchProvider>> _factories;

        public SearchProviderManager(ISettingsManager settingsManager, ICacheRepository cacheRepository)
        {
            _settingsManager = settingsManager;
            _cache = new CacheHelper(cacheRepository, "_SearchCache");
            _factories = new ConcurrentDictionary<string, Func<ISearchConnection, ISearchProvider>>(StringComparer.OrdinalIgnoreCase);
        }

        #region ISearchProviderManager Members

        public void RegisterSearchProvider(string name, Func<ISearchConnection, ISearchProvider> factory)
        {
            _factories.AddOrUpdate(name, factory, (key, oldValue) => factory);
        }

        public IEnumerable<string> RegisteredProviders
        {
            get { return _factories.Keys; }
        }

        public ISearchProvider CurrentProvider
        {
            get { return GetCurrentProvider(); }
        }

        public ISearchConnection CurrentConnection
        {
            get { return GetCurrentConnection(); }
        }

        #endregion

        #region ISearchProvider Members

        public ISearchQueryBuilder QueryBuilder
        {
            get { return GetCurrentProvider().QueryBuilder; }
        }

        public ISearchResults Search(string scope, ISearchCriteria criteria)
        {
            return GetCurrentProvider().Search(scope, criteria);
        }

        public void Index(string scope, string documentType, IDocument document)
        {
            GetCurrentProvider().Index(scope, documentType, document);
        }

        public int Remove(string scope, string documentType, string key, string value)
        {
            return GetCurrentProvider().Remove(scope, documentType, key, value);
        }

        public void RemoveAll(string scope, string documentType)
        {
            GetCurrentProvider().RemoveAll(scope, documentType);
        }

        public void Close(string scope, string documentType)
        {
            GetCurrentProvider().Close(scope, documentType);
        }

        public void Commit(string scope)
        {
            GetCurrentProvider().Commit(scope);
        }

        #endregion

        #region ISearchConnection Members

        public string DataSource
        {
            get { return GetCurrentConnection().DataSource; }
        }

        public string Scope
        {
            get { return GetCurrentConnection().Scope; }
        }

        public string Provider
        {
            get { return GetCurrentConnection().Provider; }
        }

        public string AccessKey
        {
            get { return GetCurrentConnection().AccessKey; }
        }

        #endregion


        private ISearchProvider GetCurrentProvider()
        {
            ISearchProvider result = null;

            var connection = GetCurrentConnection();

            Func<ISearchConnection, ISearchProvider> factory;
            if (_factories.TryGetValue(connection.Provider, out factory))
            {
                result = _cache.Get(
                    _cache.CreateKey("SearchProvider", connection.ConnectionString),
                    () => factory(connection),
                    GetCacheTimeout());
            }

            return result;
        }

        private MySearchConnection GetCurrentConnection()
        {
            var connectionString = GetConnectionString();

            var connection = _cache.Get(
                _cache.CreateKey("SearchConnection", connectionString),
                () => new MySearchConnection(connectionString),
                GetCacheTimeout());

            return connection;
        }

        private string GetConnectionString()
        {
            return _settingsManager.GetValue("VirtoCommerce.Core.Search.ConnectionString", ConnectionHelper.GetConnectionString("SearchConnectionString"));
        }

        private TimeSpan GetCacheTimeout()
        {
            return TimeSpan.FromSeconds(600);
        }


        private class MySearchConnection : SearchConnection
        {
            public string ConnectionString { get; private set; }

            public MySearchConnection(string connectionString)
                : base(connectionString)
            {
                ConnectionString = connectionString;
            }
        }
    }
}
