using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Data.Infrastructure;
using foundation = VirtoCommerce.CatalogModule.Data.Model;
using module = VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Data.Services
{
	public class CategoryServiceImpl : ServiceBase, ICategoryService
    {
        private readonly Func<ICatalogRepository> _catalogRepositoryFactory;
	    private readonly CacheManager _cacheManager;
        public CategoryServiceImpl(Func<ICatalogRepository> catalogRepositoryFactory, CacheManager cacheManager)
        {
            _catalogRepositoryFactory = catalogRepositoryFactory;
            _cacheManager = cacheManager;
        }

        #region ICategoryService Members

        public module.Category GetById(string categoryId)
        {
			module.Category retVal = null;
            using (var repository = _catalogRepositoryFactory())
	        {
                var dbCategory = repository.GetCategoryById(categoryId);
				if (dbCategory != null)
				{
					var dbCatalog = repository.GetCatalogById(dbCategory.CatalogId);
					var dbProperties = repository.GetAllCategoryProperties(dbCategory);
					var dbLinks = repository.GetCategoryLinks(categoryId);
					var seoInfos = repository.GetAllSeoInformation(categoryId);

					var catalog = dbCatalog.ToModuleModel();
					var properties = dbProperties.Select(x => x.ToModuleModel(catalog, dbCategory.ToModuleModel(catalog, null, dbLinks)))
												 .ToArray();
					var allParents = repository.GetAllCategoryParents(dbCategory);

					retVal = dbCategory.ToModuleModel(catalog, properties, dbLinks, seoInfos, allParents);
				}
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
				//Need add seo separately
				if (category.SeoInfos != null)
				{
					foreach(var seoInfo in category.SeoInfos)
					{
						var dbSeoInfo = seoInfo.ToFoundation(category);
						repository.Add(dbSeoInfo);
					}
				}

                repository.Add(dbCategory);

                CommitChanges(repository);
            }
            retVal = GetById(dbCategory.Id);
            return retVal;
        }

        public void Update(module.Category[] categories)
        {
            using (var repository = _catalogRepositoryFactory())
			using (var changeTracker = base.GetChangeTracker(repository))
            {
				foreach (var category in categories)
				{
					var dbCategory = repository.GetCategoryById(category.Id) as foundation.Category;
					
					if (dbCategory == null)
					{
						throw new NullReferenceException("dbCategory");
					}

					//Patch SeoInfo  separately
					if (category.SeoInfos != null)
					{
						var dbSeoInfos = new ObservableCollection<foundation.SeoUrlKeyword>(repository.GetAllSeoInformation(category.Id));
						var changedSeoInfos = category.SeoInfos.Select(x => x.ToFoundation(category)).ToList();
						dbSeoInfos.ObserveCollection(x => repository.Add(x), x => repository.Remove(x));

						changedSeoInfos.Patch(dbSeoInfos, (source, target) => source.Patch(target));
					}

					//Patch  Links  separately
					if (category.Links != null)
					{
						var dbLinks = new ObservableCollection<foundation.LinkedCategory>(repository.GetCategoryLinks(category.Id));
						var changedLinks = category.Links.Select(x => x.ToFoundation(category)).ToList();
						dbLinks.ObserveCollection(x => repository.Add(x), x => { repository.Attach(x); repository.Remove(x); });
						changedLinks.Patch(dbLinks, new LinkedCategoryComparer(), (source, target) => source.Patch(target));
					}

					var dbCategoryChanged = category.ToFoundation();
					changeTracker.Attach(dbCategory);
				
					dbCategoryChanged.Patch(dbCategory);
				}
				CommitChanges(repository);

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
