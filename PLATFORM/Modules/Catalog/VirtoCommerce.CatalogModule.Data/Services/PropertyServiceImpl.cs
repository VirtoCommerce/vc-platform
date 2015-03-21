using System;
using System.Linq;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Frameworks.Caching;
using module = VirtoCommerce.Domain.Catalog.Model;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.CatalogModule.Data.Services
{
	public class PropertyServiceImpl : ServiceBase, IPropertyService
	{
		private readonly Func<IFoundationCatalogRepository> _catalogRepositoryFactory;
		private readonly CacheManager _cacheManager;
		public PropertyServiceImpl(Func<IFoundationCatalogRepository> catalogRepositoryFactory, CacheManager cacheManager)
		{
			_catalogRepositoryFactory = catalogRepositoryFactory;
			_cacheManager = cacheManager;
		}


		#region IPropertyService Members

		public module.Property GetById(string propertyId)
		{
			module.Property retVal = null;
			using (var repository = _catalogRepositoryFactory())
			{
				var dbProperty = repository.GetPropertiesByIds(new string[] { propertyId }).FirstOrDefault();
				if (dbProperty != null)
				{
					foundation.Catalog dbCatalog = null;
					foundation.Category dbCategory = null;
					dbCatalog = repository.GetPropertyCatalog(dbProperty.PropertyId);
					if (dbCatalog == null)
					{
						dbCategory = repository.GetPropertyCategory(dbProperty.PropertyId);
						dbCatalog = repository.GetCatalogById(dbCategory.CatalogId) as foundation.Catalog;
					}
		
					var catalog = dbCatalog.ToModuleModel();
					var category = dbCategory != null ? dbCategory.ToModuleModel(catalog) : null;

					retVal = dbProperty.ToModuleModel(catalog, category);
				}
			}
			return retVal;
		}

		public module.Property[] GetCatalogProperties(string catalogId)
		{
			module.Property[] retVal = null;
			using (var repository = _catalogRepositoryFactory())
			{
				var dbCatalog = repository.GetCatalogById(catalogId);
				var dbCatalogProperties = repository.GetCatalogProperties(dbCatalog);
				var catalog = dbCatalog.ToModuleModel();
				retVal = dbCatalogProperties.Select(x => x.ToModuleModel(catalog, null)).ToArray();
			}
			return retVal;
		}

		public module.Property[] GetCategoryProperties(string categoryId)
		{
			module.Property[] retVal = null;
			using (var repository = _catalogRepositoryFactory())
			{
				var dbCategory = repository.GetCategoryById(categoryId);
				var dbCatalog = repository.GetCatalogById(dbCategory.CatalogId);
				var dbProperties = repository.GetAllCategoryProperties(dbCategory);
			
				var catalog = dbCatalog.ToModuleModel();
				var category = dbCategory.ToModuleModel(catalog);

				retVal = dbProperties.Select(x => x.ToModuleModel(catalog, category)).ToArray();
			}
			return retVal;
		}

		public module.Property Create(module.Property property)
		{
			if (property.CatalogId == null)
			{
				throw new NullReferenceException("property.CatalogId");
			}
		
			var dbProperty = property.ToFoundation();
			using (var repository = _catalogRepositoryFactory())
			{
				if (property.CategoryId != null)
				{
					var dbCategory = repository.GetCategoryById(property.CategoryId);
					repository.SetCategoryProperty(dbCategory, dbProperty);
				}
				else
				{
					var dbCatalog = repository.GetCatalogById(property.CatalogId) as foundation.Catalog;
					if(dbCatalog == null)
					{
						throw new OperationCanceledException("Add property only to catalog");
					}
					repository.SetCatalogProperty(dbCatalog, dbProperty);
				}
				repository.Add(dbProperty);
				CommitChanges(repository);
			}
			var retVal = GetById(dbProperty.PropertyId);
			return retVal;
		}

		public void Update(module.Property[] properties)
		{
			using (var repository = _catalogRepositoryFactory())
			using (var changeTracker = base.GetChangeTracker(repository))
			{
				var dbProperties = repository.GetPropertiesByIds(properties.Select(x => x.Id).ToArray());

				foreach (var dbProperty in dbProperties)
				{
					var property = properties.FirstOrDefault(x => x.Id == dbProperty.PropertyId);
					if (property != null)
					{
						changeTracker.Attach(property);

						var dbPropertyChanged = property.ToFoundation();
						dbPropertyChanged.Patch(dbProperty);
					}
				}
				CommitChanges(repository);
			}
		}


		public void Delete(string[] propertyIds)
		{
			using (var repository = _catalogRepositoryFactory())
			{
				var dbProperties = repository.GetPropertiesByIds(propertyIds);

                foreach (var dbProperty in dbProperties)
				{
					repository.Remove(dbProperty);
				}

				CommitChanges(repository);
			}
		}

		public module.PropertyDictionaryValue[] SearchDictionaryValues(string propertyId, string keyword)
		{
			var property = GetById(propertyId);
			var query = property.DictionaryValues.AsQueryable();
			//TODO: Replace to search in db
            if (!String.IsNullOrEmpty(keyword))
			{
				query = query.Where(x => x.Value.ToLowerInvariant().Contains(keyword.ToLowerInvariant()));
			}
			return query.ToArray();
		}
		#endregion
	}
}
