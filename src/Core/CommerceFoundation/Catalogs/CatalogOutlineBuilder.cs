using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.Foundation.Catalogs
{
	public static class CatalogOutlineBuilder
	{
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

            // TODO: return array
			return String.Join(";", retVal.ToArray());
		}

        /// <summary>
        /// Builds the category outline for the specified category. Called by search and indexing.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
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
	}
}
