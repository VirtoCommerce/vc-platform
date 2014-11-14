using System;
using System.Linq;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Repositories;
using VirtoCommerce.CatalogModule.Services;
using VirtoCommerce.Foundation.Frameworks.Caching;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using module = VirtoCommerce.CatalogModule.Model;

namespace VirtoCommerce.CatalogModule.Data.Services
{
    public class CategoryServiceImpl : ModuleServiceBase, ICategoryService
    {
        private readonly Func<IFoundationCatalogRepository> _catalogRepositoryFactory;
        private readonly CacheManager _cacheManager;
        public CategoryServiceImpl(Func<IFoundationCatalogRepository> catalogRepositoryFactory, CacheManager cacheManager)
        {
            _catalogRepositoryFactory = catalogRepositoryFactory;
            _cacheManager = cacheManager;
        }

        #region ICategoryService Members

        public module.Category GetById(string categoryId)
        {
            Model.Category retVal;
            using (var repository = _catalogRepositoryFactory())
            {
                var dbCategory = repository.GetCategoryById(categoryId);
                var dbCatalog = repository.GetCatalogById(dbCategory.CatalogId);
                var dbProperties = repository.GetAllCategoryProperties(dbCategory);
			
                var catalog = dbCatalog.ToModuleModel();
				var properties = dbProperties.Select(x => x.ToModuleModel(catalog, dbCategory.ToModuleModel(catalog))).ToArray();

				retVal = dbCategory.ToModuleModel(catalog, properties);
            }
            return retVal;
        }

        public module.Category Create(module.Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            var dbCategory = category.ToFoundation();
            module.Category retVal = null;
            using (var repository = _catalogRepositoryFactory())
            {
                repository.Add(dbCategory);
                CommitChanges(repository);
            }
            retVal = GetById(dbCategory.CategoryId);
            return retVal;
        }

        public void Update(module.Category[] categories)
        {
            using (var repository = _catalogRepositoryFactory())
            {
                using (var changeTracker = base.GetChangeTracker(repository))
                {
					
                    foreach (var category in categories)
                    {
                        var dbCategory = repository.GetCategoryById(category.Id) as foundation.Category;

                        if (dbCategory == null)
                        {
                            throw new NullReferenceException("dbCategory");
                        }
                        var dbCategoryChanged = category.ToFoundation();

					    changeTracker.Attach(dbCategory);

						//It need prevent real  adding link to category LinkedCategories because EF changed relations
						//automaticly to different
						changeTracker.AddAction = (x) =>
						{
							dbCategory.LinkedCategories.Remove((foundation.LinkedCategory)x);
							repository.Add(x);
						};
						
                        dbCategoryChanged.Patch(dbCategory);

                    }
                    CommitChanges(repository);
                }
            }
        }

        public void Delete(string[] categoryIds)
        {
            using (var repository = _catalogRepositoryFactory())
            {
                foreach (var categoryId in categoryIds)
                {
                    var dbCategory = repository.GetCategoryById(categoryId);
                    repository.Remove(dbCategory);
                }
                CommitChanges(repository);
            }
        }

        #endregion
    }
}
