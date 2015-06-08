using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.SearchModule.Data.Services
{
	public sealed class CatalogOutlineBuilder
	{
		private const string _separator = "/";
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
			var retVal = _cacheManager.Get(cacheKey, () => _categoryService.GetById(categoryId));
			return retVal;
		}

		public string[] GetOutlines(string categoryId)
		{
			var retVal = new List<string>();
			var stringBuilder = new StringBuilder();
			var category = GetCategoryById(categoryId);

			//first direct outline
			var outline = new List<string>();
			//catalog/parents/category
			outline.Add(category.CatalogId);
			outline.AddRange(category.Parents.Select(x => x.Id));
			outline.Add(category.Id);
			retVal.Add(String.Join(_separator, outline));

			//Next direct links (need remove directory id from outline for displaying products in mapped virtual category)
			foreach (var link in category.Links)
			{
				//VirtualCatalog/virtual categories/foreign virtual category
				outline = new List<string>();
				outline.Add(link.CatalogId);
				if (link.CategoryId != null)
				{
					link.Category = GetCategoryById(link.CategoryId);
					outline.AddRange(link.Category.Parents.Select(x => x.Id));
					outline.Add(link.CategoryId);
				}
				retVal.Add(String.Join(_separator, outline));
			}

			//Parent category links 
			foreach (var parent in category.Parents)
			{
				//Virtual catalog/virtual categories/foreign virtual category/parent category
				var parentCategory = GetCategoryById(parent.Id);
				foreach (var link in parentCategory.Links)
				{
					outline = new List<string>();
					outline.Add(link.CatalogId);
					if (link.CategoryId != null)
					{
						link.Category = GetCategoryById(link.CategoryId);
						outline.AddRange(link.Category.Parents.Select(x => x.Id));
						outline.Add(link.CategoryId);
					}
					outline.Add(parent.Id);
					retVal.Add(String.Join(_separator, outline));
				}
			}
			return retVal.Distinct().ToArray();

		}
	}
}
