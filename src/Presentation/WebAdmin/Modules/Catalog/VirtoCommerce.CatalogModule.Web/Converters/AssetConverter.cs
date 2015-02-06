using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
	public static class AssetConverter
	{
		public static webModel.ProductAssetBase ToWebModel(this moduleModel.ItemAsset assset, Uri baseUri)
		{
			webModel.ProductAssetBase retVal = new webModel.ProductImage();
			if(assset.Type == moduleModel.ItemAssetType.File)
			{
				retVal = new webModel.ProductAsset();
			}
			retVal.InjectFrom(assset);
			retVal.Url = new Uri(baseUri, assset.Url).ToString();
			return retVal;
		}

		public static moduleModel.ItemAsset ToModuleModel(this webModel.ProductAssetBase assetBase)
		{
			var retVal = new moduleModel.ItemAsset();
			retVal.InjectFrom(assetBase);
			retVal.Url = new Uri(assetBase.Url).AbsolutePath;
			if(String.IsNullOrEmpty(retVal.Group))
			{
				retVal.Group = "default";
			}
			return retVal;
		}


	}
}
