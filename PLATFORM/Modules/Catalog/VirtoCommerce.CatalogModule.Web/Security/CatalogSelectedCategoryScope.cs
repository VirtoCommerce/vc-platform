using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.CatalogModule.Web.Security
{
    public class CatalogSelectedCategoryScope : PermissionScope
    {
        private readonly ICategoryService _categoryService;
        public CatalogSelectedCategoryScope(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public override bool IsScopeAvailableForPermission(string permission)
        {
            return permission == CatalogPredefinedPermissions.Read
                      || permission == CatalogPredefinedPermissions.Update;
        }

        public override IEnumerable<string> GetEntityScopeStrings(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }


            var category = obj as Category;
            var product = obj as CatalogProduct;
            var link = obj as Model.ListEntryLink;
            var property = obj as Model.Property;

            string categoryId = null;

            if (category != null)
                categoryId = category.Id;
            if (product != null)
                categoryId = product.CategoryId;
            if (link != null)
                categoryId = link.CategoryId;
            if (property != null)
                categoryId = property.CategoryId;

            if (categoryId != null)
            {
                var resultCategory = _categoryService.GetById(categoryId, CategoryResponseGroup.WithParents);
                //Need to return scopes for all parent categories to support scope inheritance (permission defined on parent category should be active for children)
                var retVal = new[] { resultCategory.Id }.Concat(resultCategory.Parents.Select(x => x.Id)).Select(x => Type + ":" + x);
                return retVal;
            }

            return Enumerable.Empty<string>();
        }
    }
}