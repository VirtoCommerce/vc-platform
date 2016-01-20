﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Model;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Data.Services
{
    public class CatalogSearchServiceImpl : ICatalogSearchService
    {
	    private readonly Func<ICatalogRepository> _catalogRepositoryFactory;
        private readonly IItemService _itemService;
        private readonly ICatalogService _catalogService;
        private readonly ICategoryService _categoryService;

        private Dictionary<string, string> _productSortingPropReplacementMap = new Dictionary<string, string>();
        private Dictionary<string, string> _categorySortingPropReplacementMap = new Dictionary<string, string>();

        public CatalogSearchServiceImpl(Func<ICatalogRepository> catalogRepositoryFactory, IItemService itemService, ICatalogService catalogService, ICategoryService categoryService)
        {
            _catalogRepositoryFactory = catalogRepositoryFactory;
            _itemService = itemService;
            _catalogService = catalogService;
            _categoryService = categoryService;

             _productSortingPropReplacementMap["sku"] = ReflectionUtility.GetPropertyName<CatalogProduct>(x => x.Code);
            _categorySortingPropReplacementMap["sku"] = ReflectionUtility.GetPropertyName<Category>(x => x.Code);

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
			using (var repository = _catalogRepositoryFactory())
			{
                var query = repository.Categories.Where(x => criteria.WithHidden ? true : x.IsActive);

                //Get list of search in categories
                var searchCategoryIds = criteria.CategoryIds;

                if (criteria.SearchInChildren)
                {
                    if (!searchCategoryIds.IsNullOrEmpty())
                    {
                        searchCategoryIds = searchCategoryIds.Concat(repository.GetAllChildrenCategoriesIds(searchCategoryIds)).ToArray();
                        //linked categories
                        var allLinkedCategories = repository.CategoryLinks.Where(x => searchCategoryIds.Contains(x.TargetCategoryId)).Select(x => x.SourceCategoryId).ToArray();
                        searchCategoryIds = searchCategoryIds.Concat(allLinkedCategories).Distinct().ToArray();
                    }
                    else if (!criteria.CatalogIds.IsNullOrEmpty())
                    {
                        //If categories not specified need search in all catalog linked and children categories 
                        searchCategoryIds = repository.Categories.Where(x => criteria.CatalogIds.Contains(x.CatalogId)).Select(x => x.Id).ToArray();
                        var allCatalogLinkedCategories = repository.CategoryLinks.Where(x => criteria.CatalogIds.Contains(x.TargetCatalogId)).Select(x => x.SourceCategoryId).ToArray();
                        searchCategoryIds = searchCategoryIds.Concat(allCatalogLinkedCategories).Distinct().ToArray();
                    }
                }

                if (!searchCategoryIds.IsNullOrEmpty())
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
                else if (!criteria.CatalogIds.IsNullOrEmpty())
                {
                    query = query.Where(x => (criteria.CatalogIds.Contains(x.CatalogId) && (x.ParentCategoryId == null || criteria.SearchInChildren)) || (x.OutgoingLinks.Any(y => y.TargetCategoryId == null && criteria.CatalogIds.Contains(y.TargetCatalogId))));

                }

                if (!string.IsNullOrEmpty(criteria.Keyword))
                {
                    query = query.Where(x => x.Name.Contains(criteria.Keyword) || x.Code.Contains(criteria.Keyword));
                }
                else if (!string.IsNullOrEmpty(criteria.Code))
                {
                    query = query.Where(x => x.Code == criteria.Code);
                }

                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = "Name" } };
                }
                //Try to replace sorting columns names
                TryTransformSortingInfoColumnNames(_categorySortingPropReplacementMap, sortInfos);

                query = query.OrderBySortInfos(sortInfos);

                var categoryIds = query.Select(x => x.Id).ToArray();
                var categoryResponseGroup = CategoryResponseGroup.Info | CategoryResponseGroup.WithImages | CategoryResponseGroup.WithSeo | CategoryResponseGroup.WithLinks | CategoryResponseGroup.WithParents;
                if ((criteria.ResponseGroup & SearchResponseGroup.WithProperties) == SearchResponseGroup.WithProperties)
                {
                    categoryResponseGroup |= CategoryResponseGroup.WithProperties;
                }

                result.Categories = _categoryService.GetByIds(categoryIds, categoryResponseGroup)
                                                    .AsQueryable()
                                                    .OrderBySortInfos(sortInfos)
                                                    .ToList();
            
			}
		}

        private void SearchCatalogs(SearchCriteria criteria, SearchResult result)
        {
            using (var repository = _catalogRepositoryFactory())
            {
                var catalogIds = criteria.CatalogIds;
                if (catalogIds.IsNullOrEmpty())
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
                if (criteria.SearchInChildren)
                {
                    if (!searchCategoryIds.IsNullOrEmpty())
                    {
                        searchCategoryIds = searchCategoryIds.Concat(repository.GetAllChildrenCategoriesIds(searchCategoryIds)).ToArray();
                        //linked categories
                        var allLinkedCategories = repository.CategoryLinks.Where(x => searchCategoryIds.Contains(x.TargetCategoryId)).Select(x => x.SourceCategoryId).ToArray();
                        searchCategoryIds = searchCategoryIds.Concat(allLinkedCategories).Distinct().ToArray();
                    }
                    else if (!criteria.CatalogIds.IsNullOrEmpty())
                    {
                        //If category not specified need search in all linked and children categories
                        searchCategoryIds = repository.Categories.Where(x => criteria.CatalogIds.Contains(x.CatalogId)).Select(x => x.Id).ToArray();
                        var allCatalogLinkedCategories = repository.CategoryLinks.Where(x => criteria.CatalogIds.Contains(x.TargetCatalogId)).Select(x => x.SourceCategoryId).ToArray();
                        searchCategoryIds = searchCategoryIds.Concat(allCatalogLinkedCategories).Distinct().ToArray();
                    }
                }

                //Do not search in variations
                var query = repository.Items.Where(x => x.ParentId == null).Where(x => criteria.WithHidden ? true : x.IsActive);

                if (!searchCategoryIds.IsNullOrEmpty())
                {
                   query = query.Where(x => searchCategoryIds.Contains(x.CategoryId) || x.CategoryLinks.Any(c => searchCategoryIds.Contains(c.CategoryId)));
                }
				else if (!criteria.CatalogIds.IsNullOrEmpty())
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

                //Filter by property dictionary values
                if (!criteria.PropertyValues.IsNullOrEmpty())
                {
                    var propValueIds = criteria.PropertyValues.Select(x => x.ValueId).Distinct().ToArray();
                    query = query.Where(x => x.ItemPropertyValues.Any(y => propValueIds.Contains(y.KeyValue)));
                }

                if(!criteria.ProductTypes.IsNullOrEmpty())
                {
                    query = query.Where(x => criteria.ProductTypes.Contains(x.ProductType));
                }

                if(criteria.OnlyBuyable != null)
                {
                    query = query.Where(x => x.IsBuyable == criteria.OnlyBuyable);
                }

                if (criteria.OnlyWithTrackingInventory != null)
                {
                    query = query.Where(x => x.TrackInventory == criteria.OnlyWithTrackingInventory);
                }

                result.ProductsTotalCount = query.Count();
              
                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = "Name" } };
                }
                //Try to replace sorting columns names
                TryTransformSortingInfoColumnNames(_productSortingPropReplacementMap, sortInfos);

                 query = query.OrderBySortInfos(sortInfos);

                var itemIds = query.Skip(criteria.Skip)
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
                result.Products = products.AsQueryable().OrderBySortInfos(sortInfos).ToList();
            }

        }

        private static void TryTransformSortingInfoColumnNames(IDictionary<string, string> transformationMap, SortInfo[] sortingInfos)
        {
            //Try to replace sorting columns names
            foreach (var sortInfo in sortingInfos)
            {
                string newColumnName;
                if (transformationMap.TryGetValue(sortInfo.SortColumn.ToLowerInvariant(), out newColumnName))
                {
                    sortInfo.SortColumn = newColumnName;
                }
            }
        }

    }
}
