using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
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
		public static coreModel.ItemAsset ToCoreModel(this dataModel.ItemAsset dbAsset)
		{
			if (dbAsset == null)
				throw new ArgumentNullException("dbAsset");

			var retVal = new coreModel.ItemAsset();

			retVal.InjectFrom(dbAsset);

			retVal.Group = dbAsset.GroupName;
			retVal.Type = (coreModel.ItemAssetType)Enum.Parse(typeof(coreModel.ItemAssetType), dbAsset.AssetType, true);
			retVal.Url = dbAsset.AssetId;

			return retVal;

		}

		/// <summary>
		/// Converting to foundation type
		/// </summary>
		/// <param name="itemAsset">The item asset.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">itemAsset</exception>
		public static dataModel.ItemAsset ToDataModel(this coreModel.ItemAsset itemAsset)
		{
			if (itemAsset == null)
				throw new ArgumentNullException("itemAsset");

			var retVal = new dataModel.ItemAsset();
			var id = retVal.Id;
			retVal.InjectFrom(itemAsset);
			if(itemAsset.Id == null)
			{
				retVal.Id = id;
			}

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
		public static void Patch(this dataModel.ItemAsset source, dataModel.ItemAsset target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			
			var patchInjectionPolicy = new PatchInjection<dataModel.ItemAsset>(x => x.AssetType, x=> x.SortOrder );
			target.InjectFrom(patchInjectionPolicy, source);

		}
	}
}
