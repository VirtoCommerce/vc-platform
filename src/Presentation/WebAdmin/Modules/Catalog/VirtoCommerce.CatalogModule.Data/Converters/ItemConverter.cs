using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using module = VirtoCommerce.CatalogModule.Model;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
	public static class ItemConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <returns></returns>
		public static module.CatalogProduct ToModuleModel(this foundation.Item dbItem, module.Catalog catalog,
														  module.Category category, module.Property[] properties,
														  foundation.Item[] variations,
														  string mainProductId)
		{
			var retVal = new module.CatalogProduct {Id = dbItem.ItemId, Catalog = catalog, CatalogId = catalog.Id};
		    if (category != null)
			{
				retVal.Category = category;
				retVal.CategoryId = category.Id;
			}
			retVal.Code = dbItem.Code;
			retVal.Name = dbItem.Name;
			retVal.MainProductId = mainProductId;

			#region Variations
			if (variations != null)
			{
				retVal.Variations = new List<module.CatalogProduct>();
				foreach (var variation in variations)
				{
					var productVaraition = variation.ToModuleModel(catalog, category, properties, null, retVal.Id);
					productVaraition.MainProduct = retVal;
					productVaraition.MainProductId = retVal.Id;
					retVal.Variations.Add(productVaraition);
				}
			}
			#endregion

			#region Assets
			if (dbItem.ItemAssets != null)
			{
				retVal.Assets = dbItem.ItemAssets.OrderBy(x=>x.SortOrder).Select(x => x.ToModuleModel()).ToList();
			}
			#endregion

			#region Property values
			if (dbItem.ItemPropertyValues != null)
			{
				retVal.PropertyValues = dbItem.ItemPropertyValues.Select(x => x.ToModuleModel(properties)).ToList();
			}
			#endregion


			return retVal;
		}

		/// <summary>
		/// Converting to foundation type
		/// </summary>
		/// <param name="catalog"></param>
		/// <returns></returns>
		public static foundation.Item ToFoundation(this module.CatalogProduct product)
		{

			var retVal = new foundation.Product();

			//Constant fields
			retVal.IsActive = true;
			retVal.AvailabilityRule = (int)foundation.AvailabilityRule.Always;
			retVal.StartDate = DateTime.UtcNow;
			retVal.IsBuyable = true;
			retVal.MinQuantity = 1;
			retVal.MaxQuantity = 0;

			//Changed fields
			retVal.Name = product.Name;
			retVal.Code = product.Code;
			retVal.CatalogId = product.CatalogId;

			if (product.Id != null)
				retVal.ItemId = product.Id;

			retVal.ItemPropertyValues = new NullCollection<foundation.ItemPropertyValue>();
			if (product.PropertyValues != null)
			{
				retVal.ItemPropertyValues = new ObservableCollection<foundation.ItemPropertyValue>();
				foreach (var propValue in product.PropertyValues)
				{
					var dbPropValue = propValue.ToFoundation<foundation.ItemPropertyValue>() as foundation.ItemPropertyValue;
					dbPropValue.ItemId = retVal.ItemId;
					retVal.ItemPropertyValues.Add(dbPropValue);
				}
			}
			retVal.ItemAssets = new NullCollection<foundation.ItemAsset>();
		    
			if (product.Assets != null)
			{
                var assets = product.Assets.ToArray();
				retVal.ItemAssets = new ObservableCollection<foundation.ItemAsset>();
				for (int order = 0; order < assets.Length; order++)
				{
				    var asset = assets[order];
					var dbAsset = asset.ToFoundation();
					dbAsset.ItemId = product.Id;
				    dbAsset.SortOrder = order;
					retVal.ItemAssets.Add(dbAsset);
				}
			}
			return retVal;
		}


		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundation.Item source, foundation.Item target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			//Simply properties patch
			if (source.Name != null)
				target.Name = source.Name;
			if (source.Code != null)
				target.Code = source.Code;

			//Asset patch
			if (!source.ItemAssets.IsNullCollection())
			{
				source.ItemAssets.Patch(target.ItemAssets, new ItemAssetComparer(),
										 (sourceAsset, targetAsset) => sourceAsset.Patch(targetAsset));
			}
			//Property values
			if (!source.ItemPropertyValues.IsNullCollection())
			{
				source.ItemPropertyValues.Patch(target.ItemPropertyValues, new PropertyValueComparer(),
										 (sourcePropValue, targetPropValue) => sourcePropValue.Patch(targetPropValue));
			}

		}

	}

	public class ItemComparer : IEqualityComparer<foundation.Item>
	{
		#region IEqualityComparer<Item> Members

		public bool Equals(foundation.Item x, foundation.Item y)
		{
			return x.ItemId == y.ItemId;
		}

		public int GetHashCode(foundation.Item obj)
		{
			return obj.GetHashCode();
		}

		#endregion
	}
}
