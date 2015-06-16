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
using VirtoCommerce.Domain.Commerce.Services;
using System.Collections.ObjectModel;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Data.Services
{
	public class CatalogServiceImpl : ServiceBase, ICatalogService
	{
		private readonly Func<ICatalogRepository> _catalogRepositoryFactory;
		private readonly ICommerceService _commerceService;
		public CatalogServiceImpl(Func<ICatalogRepository> catalogRepositoryFactory, ICommerceService commerceService)
		{
			_catalogRepositoryFactory = catalogRepositoryFactory;
			_commerceService = commerceService;
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
				var seoInfos = _commerceService.GetSeoKeywordsForEntity(catalogId).ToArray();
				retVal = dbCatalogBase.ToCoreModel(properties, seoInfos);

			}
			return retVal;
		}

		public coreModel.Catalog Create(coreModel.Catalog catalog)
		{
			var dbCatalog = catalog.ToDataModel();
			coreModel.Catalog retVal = null;
			using (var repository = _catalogRepositoryFactory())
			{
				//Need add seo separately
				if (catalog.SeoInfos != null)
				{
					foreach (var seoInfo in catalog.SeoInfos)
					{
						var dbSeoInfo = seoInfo.ToCoreModel(dbCatalog);
						_commerceService.UpsertSeoKeyword(dbSeoInfo);
					}
				}

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

					//Patch SeoInfo  separately
					if (catalog.SeoInfos != null)
					{
						var dbSeoInfos = new ObservableCollection<SeoUrlKeyword>(_commerceService.GetSeoKeywordsForEntity(catalog.Id));
						var changedSeoInfos = catalog.SeoInfos.Select(x => x.ToCoreModel(dbCatalog)).ToList();
						dbSeoInfos.ObserveCollection(x => _commerceService.UpsertSeoKeyword(x), x => _commerceService.DeleteSeoKeywords(new string[] { x.Id }));

						changedSeoInfos.Patch(dbSeoInfos, (source, target) => _commerceService.UpsertSeoKeyword(source));
					}
		
				}

				CommitChanges(repository);
			}
		}

		public void Delete(string[] catalogIds)
		{
			using (var repository = _catalogRepositoryFactory())
			{
				repository.RemoveCatalogs(catalogIds);
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
