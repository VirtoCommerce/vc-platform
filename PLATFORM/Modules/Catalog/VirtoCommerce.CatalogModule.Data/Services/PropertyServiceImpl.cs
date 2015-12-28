using System;
using System.Linq;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Data.Infrastructure;
using coreModel = VirtoCommerce.Domain.Catalog.Model;

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
            using (var repository = _catalogRepositoryFactory())
            {
                var dbProperties = repository.GetPropertiesByIds(propertyIds);
                var result = dbProperties.Select(dbProperty => dbProperty.ToCoreModel()).ToArray();
                return result;
            }
        }

        public coreModel.Property[] GetAllCatalogProperties(string catalogId)
        {
            using (var repository = _catalogRepositoryFactory())
            {
                var dbProperties = repository.GetAllCatalogProperties(catalogId);
                var result = dbProperties.Select(dbProperty => dbProperty.ToCoreModel()).ToArray();
                return result;
            }
        }


        public coreModel.Property[] GetAllProperties()
        {
            using (var repository = _catalogRepositoryFactory())
            {
                return GetByIds(repository.Properties.Select(x => x.Id).ToArray());
            }
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
                    var dbCategory = repository.GetCategoriesByIds(new[] { property.CategoryId }, coreModel.CategoryResponseGroup.Info).FirstOrDefault();
                    if (dbCategory == null)
                    {
                        throw new NullReferenceException("dbCategory");
                    }
                    dbCategory.Properties.Add(dbProperty);
                }
                else
                {
                    var dbCatalog = repository.GetCatalogById(property.CatalogId);
                    if (dbCatalog == null)
                    {
                        throw new NullReferenceException("dbCatalog");
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
            using (var changeTracker = GetChangeTracker(repository))
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
                return query.ToArray().Select(x => x.ToCoreModel()).ToArray();
            }
        }
        #endregion
    }
}
