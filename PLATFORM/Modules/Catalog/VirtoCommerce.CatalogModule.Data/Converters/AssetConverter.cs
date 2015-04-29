using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using foundation = VirtoCommerce.CatalogModule.Data.Model;
using module = VirtoCommerce.Domain.Catalog.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

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

			var retVal = new module.ItemAsset();

			retVal.InjectFrom(dbAsset);

			retVal.Group = dbAsset.GroupName;
			retVal.Type = (module.ItemAssetType)Enum.Parse(typeof(module.ItemAssetType), dbAsset.AssetType, true);
			retVal.Url = dbAsset.AssetId;

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

			var retVal = new foundation.ItemAsset();
			retVal.InjectFrom(itemAsset);

			retVal.AssetId = itemAsset.Url;
			retVal.GroupName = itemAsset.Group;
			retVal.AssetType = itemAsset.Type.ToString().ToLower();

		
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
			
			var patchInjectionPolicy = new PatchInjection<foundation.ItemAsset>(x => x.AssetType, x=> x.SortOrder );
			target.InjectFrom(patchInjectionPolicy, source);

		}
	}
}
