using System;
using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using foundationConfig = VirtoCommerce.Foundation.AppConfig.Model;
using module = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
    public static class CategoryConverter
    {
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <param name="dbCategoryBase">The database category base.</param>
        /// <param name="catalog">The catalog.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">catalog</exception>
        public static module.Category ToModuleModel(this foundation.CategoryBase dbCategoryBase, module.Catalog catalog,
                                                    module.Property[] properties = null, foundation.LinkedCategory[] dbLinks = null,
                                                    foundationConfig.SeoUrlKeyword[] seoInfos = null, foundation.Category[] allParents = null)
        {
            if (catalog == null)
                throw new ArgumentNullException("catalog");

            var dbCategory = dbCategoryBase as foundation.Category;
            var retVal = new module.Category
            {
                CatalogId = catalog.Id,
                Code = dbCategoryBase.Code,
                Catalog = catalog,
                Id = dbCategoryBase.CategoryId,
                ParentId = dbCategoryBase.ParentCategoryId,
                Priority = dbCategoryBase.Priority,
                IsActive = dbCategoryBase.IsActive

            };

            if (dbCategory != null)
            {
                retVal.Name = dbCategory.Name;
                retVal.PropertyValues = dbCategory.CategoryPropertyValues.Select(x => x.ToModuleModel(properties)).ToList();

                retVal.Virtual = catalog.Virtual;
            }

            if (allParents != null)
            {
                retVal.Parents = allParents.Select(x => x.ToModuleModel(catalog)).ToArray();
            }

            if (dbLinks != null)
            {
                retVal.Links = dbLinks.Select(x => x.ToModuleModel(retVal)).ToList();
            }

            if (seoInfos != null)
            {
                retVal.SeoInfos = seoInfos.Select(x => x.ToModuleModel()).ToList();
            }

            return retVal;

        }

        /// <summary>
        /// Converting to foundation type
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public static foundation.CategoryBase ToFoundation(this module.Category category)
        {
            var retVal = new foundation.Category
            {
                CatalogId = category.CatalogId,
                Name = category.Name,
                Code = category.Code,
                ParentCategoryId = category.ParentId,
                EndDate = DateTime.UtcNow.AddYears(100),
                StartDate = DateTime.UtcNow,
                IsActive = category.IsActive
            };
            if (category.Id != null)
                retVal.CategoryId = category.Id;

            retVal.CategoryPropertyValues = new NullCollection<foundation.CategoryPropertyValue>();
            if (category.PropertyValues != null)
            {
                retVal.CategoryPropertyValues = new ObservableCollection<foundation.CategoryPropertyValue>();
                retVal.CategoryPropertyValues.AddRange(category.PropertyValues.Select(x => x.ToFoundation<foundation.CategoryPropertyValue>()).OfType<foundation.CategoryPropertyValue>());
            }

            retVal.LinkedCategories = new NullCollection<foundation.LinkedCategory>();
            if (category.Links != null)
            {
                retVal.LinkedCategories = new ObservableCollection<foundation.LinkedCategory>();
                retVal.LinkedCategories.AddRange(category.Links.Select(x => x.ToFoundation(category)));
            }

            return retVal;
        }

        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this foundation.CategoryBase source, foundation.CategoryBase target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var dbSource = source as foundation.Category;
            var dbTarget = target as foundation.Category;

            if (dbSource != null && dbTarget != null)
            {
                if (dbSource.Code != null)
                    dbTarget.Code = dbSource.Code;
                if (dbSource.Name != null)
                    dbTarget.Name = dbSource.Name;

                if (!dbSource.CategoryPropertyValues.IsNullCollection())
                {
                    dbSource.CategoryPropertyValues.Patch(dbTarget.CategoryPropertyValues, new PropertyValueComparer(),
                        (sourcePropValue, targetPropValue) => sourcePropValue.Patch(targetPropValue));
                }
            }
        }
    }
}
