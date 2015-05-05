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
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Data.Services
{
	public class CatalogServiceImpl : ServiceBase, ICatalogService
	{
		private readonly Func<ICatalogRepository> _catalogRepositoryFactory;
		public CatalogServiceImpl(Func<ICatalogRepository> catalogRepositoryFactory)
		{
			_catalogRepositoryFactory = catalogRepositoryFactory;
		}

		#region ICatalogService Members

		public coreModel.Catalog GetById(string catalogId)
		{
			coreModel.Catalog retVal = null;
			using (var repository = _catalogRepositoryFactory())
			{
				var dbCatalogBase = repository.GetCatalogById(catalogId);

				var dbProperties = repository.GetCatalogProperties(dbCatalogBase);
				var properties = dbProperties.Select(x => x.ToCoreModel(dbCatalogBase.ToCoreModel(), null)).ToArray();
				retVal = dbCatalogBase.ToCoreModel(properties);
			}
			return retVal;
		}

		public coreModel.Catalog Create(coreModel.Catalog catalog)
		{
			var dbCatalog = catalog.ToDataModel();
			coreModel.Catalog retVal = null;
			using (var repository = _catalogRepositoryFactory())
			{
				repository.Add(dbCatalog);
				CommitChanges(repository);
			}
			retVal = GetById(dbCatalog.Id);
			return retVal;
		}

		public void Update(coreModel.Catalog[] catalogs)
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
					var dbCatalogChanged = catalog.ToDataModel();

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


		public IEnumerable<coreModel.Catalog> GetCatalogsList()
		{
			var retVal = new List<coreModel.Catalog>();
			using (var repository = _catalogRepositoryFactory())
			{
				foreach(var catalogBase in repository.Catalogs)
				{
					retVal.Add(catalogBase.ToCoreModel());
				}
			}
			return retVal;
		}

		#endregion
	}
	
}
