using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Catalogs
{

    public class CatalogOutlineBuilder : ICatalogOutlineBuilder
    {

        private CacheHelper _cacheHelper;
        private readonly ICatalogRepository _catalogRepository;
        private readonly ICacheRepository _cacheRepository;
        private const string CatalogCacheKey = "C:C:{0}";
        private const string CategoryIdCacheKey = "C:CTID:{0}:{1}";
        private const string CategoryItemRelationsCacheKey = "CR:CTID:IID:{0}:{1}";
        private const string LinkedCategoriesCacheKey = "CL:CTID:IID:{0}:{1}";
        private readonly TimeSpan _cacheTimeout = TimeSpan.FromMinutes(1);

        public CatalogOutlineBuilder(ICatalogRepository catalogRepository, ICacheRepository cacheRepository)
        {
            _catalogRepository = catalogRepository;
            _cacheRepository = cacheRepository;
        }

        private CacheHelper Helper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }

        public CatalogOutlines BuildCategoryOutline(string catalogId, string itemId, bool useCache)
        {
            var outlines = new CatalogOutlines();

            var catalog = GetCatalog(catalogId, useCache);

            if (catalog is Catalog)
            {
                var categoryRelations = GetCategoryItemRelations(itemId, catalogId);

                if (categoryRelations.Any())
                {
                    outlines.AddRange(
                        categoryRelations.Select(
                            categoryRelation => BuildCategoryOutline(catalogId, categoryRelation.Category, useCache)));
                }
            }
            else if (catalog is VirtualCatalog)
            {
                var linkedCategories = GetLinkedCategories(itemId, catalogId);

                if (linkedCategories.Any())
                {
                    outlines.AddRange(
                        linkedCategories.Select(cat => BuildCategoryOutline(cat.CatalogId, cat, useCache)));
                }
            }

            return outlines;
        }

		
        public CatalogOutline BuildCategoryOutline(string catalogId, CategoryBase category, bool useCache = true)
        {
            // recurring adding elements
            var categories = new List<CategoryBase>();
            var outline = new CatalogOutline { CatalogId = catalogId };
            if (category != null)
            {
                BuildCategoryOutline(ref categories, catalogId, category, useCache);
                outline.Categories.AddRange(categories);
            }

            return outline;
        }

				
        #region Private Methods

        private void BuildCategoryOutline(ref List<CategoryBase> categories, string catalogId, CategoryBase category, bool useCache = true)
        {
            while (true)
            {
                categories.Insert(0, category);

                if (String.IsNullOrEmpty(category.ParentCategoryId)) return;

                var parent = GetCategoryById(catalogId, category.ParentCategoryId, useCache);
                category = parent;
            }
        }

        private CatalogBase GetCatalog(string catalogId, bool useCache = true)
        {
            var query = _catalogRepository.Catalogs.Where(x => x.CatalogId.Equals(catalogId, StringComparison.OrdinalIgnoreCase));

            return Helper.Get(
                CacheHelper.CreateCacheKey(Constants.CatalogOutlineBuilderCachePrefix, string.Format(CatalogCacheKey, catalogId)),
                () => (query).SingleOrDefault(),
                _cacheTimeout,
                useCache);
        }

        private CategoryBase GetCategoryById(string catalogId, string categoryId, bool useCache = true)
        {
            return Helper.Get(
                CacheHelper.CreateCacheKey(Constants.CatalogOutlineBuilderCachePrefix, string.Format(CategoryIdCacheKey, catalogId, categoryId)),
                () => GetCategoryByIdInternal(categoryId),
                _cacheTimeout,
                useCache);
        }

        private CategoryItemRelation[] GetCategoryItemRelations(string itemId, string catalogId, bool useCache = true)
        {
            var query = _catalogRepository.CategoryItemRelations.Expand(x => x.Category)
                   .Where(c => c.ItemId == itemId && c.CatalogId == catalogId);

            return Helper.Get(
               CacheHelper.CreateCacheKey(Constants.CatalogOutlineBuilderCachePrefix, string.Format(CategoryItemRelationsCacheKey, catalogId, itemId)),
               () => (query).ToArray(),
               _cacheTimeout,
               useCache);
        }

        private LinkedCategory[] GetLinkedCategories(string itemId, string catalogId, bool useCache = true)
        {			
            var query = _catalogRepository.CategoryItemRelations
				.Where(ci => ci.ItemId == itemId)
				.SelectMany(c => c.Category.LinkedCategories)
				.Where(lc => lc.CatalogId == catalogId);
                
			
            return Helper.Get(
               CacheHelper.CreateCacheKey(Constants.CatalogOutlineBuilderCachePrefix, string.Format(LinkedCategoriesCacheKey, catalogId, itemId)),
               () => (query).ToArray(),
               _cacheTimeout,
               useCache);
        }

        private CategoryBase GetCategoryByIdInternal(string id)
		{
			return _catalogRepository.Categories.Where(x => x.CategoryId.Equals(id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
		}

        #endregion

        #region Depreciated methods
        [Obsolete]
        public static string BuildCategoryOutline(ICatalogRepository repository, string catalogId, Item item)
        {
            var retVal = new List<String>();

            // TODO: remove this call to improve performance, methods that call this should know what type of catalog it is
            var catalog = repository.Catalogs.Single(c => c.CatalogId == catalogId);

            if (catalog is Catalog)
            {
                // TODO: item should already have relations defined, make sure to use those instead of loading new
                var categoryRelations = repository.CategoryItemRelations.Expand(x => x.Category)
                    .Where(c => c.ItemId == item.ItemId && c.CatalogId == catalogId).ToArray();
                if (categoryRelations.Any())
                {
                    retVal.AddRange(
                        categoryRelations.Select(
                            categoryRelation => BuildCategoryOutline(repository, catalogId, categoryRelation.Category)));
                }
            }
            else if (catalog is VirtualCatalog)
            {
                // TODO: item should already have relations defined, make sure to use those instead of loading new
                var linkedCategories = repository.CategoryItemRelations
                    .Where(ci => ci.ItemId == item.ItemId)
                    .SelectMany(c => c.Category.LinkedCategories)
                    .Where(lc => lc.CatalogId == catalogId).ToArray();

                if (linkedCategories.Any())
                {
                    retVal.AddRange(
                        linkedCategories.Select(cat => BuildCategoryOutline(repository, cat.CatalogId, cat)));
                }
            }

            return String.Join(";", retVal.ToArray());
        }

        /// <summary>
        /// Builds the category outline for the specified category. Called by search and indexing.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        [Obsolete]
        public static string BuildCategoryOutline(ICatalogRepository repository, string catalogId, CategoryBase category)
        {
            if (String.IsNullOrEmpty(category.ParentCategoryId))
            {
                return String.Format("{0}/{1}", catalogId, category.CategoryId);
            }

            // TODO: add caching to this method, otherwise we going to be retrieving same category many times
            var parent = repository.Categories.FirstOrDefault(x => x.CategoryId == category.ParentCategoryId);
            return String.Format("{0}/{1}", BuildCategoryOutline(repository, catalogId, parent), category.CategoryId);
        }

        /// <summary>
        /// Gets the categories hierarchy, which means simply splitting the outline into category arrays. Used by promotion engine.
        /// No database requests performed.
        /// </summary>
        /// <param name="catalogOutline">The catalog outline.</param>
        /// <returns></returns>
        public static string[] GetCategoriesHierarchy(string catalogOutline)
        {
            string[] retVal = null;
            if (catalogOutline != null)
            {
                retVal = catalogOutline.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                retVal = retVal.SelectMany(x => x.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)).Distinct().ToArray();
            }
            return retVal ?? new string[] { };
        }
        #endregion
    }
}
