using System;
using System.Linq;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Services;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.CatalogModule.Data.Services
{
	public class PropertyServiceImpl : ServiceBase, IPropertyService
	{
		private readonly Func<ICatalogRepository> _catalogRepositoryFactory;
		public PropertyServiceImpl(Func<ICatalogRepository> catalogRepositoryFactory)
		{
			_catalogRepositoryFactory = catalogRepositoryFactory;
		}


		#region IPropertyService Members

		public coreModel.Property GetById(string propertyId)
		{
			coreModel.Property retVal = null;
			using (var repository = _catalogRepositoryFactory())
			{
				var dbProperty = repository.GetPropertiesByIds(new string[] { propertyId }).FirstOrDefault();
				if (dbProperty != null)
				{
					dataModel.Catalog dbCatalog = null;
					dataModel.Category dbCategory = null;
					dbCatalog = repository.GetPropertyCatalog(dbProperty.Id);
					if (dbCatalog == null)
					{
						dbCategory = repository.GetPropertyCategory(dbProperty.Id);
						dbCatalog = repository.GetCatalogById(dbCategory.CatalogId) as dataModel.Catalog;
					}
		
					var catalog = dbCatalog.ToCoreModel();
					var category = dbCategory != null ? dbCategory.ToCoreModel(catalog) : null;

					retVal = dbProperty.ToCoreModel(catalog, category);
				}
			}
			return retVal;
		}

		public coreModel.Property[] GetCatalogProperties(string catalogId)
		{
			coreModel.Property[] retVal = null;
			using (var repository = _catalogRepositoryFactory())
			{
				var dbCatalog = repository.GetCatalogById(catalogId);
				var dbCatalogProperties = repository.GetCatalogProperties(dbCatalog);
				var catalog = dbCatalog.ToCoreModel();
				retVal = dbCatalogProperties.Select(x => x.ToCoreModel(catalog, null)).ToArray();
			}
			return retVal;
		}

		public coreModel.Property[] GetCategoryProperties(string categoryId)
		{
			coreModel.Property[] retVal = null;
			using (var repository = _catalogRepositoryFactory())
			{
				var dbCategory = repository.GetCategoryById(categoryId);
				var dbCatalog = repository.GetCatalogById(dbCategory.CatalogId);
				var dbProperties = repository.GetAllCategoryProperties(dbCategory);
			
				var catalog = dbCatalog.ToCoreModel();
				var category = dbCategory.ToCoreModel(catalog);

				retVal = dbProperties.Select(x => x.ToCoreModel(catalog, category)).ToArray();
			}
			return retVal;
		}

		public coreModel.Property Create(coreModel.Property property)
		{
			if (property.CatalogId == null)
			{
				throw new NullReferenceException("property.CatalogId");
			}
		
			var dbProperty = property.ToDataModel();
			using (var repository = _catalogRepositoryFactory())
			{
				if (property.CategoryId != null)
				{
					var dbCategory = repository.GetCategoryById(property.CategoryId);
					repository.SetCategoryProperty(dbCategory, dbProperty);
				}
				else
				{
					var dbCatalog = repository.GetCatalogById(property.CatalogId) as dataModel.Catalog;
					if(dbCatalog == null)
					{
						throw new OperationCanceledException("Add property only to catalog");
					}
					repository.SetCatalogProperty(dbCatalog, dbProperty);
				}
				repository.Add(dbProperty);
				CommitChanges(repository);
			}
			var retVal = GetById(dbProperty.Id);
			return retVal;
		}

		public void Update(coreModel.Property[] properties)
		{
			using (var repository = _catalogRepositoryFactory())
			using (var changeTracker = base.GetChangeTracker(repository))
			{
				var dbProperties = repository.GetPropertiesByIds(properties.Select(x => x.Id).ToArray());

				foreach (var dbProperty in dbProperties)
				{
					var property = properties.FirstOrDefault(x => x.Id == dbProperty.Id);
					if (property != null)
					{
						changeTracker.Attach(property);

						var dbPropertyChanged = property.ToDataModel();
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

		public coreModel.PropertyDictionaryValue[] SearchDictionaryValues(string propertyId, string keyword)
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
