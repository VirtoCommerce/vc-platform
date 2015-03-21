using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Caching;
using VirtoCommerce.Framework.Web.Settings;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using module = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Data.Services
{
    public class CatalogSearchServiceImpl : ICatalogSearchService
    {
	    private readonly Func<IFoundationCatalogRepository> _catalogRepositoryFactory;
        private readonly IItemService _itemService;
        private readonly ICatalogService _catalogService;
        private readonly ICategoryService _categoryService;
        private readonly CacheManager _cacheManager;

        public CatalogSearchServiceImpl(Func<IFoundationCatalogRepository> catalogRepositoryFactory, IItemService itemService,
                                        ICatalogService catalogService, ICategoryService categoryService, CacheManager cacheManager = null)
        {
            _catalogRepositoryFactory = catalogRepositoryFactory;
            _itemService = itemService;
            _catalogService = catalogService;
            _categoryService = categoryService;
			_cacheManager = cacheManager ?? CacheManager.NoCache;
        }

		public module.SearchResult Search(module.SearchCriteria criteria)
		{
			var cacheKey = CacheKey.Create(Constants.CatalogCachePrefix + ".Search", criteria.ToString());
			var result = _cacheManager.Get(cacheKey, () =>
				{
					var retVal = new module.SearchResult();
					var taskList = new List<Task>();

					if ((criteria.ResponseGroup & module.ResponseGroup.WithProducts) == module.ResponseGroup.WithProducts)
					{
						taskList.Add(Task.Factory.StartNew(() => SearchItems(criteria, retVal)));
					}
					if ((criteria.ResponseGroup & module.ResponseGroup.WithCatalogs) == module.ResponseGroup.WithCatalogs)
					{
						taskList.Add(Task.Factory.StartNew(() => SearchCatalogs(criteria, retVal)));
					}
					if ((criteria.ResponseGroup & module.ResponseGroup.WithCategories) == module.ResponseGroup.WithCategories)
					{
						taskList.Add(Task.Factory.StartNew(() => SearchCategories(criteria, retVal)));
					}
					Task.WaitAll(taskList.ToArray());

					return retVal;
				});
			return result;
		}

        private void SearchCategories(module.SearchCriteria criteria, module.SearchResult result)
        {
            // TODO: optimize for performance, need to eliminate number of database queries
            // 1. Catalog should either be passed or loaded using caching
            // 2. Categories should be loaded by passing array of ids instead of parallel locading one by one
            using (var repository = _catalogRepositoryFactory())
            {
                if (!String.IsNullOrEmpty(criteria.CatalogId))
                {
                    var query = repository.Categories.Where(x => x.CatalogId == criteria.CatalogId);

					var cacheKey = CacheKey.Create(Constants.CatalogCachePrefix + ".GetCatalogById", criteria.CatalogId);
					var dbCatalog = _cacheManager.Get(cacheKey, () => repository.GetCatalogById(criteria.CatalogId));

                    var isVirtual = dbCatalog is foundation.VirtualCatalog;
                    if (!String.IsNullOrEmpty(criteria.CategoryId))
                    {
                        //var dbCategory = repository.GetCategoryById(criteria.CategoryId);

                        if (isVirtual)
                        {
                            //Need return all linked categories also
                            var allLinkedPhysicalCategoriesIds = repository.Categories.OfType<foundation.LinkedCategory>()
                                                    .Where(x => x.LinkedCategoryId == criteria.CategoryId)
                                                    .Select(x => x.ParentCategoryId)
                                                    .ToArray();
                            //Search in all catalogs
                            query = repository.Categories;
                            query = query.Where(x => x.ParentCategoryId == criteria.CategoryId || allLinkedPhysicalCategoriesIds.Contains(x.CategoryId));
                        }
                        else
                        {
                            query = query.Where(x => x.ParentCategoryId == criteria.CategoryId);
                        }
                    }
                    else if (!String.IsNullOrEmpty(criteria.CatalogId))
                    {
                        query = query.Where(x => x.CatalogId == criteria.CatalogId && (x.ParentCategoryId == null || criteria.GetAllCategories));
                        if (isVirtual)
                        {
                            //Need return all linked categories 
                            var allLinkedCategoriesIds = repository.GetCatalogLinks(criteria.CatalogId).Select(x => x.ParentCategoryId).ToArray();
                            //Search in all catalogs
                            query = repository.Categories;
                            query = query.Where(x => (x.CatalogId == criteria.CatalogId && (x.ParentCategoryId == null || criteria.GetAllCategories)) || allLinkedCategoriesIds.Contains(x.CategoryId));
                        }
                    }

                    var categoryIds = query.OfType<foundation.Category>().Select(x => x.CategoryId).ToArray();

                    var categories = new ConcurrentBag<module.Category>();
                    var parallelOptions = new ParallelOptions
                    {
                        MaxDegreeOfParallelism = 10
                    };
                    Parallel.ForEach(categoryIds, parallelOptions, (x) =>
                    {
                        var category = _categoryService.GetById(x);

                        categories.Add(category);

                    });

                    //Must order by priority
                    result.Categories = categories.OrderByDescending(x => x.Priority).ThenBy(x => x.Name).ToList();
                }
            }
        }

        private void SearchCatalogs(module.SearchCriteria criteria, module.SearchResult result)
        {
            using (var repository = _catalogRepositoryFactory())
            {
                var catalogIds = repository.Catalogs.Select(x => x.CatalogId).ToArray();
                var catalogs = new ConcurrentBag<module.Catalog>();
                var parallelOptions = new ParallelOptions
                {
                    MaxDegreeOfParallelism = 10
                };
                Parallel.ForEach(catalogIds, parallelOptions, (x) =>
                {
                    var catalog = _catalogService.GetById(x);
                    catalogs.Add(catalog);
                });
                result.Catalogs = catalogs.OrderByDescending(x => x.Name).ToList();
            }
        }

        private void SearchItems(module.SearchCriteria criteria, module.SearchResult result)
        {
            using (var repository = _catalogRepositoryFactory())
            {
                var query = repository.Items;
				if ((criteria.ResponseGroup & module.ResponseGroup.WithVariations) != module.ResponseGroup.WithVariations)
				{
					query = query.Where(x => x.IsActive);
				}

                if (!String.IsNullOrEmpty(criteria.CategoryId))
                {
                    query = query.Where(x => x.CategoryItemRelations.Any(c => c.CategoryId == criteria.CategoryId));
                }
                else if (!String.IsNullOrEmpty(criteria.CatalogId))
                {
                    query = query.Where(x => x.CatalogId == criteria.CatalogId && !x.CategoryItemRelations.Any());
                }
				
                result.TotalCount = query.Count();

                var itemIds = query.OrderByDescending(x => x.Name)
                                   .Skip(criteria.Start)
                                   .Take(criteria.Count)
                                   .Select(x => x.ItemId)
                                   .ToArray();

				var productResponseGroup = module.ItemResponseGroup.ItemInfo | module.ItemResponseGroup.ItemAssets;
				if ((criteria.ResponseGroup & module.ResponseGroup.WithProperties) ==module.ResponseGroup.WithProperties)
				{
					productResponseGroup |= module.ItemResponseGroup.ItemProperties;
				}
				if ((criteria.ResponseGroup & module.ResponseGroup.WithVariations) == module.ResponseGroup.WithVariations)
				{
					productResponseGroup |= module.ItemResponseGroup.Variations;
				}

				var products = _itemService.GetByIds(itemIds, productResponseGroup);
                result.Products = products.OrderByDescending(x => x.Name).ToList();
            }
        }
    }
}
