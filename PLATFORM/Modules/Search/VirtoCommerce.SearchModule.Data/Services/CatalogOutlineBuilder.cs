using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.SearchModule.Data.Services
{
    public sealed class CatalogOutlineBuilder
    {
        private readonly ICategoryService _categoryService;
        private readonly CacheManager _cacheManager;

        public CatalogOutlineBuilder(ICategoryService categoryService, CacheManager cacheManager)
        {
            _categoryService = categoryService;
            _cacheManager = cacheManager;
        }

        private Category GetCategoryById(string categoryId)
        {
            var cacheKey = CacheKey.Create("CatalogOutlineBuilder.GetCategoryById", categoryId);
            var retVal = _cacheManager.Get(cacheKey, () => _categoryService.GetById(categoryId, CategoryResponseGroup.Full));
            return retVal;
        }

        /// <summary>
        /// Returns a collection of all possible paths to the root (catalog): catalog/parents/category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public string[] GetOutlines(string categoryId)
        {
            var result = new List<string>();

            var outlines = new List<List<string>>();
            AddOutlinesForParentAndLinkedCategories(categoryId, new List<string>(), outlines);
            result.AddRange(outlines.SelectMany(ExpandOutline));

            return result.Distinct().ToArray();
        }


        private void AddOutlinesForParentAndLinkedCategories(string categoryId, List<string> outline, List<List<string>> outlines)
        {
            var newOutline = new List<string>(outline);
            newOutline.Insert(0, categoryId);

            var category = GetCategoryById(categoryId);

            if (string.IsNullOrEmpty(category.ParentId))
            {
                var finalOutline = new List<string>(newOutline);
                finalOutline.Insert(0, category.CatalogId);
                outlines.Add(finalOutline);
            }
            else
            {
                AddOutlinesForParentAndLinkedCategories(category.ParentId, newOutline, outlines);
            }

            foreach (var link in category.Links.Where(link => !string.IsNullOrEmpty(link.CategoryId)))
            {
                // Don't include the linked category in the outline, so pass the original outline
                AddOutlinesForParentAndLinkedCategories(link.CategoryId, outline, outlines);
            }
        }

        private static IEnumerable<string> ExpandOutline(List<string> outline)
        {
            var result = new List<string> { outline.First(), string.Join("/", outline) };

            // For each child category create a separate outline: catalog/child_category
            if (outline.Count > 2)
            {
                var catalogId = outline.FirstOrDefault();
                result.AddRange(
                    outline.Skip(1)
                    .Select(categoryId =>
                        string.Join("/", catalogId, categoryId)));
            }

            return result;
        }
    }
}
