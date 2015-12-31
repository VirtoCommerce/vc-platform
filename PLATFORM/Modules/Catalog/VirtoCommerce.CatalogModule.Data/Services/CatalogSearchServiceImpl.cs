using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;

namespace VirtoCommerce.CatalogModule.Data.Services
{
    public class CatalogSearchServiceImpl : ICatalogSearchService
    {
	    private readonly Func<ICatalogRepository> _catalogRepositoryFactory;
        private readonly IItemService _itemService;
        private readonly ICatalogService _catalogService;
        private readonly ICategoryService _categoryService;

        public CatalogSearchServiceImpl(Func<ICatalogRepository> catalogRepositoryFactory, IItemService itemService, ICatalogService catalogService, ICategoryService categoryService)
        {
            _catalogRepositoryFactory = catalogRepositoryFactory;
            _itemService = itemService;
            _catalogService = catalogService;
            _categoryService = categoryService;
		}

        public SearchResult Search(SearchCriteria criteria)
		{
            var retVal = new SearchResult();
			var taskList = new List<Task>();

            if ((criteria.ResponseGroup & SearchResponseGroup.WithProducts) == SearchResponseGroup.WithProducts)
			{
				taskList.Add(Task.Factory.StartNew(() => SearchItems(criteria, retVal)));
			}

            if ((criteria.ResponseGroup & SearchResponseGroup.WithCatalogs) == SearchResponseGroup.WithCatalogs)
			{
				taskList.Add(Task.Factory.StartNew(() => SearchCatalogs(criteria, retVal)));
			}

            if ((criteria.ResponseGroup & SearchResponseGroup.WithCategories) == SearchResponseGroup.WithCategories)
			{
				taskList.Add(Task.Factory.StartNew(() => SearchCategories(criteria, retVal)));
			}

			Task.WaitAll(taskList.ToArray());

			return retVal;
		}

        private void SearchCategories(SearchCriteria criteria, SearchResult result)
		{
			// TODO: optimize for performance, need to eliminate number of database queries
			// 1. Catalog should either be passed or loaded using caching
			// 2. Categories should be loaded by passing array of ids instead of parallel loading one by one
			using (var repository = _catalogRepositoryFactory())
			{
                var query = repository.Categories.Where(x => criteria.WithHidden ? true : x.IsActive);

                //Get list of search in categories
                var searchCategoryIds = criteria.CategoryIds;
                if (searchCategoryIds != null && searchCategoryIds.Any() && criteria.SearchInChildren)
                {
                    searchCategoryIds = searchCategoryIds.Concat(repository.GetAllChildrenCategoriesIds(searchCategoryIds)).ToArray();
                }

                //Filter by keyword in all categories
                if (!string.IsNullOrEmpty(criteria.Keyword))
				{
					query = query.Where(x => x.Name.Contains(criteria.Keyword) || x.Code.Contains(criteria.Keyword));
				}
				else if (searchCategoryIds != null && searchCategoryIds.Any())
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
                else if (!string.IsNullOrEmpty(criteria.Code))
				{
					query = query.Where(x => x.Code == criteria.Code);
				}
                else if (criteria.CatalogIds != null)
                {
                    query = query.Where(x => (criteria.CatalogIds.Contains(x.CatalogId) && (x.ParentCategoryId == null || criteria.SearchInChildren)) || (x.OutgoingLinks.Any(y => y.TargetCategoryId == null && criteria.CatalogIds.Contains(y.TargetCatalogId))));

                }

                var categoryIds = query.Select(x => x.Id).ToArray();
                var categoryResponseGroup = CategoryResponseGroup.Info | CategoryResponseGroup.WithImages | CategoryResponseGroup.WithSeo | CategoryResponseGroup.WithLinks | CategoryResponseGroup.WithParents;
                if ((criteria.ResponseGroup & SearchResponseGroup.WithProperties) == SearchResponseGroup.WithProperties)
                {
                    categoryResponseGroup |= CategoryResponseGroup.WithProperties;
                }

                result.Categories = _categoryService.GetByIds(categoryIds, categoryResponseGroup)
                                                    .OrderByDescending(x => x.Priority)
                                                    .ThenBy(x => x.Name)
                                                    .ToList();
            
			}
		}

