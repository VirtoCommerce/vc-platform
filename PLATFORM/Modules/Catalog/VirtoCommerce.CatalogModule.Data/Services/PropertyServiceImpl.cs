using System;
using System.Linq;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Services;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Core.Caching;
using System.Collections.Generic;

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
            return GetByIds(new[] { propertyId }).FirstOrDefault();
		}

        public coreModel.Property[] GetByIds(string[] propertyIds)
        {
            var retVal = new List<coreModel.Property>();
            using (var repository = _catalogRepositoryFactory())
            {
                var dbProperties = repository.GetPropertiesByIds(propertyIds);
                foreach (var dbProperty in dbProperties)
                {
                    var catalog = dbProperty.Catalog.ToCoreModel();
                    var category = dbProperty.Category != null ? dbProperty.Category.ToCoreModel(catalog) : null;
                    var property = dbProperty.ToCoreModel(catalog, category);
                    retVal.Add(property);
                }
            }
            return retVal.ToArray();
        }

        public coreModel.Property[] GetCatalogProperties(string catalogId)
		{
			coreModel.Property[] retVal = null;
			using (var repository = _catalogRepositoryFactory())
			{
				var dbCatalog = repository.GetCatalogById(catalogId);
				var catalog = dbCatalog.ToCoreModel();
                retVal = GetByIds(dbCatalog.Properties.Where(x => x.CategoryId == null).Select(x => x.Id).ToArray());
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

				//property override - need leave only property has a min distance to target category 
				//Algorithm based on index property in resulting list (property with min index will more closed to category)
				var propertyGroups = dbProperties.Select((x, index) => new { PropertyName = x.Name.ToLowerInvariant(), Property = x, Index = index }).GroupBy(x => x.PropertyName);
				dbProperties = propertyGroups.Select(x => x.OrderBy(y => y.Index).First().Property).ToArray();

				retVal = retVal = GetByIds(dbProperties.Select(x => x.Id).ToArray()); 
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
                    dbCategory.Properties.Add(dbProperty);
				}
				else
				{
					var dbCatalog = repository.GetCatalogById(property.CatalogId) as dataModel.Catalog;
					if(dbCatalog == null)
					{
						throw new OperationCanceledException("Add property only to catalog");
					}
                    dbCatalog.Properties.Add(dbProperty);
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
						changeTracker.Attach(dbProperty);

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
            using (var repository = _catalogRepositoryFactory())
            {
                var query = repository.PropertyDictionaryValues.Where(x => x.PropertyId == propertyId);
                if (!String.IsNullOrEmpty(keyword))
                {
                    query = query.Where(x => x.Value.Contains(keyword));
                }
                return query.ToArray().Select(x=> x.ToCoreModel()).ToArray();
            }
		}
		#endregion
	}
}
