using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Caching;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Commerce.Services;

namespace VirtoCommerce.CatalogModule.Data.Services
{
    public class CatalogSearchServiceImpl : ICatalogSearchService
    {
	    private readonly Func<ICatalogRepository> _catalogRepositoryFactory;
        private readonly IItemService _itemService;
        private readonly ICatalogService _catalogService;
        private readonly ICategoryService _categoryService;
		private readonly ICommerceService _commerceService;

        public CatalogSearchServiceImpl(Func<ICatalogRepository> catalogRepositoryFactory, IItemService itemService,
                                        ICatalogService catalogService, ICategoryService categoryService, ICommerceService commerceService)
        {
            _catalogRepositoryFactory = catalogRepositoryFactory;
            _itemService = itemService;
            _catalogService = catalogService;
            _categoryService = categoryService;
			_commerceService = commerceService;
		}

		public coreModel.SearchResult Search(coreModel.SearchCriteria criteria)
		{
			var retVal = new coreModel.SearchResult();
			var taskList = new List<Task>();

			if ((criteria.ResponseGroup & coreModel.ResponseGroup.WithProducts) == coreModel.ResponseGroup.WithProducts)
			{
				taskList.Add(Task.Factory.StartNew(() => SearchItems(criteria, retVal)));
			}
			if ((criteria.ResponseGroup & coreModel.ResponseGroup.WithCatalogs) == coreModel.ResponseGroup.WithCatalogs)
			{
				taskList.Add(Task.Factory.StartNew(() => SearchCatalogs(criteria, retVal)));
			}
			if ((criteria.ResponseGroup & coreModel.ResponseGroup.WithCategories) == coreModel.ResponseGroup.WithCategories)
			{
				taskList.Add(Task.Factory.StartNew(() => SearchCategories(criteria, retVal)));
			}
			Task.WaitAll(taskList.ToArray());

			return retVal;
		}

        private void SearchCategories(coreModel.SearchCriteria criteria, coreModel.SearchResult result)
        {
            // TODO: optimize for performance, need to eliminate number of database queries
            // 1. Catalog should either be passed or loaded using caching
            // 2. Categories should be loaded by passing array of ids instead of parallel locading one by one
            using (var repository = _catalogRepositoryFactory())
            {
                if (!String.IsNullOrEmpty(criteria.CatalogId))
                {
                    var query = repository.Categories.Where(x => x.CatalogId == criteria.CatalogId);

					var dbCatalog =  repository.GetCatalogById(criteria.CatalogId);

                    var isVirtual = dbCatalog is dataModel.VirtualCatalog;
                    if (!String.IsNullOrEmpty(criteria.CategoryId))
                    {
                        //var dbCategory = repository.GetCategoryById(criteria.CategoryId);

                        if (isVirtual)
                        {
                            //Need return all linked categories also
                            var allLinkedPhysicalCategoriesIds = repository.Categories.OfType<dataModel.LinkedCategory>()
                                                    .Where(x => x.LinkedCategoryId == criteria.CategoryId)
                                                    .Select(x => x.ParentCategoryId)
                                                    .ToArray();
                            //Search in all catalogs
                            query = repository.Categories;
                            query = query.Where(x => x.ParentCategoryId == criteria.CategoryId || allLinkedPhysicalCategoriesIds.Contains(x.Id));
                        }
                        else
                        {
                            query = query.Where(x => x.ParentCategoryId == criteria.CategoryId);
                        }
                    }
					else if(!String.IsNullOrEmpty(criteria.Code))
					{
						query = query.Where(x => x.Code == criteria.Code);
					}
					else if (!String.IsNullOrEmpty(criteria.SeoKeyword))
					{
						var urlKeyword = _commerceService.GetSeoKeywordsByKeyword(criteria.SeoKeyword).Where(x => x.KeywordType == (int)SeoUrlKeywordTypes.Category).FirstOrDefault();
						if(urlKeyword == null)
						{
							query = query.Where(x=> false);
						}
						else
						{
							query = query.Where(x => x.Id == urlKeyword.KeywordValue);
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
                            query = query.Where(x => (x.CatalogId == criteria.CatalogId && (x.ParentCategoryId == null || criteria.GetAllCategories)) || allLinkedCategoriesIds.Contains(x.Id));
                        }
                    }

                    var categoryIds = query.OfType<dataModel.Category>().Select(x => x.Id).ToArray();

                    var categories = new ConcurrentBag<coreModel.Category>();
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

        private void SearchCatalogs(coreModel.SearchCriteria criteria, coreModel.SearchResult result)
        {
            using (var repository = _catalogRepositoryFactory())
            {
                var catalogIds = repository.Catalogs.Select(x => x.Id).ToArray();
                var catalogs = new ConcurrentBag<coreModel.Catalog>();
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

        private void SearchItems(coreModel.SearchCriteria criteria, coreModel.SearchResult result)
        {
            using (var repository = _catalogRepositoryFactory())
            {
				var query = repository.Items;
				if ((criteria.ResponseGroup & coreModel.ResponseGroup.WithVariations) != coreModel.ResponseGroup.WithVariations)
				{
					query = query.Where(x => x.IsActive);
				}

                if (!String.IsNullOrEmpty(criteria.CategoryId))
                {
                    query = query.Where(x=> x.CategoryItemRelations.Any(c => c.CategoryId == criteria.CategoryId));
                }
                else if (!String.IsNullOrEmpty(criteria.CatalogId))
                {
                    query = query.Where(x => x.CatalogId == criteria.CatalogId && !x.CategoryItemRelations.Any());
                }

				if (!String.IsNullOrEmpty(criteria.Code))
				{
					query = query.Where(x => x.Code == criteria.Code);
				}
				else if (!String.IsNullOrEmpty(criteria.SeoKeyword))
				{
					var urlKeyword = _commerceService.GetSeoKeywordsByKeyword(criteria.SeoKeyword).Where(x => x.KeywordType == (int)SeoUrlKeywordTypes.Item).FirstOrDefault();
					if (urlKeyword == null)
					{
						query = query.Where(x => false);
					}
					else
					{
						query = query.Where(x => x.Id == urlKeyword.KeywordValue);
					}
				}

                result.TotalCount = query.Count();

                var itemIds = query.OrderByDescending(x => x.Name)
                                   .Skip(criteria.Start)
                                   .Take(criteria.Count)
                                   .Select(x => x.Id)
                                   .ToArray();

				var productResponseGroup = coreModel.ItemResponseGroup.ItemInfo | coreModel.ItemResponseGroup.ItemAssets;
				if ((criteria.ResponseGroup & coreModel.ResponseGroup.WithProperties) ==coreModel.ResponseGroup.WithProperties)
				{
					productResponseGroup |= coreModel.ItemResponseGroup.ItemProperties;
				}
				if ((criteria.ResponseGroup & coreModel.ResponseGroup.WithVariations) == coreModel.ResponseGroup.WithVariations)
				{
					productResponseGroup |= coreModel.ItemResponseGroup.Variations;
				}

				var products = _itemService.GetByIds(itemIds, productResponseGroup);
                result.Products = products.OrderByDescending(x => x.Name).ToList();
            }
        }
    }
}
