using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Repositories;
using VirtoCommerce.CatalogModule.Services;
using VirtoCommerce.Foundation.Frameworks.Caching;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using foundationConfig = VirtoCommerce.Foundation.AppConfig.Model;
using module = VirtoCommerce.CatalogModule.Model;

namespace VirtoCommerce.CatalogModule.Data.Services
{
    public class CategoryServiceImpl : ModuleServiceBase, ICategoryService
    {
        private readonly Func<IFoundationCatalogRepository> _catalogRepositoryFactory;
		private readonly Func<IFoundationAppConfigRepository> _appConfigRepositoryFactory;
        private readonly CacheManager _cacheManager;
        public CategoryServiceImpl(Func<IFoundationCatalogRepository> catalogRepositoryFactory, Func<IFoundationAppConfigRepository> appConfigRepositoryFactory, 
								   CacheManager cacheManager)
        {
            _catalogRepositoryFactory = catalogRepositoryFactory;
			_appConfigRepositoryFactory = appConfigRepositoryFactory;
            _cacheManager = cacheManager;
        }

        #region ICategoryService Members

        public module.Category GetById(string categoryId)
        {
            Model.Category retVal;
            using (var repository = _catalogRepositoryFactory())
			using (var appConfigRepository = _appConfigRepositoryFactory())
            {
                var dbCategory = repository.GetCategoryById(categoryId);
                var dbCatalog = repository.GetCatalogById(dbCategory.CatalogId);
                var dbProperties = repository.GetAllCategoryProperties(dbCategory);
				var dbLinks = repository.GetCategoryLinks(categoryId);
				var seoInfos = appConfigRepository.GetAllSeoInformation(categoryId);
			
                var catalog = dbCatalog.ToModuleModel();
				var properties = dbProperties.Select(x => x.ToModuleModel(catalog, dbCategory.ToModuleModel(catalog, null,  dbLinks)))
											 .ToArray();

				retVal = dbCategory.ToModuleModel(catalog, properties, dbLinks, seoInfos);
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
			using (var appConfigRepository = _appConfigRepositoryFactory())
            {
				//Need add seo separately
				if (category.SeoInfos != null)
				{
					foreach(var seoInfo in category.SeoInfos)
					{
						var dbSeoInfo = seoInfo.ToFoundation(category);
						appConfigRepository.Add(dbSeoInfo);
					}
				}

                repository.Add(dbCategory);

                CommitChanges(repository);
				CommitChanges(appConfigRepository);
            }
            retVal = GetById(dbCategory.CategoryId);
            return retVal;
        }

        public void Update(module.Category[] categories)
        {
            using (var repository = _catalogRepositoryFactory())
			using (var appConfigRepository = _appConfigRepositoryFactory())
			using (var changeTracker = base.GetChangeTracker(repository))
            {
				foreach (var category in categories)
				{
					var dbCategory = repository.GetCategoryById(category.Id) as foundation.Category;
					dbCategory.LinkedCategories.AddRange(repository.GetCategoryLinks(category.Id));

					if (dbCategory == null)
					{
						throw new NullReferenceException("dbCategory");
					}

					//Patch seoInfo
					if (category.SeoInfos != null)
					{
						var dbSeoInfos = new ObservableCollection<foundationConfig.SeoUrlKeyword>(appConfigRepository.GetAllSeoInformation(category.Id));
						var changedSeoInfos = category.SeoInfos.Select(x => x.ToFoundation(category)).ToList();
						dbSeoInfos.ObserveCollection(x => appConfigRepository.Add(x), x => appConfigRepository.Remove(x));

						changedSeoInfos.Patch(dbSeoInfos, new SeoInfoComparer(), (source, target) => source.Patch(target));
					}

					var dbCategoryChanged = category.ToFoundation();
					changeTracker.Attach(dbCategory);
					//It need prevent real  adding link to category LinkedCategories because EF changed relations
					//automaticly to different
					changeTracker.AddAction = (x) =>
					{
						if (x is foundation.LinkedCategory)
						{
							dbCategory.LinkedCategories.Remove((foundation.LinkedCategory)x);
						}
						repository.Add(x);
					};
					changeTracker.RemoveAction = (x) =>
					{
						if (x is foundation.LinkedCategory)
						{
							repository.Attach(x);
						}
						repository.Remove(x);
					};

					dbCategoryChanged.Patch(dbCategory);
					//Need prevent storing links with category
					dbCategory.LinkedCategories.Clear();
				}
				CommitChanges(repository);
				CommitChanges(appConfigRepository);

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
