using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Data.Common;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
	public static class ItemConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <returns></returns>
		public static coreModel.CatalogProduct ToCoreModel(this dataModel.Item dbItem)
		{
			var retVal = new coreModel.CatalogProduct();
			retVal.InjectFrom(dbItem);
			retVal.Catalog = dbItem.Catalog.ToCoreModel();

			if (dbItem.Category != null)
			{
                retVal.Category = dbItem.Category.ToCoreModel();
			}

			retVal.MainProductId = dbItem.ParentId;

			retVal.IsActive = dbItem.IsActive;
			retVal.IsBuyable = dbItem.IsBuyable;
			retVal.TrackInventory = dbItem.TrackInventory;

			retVal.MaxQuantity = (int)dbItem.MaxQuantity;
			retVal.MinQuantity = (int)dbItem.MinQuantity;


			//Links
			retVal.Links = dbItem.CategoryLinks.Select(x => x.ToCoreModel()).ToList();

			//Images
			if (dbItem.Images != null)
			{
				retVal.Images = dbItem.Images.OrderBy(x => x.SortOrder).Select(x => x.ToCoreModel()).ToList();
			}

			//Assets
			if (dbItem.Assets != null)
			{
				retVal.Assets = dbItem.Assets.OrderBy(x=>x.CreatedDate).Select(x => x.ToCoreModel()).ToList();
			}

			// Property values
			if (dbItem.ItemPropertyValues != null)
			{
				retVal.PropertyValues = dbItem.ItemPropertyValues.OrderBy(x=>x.Name).Select(x => x.ToCoreModel()).ToList();
			}

			// Variations
			retVal.Variations = new List<coreModel.CatalogProduct>();
			foreach (var variation in dbItem.Childrens)
			{
				var productVaraition = variation.ToCoreModel();
				productVaraition.MainProduct = retVal;
				productVaraition.MainProductId = retVal.Id;
				
				retVal.Variations.Add(productVaraition);
			}

			// EditorialReviews
			retVal.Reviews = dbItem.EditorialReviews.Select(x => x.ToCoreModel()).ToList();

            // Associations
            retVal.Associations = dbItem.AssociationGroups.SelectMany(x => x.Associations).Select(x => x.ToCoreModel()).ToList();

            //TaxType category inheritance
            if (retVal.TaxType == null && retVal.Category != null)
			{
				retVal.TaxType = retVal.Category.TaxType;
			}

            retVal.Properties = new List<coreModel.Property>();
        
            //Properties
            if (retVal.Category != null)
            {
                //Add inherited from category and catalog properties
                retVal.Properties.AddRange(retVal.Category.Properties);
            }
            else
            {
                retVal.Properties.AddRange(retVal.Catalog.Properties);
            }

            //Next need set Property in PropertyValues objects
            foreach (var propValue in retVal.PropertyValues)
            {
                propValue.Property = retVal.Properties.FirstOrDefault(x => x.IsSuitableForValue(propValue));
            }

            #region Variation property, assets, review inheritance
            if (dbItem.Parent != null)
			{
				//TaxType from main product inheritance
				if(dbItem.TaxType == null && dbItem.Parent.TaxType != null)
				{
					retVal.TaxType = dbItem.Parent.TaxType;
				}
				var allProductPropertyNames = dbItem.Parent.ItemPropertyValues.Select(x => x.Name).Distinct().ToArray();
				//Property inheritance
				if (allProductPropertyNames != null)
				{
					//Need copy not overridden property values from main product to variation
					var overriddenPropertyNames = retVal.PropertyValues.Select(x => x.PropertyName).ToArray();
					var inheritedPropertyNames = allProductPropertyNames.Except(overriddenPropertyNames);
					var dbInheritedPropertyValues = dbItem.Parent.ItemPropertyValues.Where(x => inheritedPropertyNames.Contains(x.Name));
					foreach (var dbInheritedPropertyValue in dbInheritedPropertyValues)
					{
						//Reset id for correct value override
						var propertyValue = dbInheritedPropertyValue.ToCoreModel();
						propertyValue.Id = null;
						retVal.PropertyValues.Add(propertyValue);
					}
				}
				//Image inheritance
				if (!retVal.Images.Any() && dbItem.Parent.Images != null)
				{
					retVal.Images = dbItem.Parent.Images.OrderBy(x => x.SortOrder).Select(x => x.ToCoreModel()).ToList();
					foreach (var image in retVal.Images)
					{
						//Reset id for correct override
						image.Id = null;
					}
				}
				//Review inheritance
				if ((retVal.Reviews == null  || !retVal.Reviews.Any()) && dbItem.Parent.EditorialReviews != null)
				{
					retVal.Reviews = dbItem.Parent.EditorialReviews.Select(x => x.ToCoreModel()).ToList();
					foreach (var review in retVal.Reviews)
					{
						//Reset id for correct override
						review.Id = null;
					}
				}

			}
			#endregion

			return retVal;
		}

		/// <summary>
		/// Converting to foundation type
		/// </summary>
		/// <param name="catalog"></param>
		/// <returns></returns>
		public static dataModel.Item ToDataModel(this coreModel.CatalogProduct product)
		{
			var retVal = new dataModel.Item();
			var id = retVal.Id;
			retVal.InjectFrom(product);
		
			if(product.StartDate == default(DateTime))
			{
				retVal.StartDate = DateTime.UtcNow;
			}

			retVal.IsActive = product.IsActive ?? true;
			retVal.IsBuyable = product.IsBuyable ?? true;
			retVal.TrackInventory = product.TrackInventory ?? true;
			retVal.MaxQuantity = product.MaxQuantity ?? 0;
			retVal.MinQuantity = product.MinQuantity ?? 0;

			retVal.ParentId = product.MainProductId;
			//Constant fields
			//Only for main product
			retVal.AvailabilityRule = (int)coreModel.AvailabilityRule.Always;
			retVal.MinQuantity = 1;
			retVal.MaxQuantity = 0;

			retVal.CatalogId = product.CatalogId;
			retVal.CategoryId = String.IsNullOrEmpty(product.CategoryId) ? null : product.CategoryId;

			#region ItemPropertyValues
			if (product.PropertyValues != null)
			{
				retVal.ItemPropertyValues = new ObservableCollection<dataModel.PropertyValue>();
                retVal.ItemPropertyValues.AddRange(product.PropertyValues.Select(x => x.ToDataModel()));
			
			}
			#endregion

			#region Assets
			if (product.Assets != null)
			{
				retVal.Assets = new ObservableCollection<dataModel.Asset>(product.Assets.Select(x => x.ToDataModel()));
			}
			#endregion

			#region Images
			if (product.Images != null)
			{
				retVal.Images = new ObservableCollection<dataModel.Image>(product.Images.Select(x => x.ToDataModel()));
			}
			#endregion

			#region Links
			if (product.Links != null)
			{
				retVal.CategoryLinks = new ObservableCollection<dataModel.CategoryItemRelation>();
				retVal.CategoryLinks.AddRange(product.Links.Select(x => x.ToDataModel(product)));
			}
			#endregion

			#region EditorialReview
			if (product.Reviews != null)
			{
				retVal.EditorialReviews = new ObservableCollection<dataModel.EditorialReview>();
				retVal.EditorialReviews.AddRange(product.Reviews.Select(x => x.ToDataModel(retVal)));
			}
			#endregion

			#region Associations
			if (product.Associations != null)
			{
				retVal.AssociationGroups = new ObservableCollection<dataModel.AssociationGroup>();
				var associations = product.Associations.ToArray();
				for (int order = 0; order < associations.Count(); order++)
				{
					var association = associations[order];
					var associationGroup = retVal.AssociationGroups.FirstOrDefault(x => x.Name == association.Name);
					if (associationGroup == null)
					{
						associationGroup = new dataModel.AssociationGroup
						{
							Name = association.Name,
							Description = association.Description,
							Priority = 1,
						};
						retVal.AssociationGroups.Add(associationGroup);
					}
					var foundationAssociation = association.ToDataModel();
					foundationAssociation.Priority = order;
					associationGroup.Associations.Add(foundationAssociation);
				}
			}
			#endregion

			return retVal;
		}


		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this coreModel.CatalogProduct source, dataModel.Item target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			//TODO: temporary solution because partial update replaced not nullable properties in db entity
			if (source.IsBuyable != null)
				target.IsBuyable = source.IsBuyable.Value;
			if (source.IsActive != null)
				target.IsActive = source.IsActive.Value;
			if (source.TrackInventory != null)
				target.TrackInventory = source.TrackInventory.Value;
			if (source.MinQuantity != null)
				target.MinQuantity = source.MinQuantity.Value;
			if (source.MaxQuantity != null)
				target.MaxQuantity = source.MaxQuantity.Value;
            //Handle three valuable states (null, empty and have value states) for case when need reset catalog or category
            if (source.CatalogId == String.Empty)
                 target.CatalogId = null;
            if (source.CategoryId == String.Empty)
                target.CategoryId = null;

            var patchInjectionPolicy = new PatchInjection<dataModel.Item>(x => x.Name, x => x.Code, x => x.ManufacturerPartNumber, x => x.Gtin, x => x.ProductType,
                                                                          x => x.WeightUnit, x => x.Weight, x => x.MeasureUnit, x => x.Height, x => x.Length, x => x.Width, x => x.EnableReview, x => x.MaxNumberOfDownload,
                                                                          x => x.DownloadExpiration, x => x.DownloadType, x => x.HasUserAgreement, x => x.ShippingType, x => x.TaxType, x => x.Vendor, x => x.CatalogId, x => x.CategoryId);

            var dbSource = source.ToDataModel();
			target.InjectFrom(patchInjectionPolicy, dbSource);

			#region Assets
			if (!dbSource.Assets.IsNullCollection())
			{
				dbSource.Assets.Patch(target.Assets, (sourceAsset, targetAsset) => sourceAsset.Patch(targetAsset));
			}
			#endregion
			#region Images
			if (!dbSource.Images.IsNullCollection())
			{
				dbSource.Images.Patch(target.Images, (sourceImage, targetImage) => sourceImage.Patch(targetImage));
			}
			#endregion

			#region ItemPropertyValues
			if (!dbSource.ItemPropertyValues.IsNullCollection())
			{
				//Need skip inherited properties without overridden value
				if (source.MainProduct != null)
				{
					var dbParent = source.MainProduct.ToDataModel();
					var parentPropValues = dbParent.ItemPropertyValues.ToLookup(x => x.Name + "-" + x.ToString());
					var variationPropValues = dbSource.ItemPropertyValues.ToLookup(x => x.Name + "-" + x.ToString());
					dbSource.ItemPropertyValues = new ObservableCollection<dataModel.PropertyValue>(variationPropValues.Where(x => !parentPropValues.Contains(x.Key)).SelectMany(x => x));
				}

				dbSource.ItemPropertyValues.Patch(target.ItemPropertyValues, (sourcePropValue, targetPropValue) => sourcePropValue.Patch(targetPropValue));
			}

			#endregion

			#region Links
			if (!dbSource.CategoryLinks.IsNullCollection())
			{
				dbSource.CategoryLinks.Patch(target.CategoryLinks, new CategoryItemRelationComparer(),
										 (sourcePropValue, targetPropValue) => sourcePropValue.Patch(targetPropValue));
			}
			#endregion


			#region EditorialReviews
			if (!dbSource.EditorialReviews.IsNullCollection())
			{
				dbSource.EditorialReviews.Patch(target.EditorialReviews, (sourcePropValue, targetPropValue) => sourcePropValue.Patch(targetPropValue));
			}
			#endregion

			#region Association
			if (!dbSource.AssociationGroups.IsNullCollection())
			{
				var associationComparer = AnonymousComparer.Create((dataModel.AssociationGroup x) => x.Name);
				dbSource.AssociationGroups.Patch(target.AssociationGroups, associationComparer,
										 (sourceGroup, targetGroup) => sourceGroup.Patch(targetGroup));
			}
			#endregion
		}

	}
}
