using System;
using System.Collections.ObjectModel;
using System.Linq;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using VirtoCommerce.Domain.Commerce.Model;

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
        public static coreModel.Category ToCoreModel(this dataModel.CategoryBase dbCategoryBase, coreModel.Catalog catalog,
                                                    coreModel.Property[] properties = null, dataModel.LinkedCategory[] dbLinks = null,
                                                    SeoUrlKeyword[] seoInfos = null, dataModel.Category[] allParents = null)
        {
            if (catalog == null)
                throw new ArgumentNullException("catalog");

			var retVal = new coreModel.Category();
			retVal.InjectFrom(dbCategoryBase);
			retVal.CatalogId = catalog.Id;
			retVal.Catalog = catalog;
			retVal.ParentId = dbCategoryBase.ParentCategoryId;

            var dbCategory = dbCategoryBase as dataModel.Category;
            if (dbCategory != null)
            {
                retVal.PropertyValues = dbCategory.CategoryPropertyValues.Select(x => x.ToCoreModel(properties)).ToList();
                retVal.Virtual = catalog.Virtual;
            }

            if (allParents != null)
            {
                retVal.Parents = allParents.Select(x => x.ToCoreModel(catalog)).ToArray();
            }

            if (dbLinks != null)
            {
                retVal.Links = dbLinks.Select(x => x.ToCoreModel(retVal)).ToList();
            }

            if (seoInfos != null)
            {
                retVal.SeoInfos = seoInfos.Select(x => x.ToCoreModel()).ToList();
            }

            return retVal;

        }

        /// <summary>
        /// Converting to foundation type
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public static dataModel.CategoryBase ToDataModel(this coreModel.Category category)
        {
			var retVal = new dataModel.Category();

			var id = retVal.Id;
			retVal.InjectFrom(category);
			if(category.Id == null)
			{
				retVal.Id = id;
			}
			retVal.ParentCategoryId = category.ParentId;
			retVal.EndDate = DateTime.UtcNow.AddYears(100);
			retVal.StartDate = DateTime.UtcNow;
          
            retVal.CategoryPropertyValues = new NullCollection<dataModel.CategoryPropertyValue>();
            if (category.PropertyValues != null)
            {
                retVal.CategoryPropertyValues = new ObservableCollection<dataModel.CategoryPropertyValue>();
                retVal.CategoryPropertyValues.AddRange(category.PropertyValues.Select(x => x.ToDataModel<dataModel.CategoryPropertyValue>()).OfType<dataModel.CategoryPropertyValue>());
            }

            retVal.LinkedCategories = new NullCollection<dataModel.LinkedCategory>();
            if (category.Links != null)
            {
                retVal.LinkedCategories = new ObservableCollection<dataModel.LinkedCategory>();
                retVal.LinkedCategories.AddRange(category.Links.Select(x => x.ToDataModel(category)));
            }

            return retVal;
        }

        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this dataModel.CategoryBase source, dataModel.CategoryBase target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var dbSource = source as dataModel.Category;
            var dbTarget = target as dataModel.Category;

            if (dbSource != null && dbTarget != null)
            {
				var patchInjectionPolicy = new PatchInjection<dataModel.Category>(x => x.Code, x=>x.Name);
				target.InjectFrom(patchInjectionPolicy, source);

                if (!dbSource.CategoryPropertyValues.IsNullCollection())
                {
                    dbSource.CategoryPropertyValues.Patch(dbTarget.CategoryPropertyValues, (sourcePropValue, targetPropValue) => sourcePropValue.Patch(targetPropValue));
                }
            }
        }
    }
}
