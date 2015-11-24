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
			// 2. Categories should be loaded by passing array of ids instead of parallel loading one by one
			using (var repository = _catalogRepositoryFactory())
			{
				var query = repository.Categories;

			    //Get list of search in categories
                var searchCategoryIds = criteria.CategoriesIds;
                if (searchCategoryIds != null && criteria.SearchInChildren)
                {
                    searchCategoryIds = searchCategoryIds.Concat(repository.GetAllChildrenCategoriesIds(searchCategoryIds)).ToArray();
                }

                //Filter by keyword in all categories
 				if (!String.IsNullOrEmpty(criteria.Keyword))
				{
					query = query.Where(x => x.Name.Contains(criteria.Keyword) || x.Code.Contains(criteria.Keyword));
				}
				else if (searchCategoryIds != null)
				{
                    if (criteria.HideDirectLinkedCategories)
                    {
                        query = query.Where(x => searchCategoryIds.Contains(x.ParentCategoryId) || x.OutgoingLinks.Any(y => searchCategoryIds.Contains(y.TargetCategory.ParentCategoryId)));
                    }
                    else
                    {
                        query = query.Where(x => searchCategoryIds.Contains(x.ParentCategoryId) || x.OutgoingLinks.Any(y => searchCategoryIds.Contains(y.TargetCategoryId)));
                    }
 				}
				else if (!String.IsNullOrEmpty(criteria.Code))
				{
					query = query.Where(x => x.Code == criteria.Code);
				}
                else if (criteria.CatalogsIds != null)
                {
                    query = query.Where(x => (criteria.CatalogsIds.Contains(x.CatalogId) && (x.ParentCategoryId == null || criteria.SearchInChildren)) || (x.OutgoingLinks.Any(y => y.TargetCategoryId == null && criteria.CatalogsIds.Contains(y.TargetCatalogId))));

                }

                var categoryIds = query.Select(x => x.Id).ToArray();

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

        private void SearchCatalogs(coreModel.SearchCriteria criteria, coreModel.SearchResult result)
        {
            using (var repository = _catalogRepositoryFactory())
            {
                var catalogIds = criteria.CatalogsIds;
                if (catalogIds == null)
                {
                    catalogIds = repository.Catalogs.Select(x => x.Id).ToArray();
                }
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
			         
                //list of search categories
                var searchCategoryIds = criteria.CategoriesIds;
                if(searchCategoryIds != null && criteria.SearchInChildren)
                {
                    searchCategoryIds = searchCategoryIds.Concat(repository.GetAllChildrenCategoriesIds(searchCategoryIds)).ToArray();
                }

				var query = repository.Items;
				if ((criteria.ResponseGroup & coreModel.ResponseGroup.WithVariations) != coreModel.ResponseGroup.WithVariations)
				{
					query = query.Where(x => x.ParentId == null);
				}

                if(searchCategoryIds != null)
                {
                   query = query.Where(x => searchCategoryIds.Contains(x.CategoryId) || x.CategoryLinks.Any(c => searchCategoryIds.Contains(c.CategoryId)));
                }
				else if (criteria.CatalogsIds != null)
				{
					query = query.Where(x => criteria.CatalogsIds.Contains(x.CatalogId) && (criteria.SearchInChildren || x.CategoryId == null));
				}

				if (!String.IsNullOrEmpty(criteria.Code))
				{
					query = query.Where(x => x.Code == criteria.Code);
				}
				else if (!String.IsNullOrEmpty(criteria.Keyword))
				{
					query = query.Where(x => x.Name.Contains(criteria.Keyword) || x.Code.Contains(criteria.Keyword));
				}

                result.TotalCount = query.Count();

                var itemIds = query.OrderByDescending(x => x.Name)
                                   .Skip(criteria.Start)
                                   .Take(criteria.Count)
                                   .Select(x => x.Id)
                                   .ToArray();

				var productResponseGroup = coreModel.ItemResponseGroup.ItemInfo | coreModel.ItemResponseGroup.ItemAssets | ItemResponseGroup.Links;
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
