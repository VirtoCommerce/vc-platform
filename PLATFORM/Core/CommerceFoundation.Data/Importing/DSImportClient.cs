using Microsoft.Practices.Unity;
using System;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Importing;
using VirtoCommerce.Foundation.Importing.Factories;
using VirtoCommerce.Foundation.Importing.Model;
using VirtoCommerce.Foundation.Importing.Repositories;
using VirtoCommerce.Foundation.Security.Services;

namespace VirtoCommerce.Foundation.Data.Importing
{
	public class DSImportClient : DSClientBase, IImportRepository
	{
		[InjectionConstructor]
		public DSImportClient(IImportJobEntityFactory entityFactory, ISecurityTokenInjector tokenInjector, IServiceConnectionFactory connFactory)
			: base(connFactory.GetConnectionString(ImportConfiguration.Instance.Connection.DataServiceUri), entityFactory, tokenInjector)
		{
		}

        public DSImportClient(Uri serviceUri, IImportJobEntityFactory entityFactory, ISecurityTokenInjector tokenInjector)
			: base(serviceUri, entityFactory, tokenInjector)
		{
		}

		#region IImportJobRepository Members
		
		public IQueryable<ImportJob> ImportJobs
		{
			get { return GetAsQueryable<ImportJob>(); }
		}
		
		public IQueryable<MappingItem> MappingItems
		{
			get { return GetAsQueryable<MappingItem>(); }
		}
		
		#endregion
	}
}
