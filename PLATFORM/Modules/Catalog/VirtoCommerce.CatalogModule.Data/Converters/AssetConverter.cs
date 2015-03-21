using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using module = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
	public static class AssetConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static module.ItemAsset ToModuleModel(this foundation.ItemAsset dbAsset)
		{
			if (dbAsset == null)
				throw new ArgumentNullException("dbAsset");
			var retVal = new module.ItemAsset
			{
				Id = dbAsset.ItemAssetId,
				ItemId = dbAsset.ItemId,
				Group = dbAsset.GroupName,
				Type = (module.ItemAssetType)Enum.Parse(typeof(module.ItemAssetType), dbAsset.AssetType, true),
				Url = dbAsset.AssetId
			};
			return retVal;

		}

		/// <summary>
		/// Converting to foundation type
		/// </summary>
		/// <param name="itemAsset">The item asset.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">itemAsset</exception>
		public static foundation.ItemAsset ToFoundation(this module.ItemAsset itemAsset)
		{
			if (itemAsset == null)
				throw new ArgumentNullException("itemAsset");

			var retVal = new foundation.ItemAsset
			{
				AssetId = itemAsset.Url,
				GroupName = itemAsset.Group,
				AssetType = itemAsset.Type.ToString().ToLower(),
				ItemId = itemAsset.ItemId,
			};
			if (itemAsset.Id != null)
			{
				retVal.ItemAssetId = itemAsset.Id;
			}
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundation.ItemAsset source, foundation.ItemAsset target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			//Simply properties patch
			if (source.AssetType != null)
				target.AssetType = source.AssetType;

			target.SortOrder = source.SortOrder;

		}
	}

	public class ItemAssetComparer : IEqualityComparer<foundation.ItemAsset>
	{
		#region IEqualityComparer<Item> Members

		public bool Equals(foundation.ItemAsset x, foundation.ItemAsset y)
		{
			return x.ItemAssetId == y.ItemAssetId;
		}

		public int GetHashCode(foundation.ItemAsset obj)
		{
			return obj.ItemAssetId.GetHashCode();
		}

		#endregion
	}
}
