using System;
using System.Linq;
using VirtoCommerce.Foundation.Search.Repositories;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Search.Factories;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.Model;
using VirtoCommerce.Foundation.Security.Services;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Data.Search
{
    public class DSSearchClient : DSClientBase, IBuildSettingsRepository
    {
		[InjectionConstructor]
		public DSSearchClient(ISearchEntityFactory catalogEntityFactory, ISecurityTokenInjector tokenInjector, IServiceConnectionFactory connFactory)
			: base(connFactory.GetConnectionString(SearchConfiguration.Instance.Connection.DataServiceUri), catalogEntityFactory, tokenInjector)
		{
		}

        public DSSearchClient(Uri serviceUri, ISearchEntityFactory entityFactory, ISecurityTokenInjector tokenInjector)
			: base(serviceUri, entityFactory, tokenInjector)
		{
		}

        #region IBuildSettingsRepository Members
        public IQueryable<BuildSettings> BuildSettings
		{
            get { return GetAsQueryable<BuildSettings>(); }
		}
		#endregion
    }
}
