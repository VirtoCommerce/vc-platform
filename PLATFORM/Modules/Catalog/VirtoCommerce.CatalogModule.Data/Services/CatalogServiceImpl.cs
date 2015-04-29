using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Data.Infrastructure;
using foundation = VirtoCommerce.CatalogModule.Data.Model;
using module = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Data.Services
{
	public class CatalogServiceImpl : ServiceBase, ICatalogService
	{
		private readonly Func<ICatalogRepository> _catalogRepositoryFactory;
		private readonly CacheManager _cacheManager;
		public CatalogServiceImpl(Func<ICatalogRepository> catalogRepositoryFactory, CacheManager cacheManager = null)
		{
			_catalogRepositoryFactory = catalogRepositoryFactory;
			_cacheManager = cacheManager ?? CacheManager.NoCache;
		}

		#region ICatalogService Members

		public module.Catalog GetById(string catalogId)
		{
			module.Catalog retVal = null;
			using (var repository = _catalogRepositoryFactory())
			{
				var dbCatalogBase = repository.GetCatalogById(catalogId);

				var dbProperties = repository.GetCatalogProperties(dbCatalogBase);
				var properties = dbProperties.Select(x => x.ToModuleModel(dbCatalogBase.ToModuleModel(), null)).ToArray();
				retVal = dbCatalogBase.ToModuleModel(properties);
			}
			return retVal;
		}

		public module.Catalog Create(module.Catalog catalog)
		{
			var dbCatalog = catalog.ToFoundation();
			module.Catalog retVal = null;
			using (var repository = _catalogRepositoryFactory())
			{
				repository.Add(dbCatalog);
				CommitChanges(repository);
			}
			retVal = GetById(dbCatalog.Id);
			return retVal;
		}

		public void Update(module.Catalog[] catalogs)
		{
			using (var repository = _catalogRepositoryFactory())
			using (var changeTracker = base.GetChangeTracker(repository))
			{
				foreach (var catalog in catalogs)
				{
					var dbCatalog = repository.GetCatalogById(catalog.Id);
					if (dbCatalog == null)
					{
						throw new NullReferenceException("dbCatalog");
					}
					var dbCatalogChanged = catalog.ToFoundation();

					changeTracker.Attach(dbCatalog);
					dbCatalogChanged.Patch(dbCatalog);
				}

				CommitChanges(repository);
			}
		}

		public void Delete(string[] catalogIds)
		{
			using (var repository = _catalogRepositoryFactory())
			{
				foreach (var catalogId in catalogIds)
				{
					var dbCatalog = repository.GetCatalogById(catalogId);
					repository.Remove(dbCatalog);
				}
				CommitChanges(repository);
			}
		}


		public IEnumerable<module.Catalog> GetCatalogsList()
		{
			var retVal = new List<module.Catalog>();
			using (var repository = _catalogRepositoryFactory())
			{
				foreach(var catalogBase in repository.Catalogs)
				{
					retVal.Add(catalogBase.ToModuleModel());
				}
			}
			return retVal;
		}

		#endregion
	}
	
}
