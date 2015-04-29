using System;
using System.Collections.ObjectModel;
using System.Linq;
using foundation = VirtoCommerce.CatalogModule.Data.Model;
using module = VirtoCommerce.Domain.Catalog.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

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
                                                    foundation.SeoUrlKeyword[] seoInfos = null, foundation.Category[] allParents = null)
        {
            if (catalog == null)
                throw new ArgumentNullException("catalog");

			var retVal = new module.Category();
			retVal.InjectFrom(dbCategoryBase);
			retVal.CatalogId = catalog.Id;
			retVal.Catalog = catalog;
			retVal.ParentId = dbCategoryBase.ParentCategoryId;

            var dbCategory = dbCategoryBase as foundation.Category;
            if (dbCategory != null)
            {
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
			var retVal = new foundation.Category();
			retVal.InjectFrom(category);
			retVal.ParentCategoryId = category.ParentId;
			retVal.EndDate = DateTime.UtcNow.AddYears(100);
			retVal.StartDate = DateTime.UtcNow;
          
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
				var patchInjectionPolicy = new PatchInjection<foundation.Category>(x => x.Code, x=>x.Name);
				target.InjectFrom(patchInjectionPolicy, source);

                if (!dbSource.CategoryPropertyValues.IsNullCollection())
                {
                    dbSource.CategoryPropertyValues.Patch(dbTarget.CategoryPropertyValues, (sourcePropValue, targetPropValue) => sourcePropValue.Patch(targetPropValue));
                }
            }
        }
    }
}
