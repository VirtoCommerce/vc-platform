﻿using System;
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
        /// <param name="dbCategory">The database category base.</param>
        /// <param name="catalog">The catalog.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">catalog</exception>
        public static coreModel.Category ToCoreModel(this dataModel.Category dbCategory, coreModel.Catalog catalog,
                                                     dataModel.Category[] allParents = null)
        {
            if (catalog == null)
                throw new ArgumentNullException("catalog");

			var retVal = new coreModel.Category();
			retVal.InjectFrom(dbCategory);
			retVal.CatalogId = catalog.Id;
			retVal.Catalog = catalog;
			retVal.ParentId = dbCategory.ParentCategoryId;
			retVal.IsActive = dbCategory.IsActive;


			retVal.PropertyValues = dbCategory.CategoryPropertyValues.Select(x => x.ToCoreModel(dbCategory.Properties)).ToList();
			retVal.Virtual = catalog.Virtual;
			retVal.Links = dbCategory.OutgoingLinks.Select(x => x.ToCoreModel(retVal)).ToList();


            if (allParents != null)
            {
                retVal.Parents = allParents.Select(x => x.ToCoreModel(catalog)).ToArray();
            }

			//Try to inherit taxType from parent category
			if(retVal.TaxType == null && retVal.Parents != null)
			{
				retVal.TaxType = retVal.Parents.Select(x => x.TaxType).Where(x => x != null).FirstOrDefault();
			}

			#region Images
			if (dbCategory.Images != null)
			{
				retVal.Images = dbCategory.Images.OrderBy(x => x.SortOrder).Select(x => x.ToCoreModel()).ToList();
			}
			#endregion

            return retVal;

        }

        /// <summary>
        /// Converting to foundation type
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public static dataModel.Category ToDataModel(this coreModel.Category category)
        {
			var retVal = new dataModel.Category();

			retVal.InjectFrom(category);
	
			retVal.ParentCategoryId = category.ParentId;
			retVal.EndDate = DateTime.UtcNow.AddYears(100);
			retVal.StartDate = DateTime.UtcNow;
			retVal.IsActive = category.IsActive ?? true;
          
            if (category.PropertyValues != null)
            {
                retVal.CategoryPropertyValues = new ObservableCollection<dataModel.PropertyValue>();
                retVal.CategoryPropertyValues.AddRange(category.PropertyValues.Select(x => x.ToDataModel()));
            }

            if (category.Links != null)
            {
				retVal.OutgoingLinks = new ObservableCollection<dataModel.CategoryRelation>();
				retVal.OutgoingLinks.AddRange(category.Links.Select(x => x.ToDataModel(category)));
            }

			#region Images
			if (category.Images != null)
			{
				retVal.Images = new ObservableCollection<dataModel.Image>(category.Images.Select(x=>x.ToDataModel()));
			}
			#endregion

            return retVal;
        }

        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
		public static void Patch(this coreModel.Category source, dataModel.Category target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

			//TODO: temporary solution because partial update replaced not nullable properties in db entity
			if (source.IsActive != null)
				target.IsActive = source.IsActive.Value;
            //Handle three valuable states (null, empty and have value states) for case when need reset catalog or category
            if (source.CatalogId == String.Empty)
                target.CatalogId = null;
            if (source.ParentId == String.Empty)
                target.ParentCategoryId = null;


            var dbSource = source.ToDataModel() as dataModel.Category;
			var dbTarget = target as dataModel.Category;

            if (dbSource != null && dbTarget != null)
            {
                var patchInjectionPolicy = new PatchInjection<dataModel.Category>(x => x.Code, x => x.Name, x => x.TaxType, x => x.CatalogId, x => x.ParentCategoryId);
                dbTarget.InjectFrom(patchInjectionPolicy, dbSource);

                if (!dbSource.CategoryPropertyValues.IsNullCollection())
                {
                    dbSource.CategoryPropertyValues.Patch(dbTarget.CategoryPropertyValues, (sourcePropValue, targetPropValue) => sourcePropValue.Patch(targetPropValue));
                }

				if(!dbSource.OutgoingLinks.IsNullCollection())
				{
					dbSource.OutgoingLinks.Patch(dbTarget.OutgoingLinks, new LinkedCategoryComparer(), (sourceLink, targetLink) => sourceLink.Patch(targetLink));
				}

				if (!dbSource.Images.IsNullCollection())
				{
					dbSource.Images.Patch(dbTarget.Images, (sourceImage, targetImage) => sourceImage.Patch(targetImage));
				}
            }
        }
    }
}
