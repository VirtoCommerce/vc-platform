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
		public static coreModel.CatalogProduct ToCoreModel(this dataModel.Item dbItem, coreModel.Catalog catalog,
														  coreModel.Category category, coreModel.Property[] properties,
														  dataModel.Item[] variations,
														  SeoUrlKeyword[] seoInfos,
														  string mainProductId,
														  coreModel.CatalogProduct[] associatedProducts)
		{
			var retVal = new coreModel.CatalogProduct();
			retVal.InjectFrom(dbItem);
			retVal.Catalog = catalog;
			retVal.CatalogId = catalog.Id;
		
			if (category != null)
			{
				retVal.Category = category;
				retVal.CategoryId = category.Id;
			}

			retVal.MainProductId = mainProductId;

			#region Links
			retVal.Links = dbItem.CategoryItemRelations.Select(x => x.ToCoreModel()).ToList();
			#endregion

			#region Variations
			if (variations != null)
			{
				retVal.Variations = new List<coreModel.CatalogProduct>();
				foreach (var variation in variations)
				{
					var productVaraition = variation.ToCoreModel(catalog, category, properties,
																   variations: null,
																   seoInfos: null,
																   mainProductId: retVal.Id, associatedProducts: null);
					productVaraition.MainProduct = retVal;
					productVaraition.MainProductId = retVal.Id;
					retVal.Variations.Add(productVaraition);
				}
			}
			#endregion

			#region Assets
			if (dbItem.ItemAssets != null)
			{
				retVal.Assets = dbItem.ItemAssets.OrderBy(x => x.SortOrder).Select(x => x.ToCoreModel()).ToList();
			}
			#endregion

			#region Property values
			if (dbItem.ItemPropertyValues != null)
			{
				retVal.PropertyValues = dbItem.ItemPropertyValues.Select(x => x.ToCoreModel(properties)).ToList();
			}
			#endregion

			#region SeoInfo
			if (seoInfos != null)
			{
				retVal.SeoInfos = seoInfos.Select(x => x.ToCoreModel()).ToList();
			}
			#endregion

			#region EditorialReviews
			if (dbItem.EditorialReviews != null)
			{
				retVal.Reviews = dbItem.EditorialReviews.Select(x => x.ToCoreModel()).ToList();
			}
			#endregion

			#region Associations
			if (dbItem.AssociationGroups != null && associatedProducts != null)
			{
				retVal.Associations = new List<coreModel.ProductAssociation>();
				foreach (var association in dbItem.AssociationGroups.SelectMany(x => x.Associations))
				{
					var associatedProduct = associatedProducts.FirstOrDefault(x => x.Id == association.ItemId);
					if (associatedProduct != null)
					{
						var productAssociation = association.ToCoreModel(associatedProduct);
						retVal.Associations.Add(productAssociation);
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
			var retVal = new dataModel.Product();
			var id = retVal.Id;
			retVal.InjectFrom(product);
			if (product.Id == null)
			{
				retVal.Id = id;
			}

			if(product.StartDate == default(DateTime))
			{
				retVal.StartDate = DateTime.UtcNow;
			}
			//Constant fields
			//Only for main product
			retVal.AvailabilityRule = (int)coreModel.AvailabilityRule.Always;
			retVal.MinQuantity = 1;
			retVal.MaxQuantity = 0;
			//If it variation need make active false (workaround)
			retVal.IsActive = product.MainProductId == null;
  
			//Changed fields
			retVal.CatalogId = product.CatalogId;

			#region ItemPropertyValues
			if (product.PropertyValues != null)
			{
				retVal.ItemPropertyValues = new ObservableCollection<dataModel.ItemPropertyValue>();
				foreach (var propValue in product.PropertyValues)
				{
					var dbPropValue = propValue.ToDataModel<dataModel.ItemPropertyValue>() as dataModel.ItemPropertyValue;
					retVal.ItemPropertyValues.Add(dbPropValue);
					dbPropValue.ItemId = retVal.Id;
				}
			}
			#endregion

			#region ItemAssets
			if (product.Assets != null)
			{
				var assets = product.Assets.ToArray();
				retVal.ItemAssets = new ObservableCollection<dataModel.ItemAsset>();
				for (int order = 0; order < assets.Length; order++)
				{
					var asset = assets[order];
					var dbAsset = asset.ToDataModel();
					dbAsset.SortOrder = order;
					retVal.ItemAssets.Add(dbAsset);
				}
			}
			#endregion

			#region CategoryItemRelations
			if (product.Links != null)
			{
				retVal.CategoryItemRelations = new ObservableCollection<dataModel.CategoryItemRelation>();
				retVal.CategoryItemRelations.AddRange(product.Links.Select(x => x.ToDataModel(product)));
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
		public static void Patch(this dataModel.Item source, dataModel.Item target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjectionPolicy = new PatchInjection<dataModel.Item>(x => x.Name, x => x.Code, x => x.IsBuyable, x=> x.IsActive, x=>x.TrackInventory);
			target.InjectFrom(patchInjectionPolicy, source);

			#region ItemAssets
			if (!source.ItemAssets.IsNullCollection())
			{
				source.ItemAssets.Patch(target.ItemAssets, (sourceAsset, targetAsset) => sourceAsset.Patch(targetAsset));
			}
			#endregion

			#region ItemPropertyValues
			if (!source.ItemPropertyValues.IsNullCollection())
			{
				source.ItemPropertyValues.Patch(target.ItemPropertyValues, (sourcePropValue, targetPropValue) => sourcePropValue.Patch(targetPropValue));
			}

			#endregion

			#region CategoryItemRelations
			if (!source.CategoryItemRelations.IsNullCollection())
			{
				source.CategoryItemRelations.Patch(target.CategoryItemRelations, new CategoryItemRelationComparer(),
										 (sourcePropValue, targetPropValue) => sourcePropValue.Patch(targetPropValue));
			}
			#endregion


			#region EditorialReviews
			if (!source.EditorialReviews.IsNullCollection())
			{
				source.EditorialReviews.Patch(target.EditorialReviews, (sourcePropValue, targetPropValue) => sourcePropValue.Patch(targetPropValue));
			}
			#endregion

			#region Association
			if (!source.AssociationGroups.IsNullCollection())
			{
				var associationComparer = AnonymousComparer.Create((dataModel.AssociationGroup x) => x.Name);
				source.AssociationGroups.Patch(target.AssociationGroups, associationComparer,
										 (sourceGroup, targetGroup) => sourceGroup.Patch(targetGroup));
			}
			#endregion
		}

	}
}