        private void SearchCatalogs(SearchCriteria criteria, SearchResult result)
        {
            using (var repository = _catalogRepositoryFactory())
            {
                var catalogIds = criteria.CatalogIds;
                if (catalogIds == null)
                {
                    catalogIds = repository.Catalogs.Select(x => x.Id).ToArray();
                }

                var catalogs = new ConcurrentBag<Catalog>();
                var parallelOptions = new ParallelOptions
                {
                    MaxDegreeOfParallelism = 10
                };

                Parallel.ForEach(catalogIds, parallelOptions, catalogId =>
                {
                    var catalog = _catalogService.GetById(catalogId);
                    catalogs.Add(catalog);
                });

                result.Catalogs = catalogs.OrderByDescending(x => x.Name).ToList();
            }
        }

        private void SearchItems(SearchCriteria criteria, SearchResult result)
        {
            using (var repository = _catalogRepositoryFactory())
            {
                //list of search categories
                var searchCategoryIds = criteria.CategoryIds;
                if (searchCategoryIds != null && criteria.SearchInChildren)
                {
                    searchCategoryIds = searchCategoryIds.Concat(repository.GetAllChildrenCategoriesIds(searchCategoryIds)).ToArray();
                    //linked categories
                    var allLinkedCategories = repository.CategoryLinks.Where(x => searchCategoryIds.Contains(x.TargetCategoryId)).Select(x => x.SourceCategoryId).ToArray();
                    searchCategoryIds = searchCategoryIds.Concat(allLinkedCategories).Distinct().ToArray();
                }

                //Do not search in variations
                var query = repository.Items.Where(x => x.ParentId == null && criteria.WithHidden ? true : x.IsActive);

                if (searchCategoryIds != null)
                {
                   query = query.Where(x => searchCategoryIds.Contains(x.CategoryId) || x.CategoryLinks.Any(c => searchCategoryIds.Contains(c.CategoryId)));
                }
				else if (criteria.CatalogIds != null)
				{
					query = query.Where(x => criteria.CatalogIds.Contains(x.CatalogId) && (criteria.SearchInChildren || x.CategoryId == null));
				}

                if (!string.IsNullOrEmpty(criteria.Code))
				{
					query = query.Where(x => x.Code == criteria.Code);
				}
                else if (!string.IsNullOrEmpty(criteria.Keyword))
				{
					query = query.Where(x => x.Name.Contains(criteria.Keyword) || x.Code.Contains(criteria.Keyword));
				}

                //Filter by property  dictionary values
                if (criteria.PropertyValues != null && criteria.PropertyValues.Any())
                {
                    var propValueIds = criteria.PropertyValues.Select(x => x.ValueId).Distinct().ToArray();
                    query = query.Where(x => x.ItemPropertyValues.Any(y => propValueIds.Contains(y.KeyValue)));
                }

                result.ProductsTotalCount = query.Count();

                var itemIds = query.OrderByDescending(x => x.Name)
                                   .Skip(criteria.Skip)
                                   .Take(criteria.Take)
                                   .Select(x => x.Id)
                                   .ToArray();

                var productResponseGroup = ItemResponseGroup.ItemInfo | ItemResponseGroup.ItemAssets | ItemResponseGroup.Links | ItemResponseGroup.Seo;

                if ((criteria.ResponseGroup & SearchResponseGroup.WithProperties) == SearchResponseGroup.WithProperties)
				{
                    productResponseGroup |= ItemResponseGroup.ItemProperties;
				}

                if ((criteria.ResponseGroup & SearchResponseGroup.WithVariations) == SearchResponseGroup.WithVariations)
				{
                    productResponseGroup |= ItemResponseGroup.Variations;
				}

				var products = _itemService.GetByIds(itemIds, productResponseGroup);
                result.Products = products.OrderByDescending(x => x.Name).ToList();
            }
        }
    }
}
