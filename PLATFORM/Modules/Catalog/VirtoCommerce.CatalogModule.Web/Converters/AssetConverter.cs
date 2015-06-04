using System;
using System.Web;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
    public static class AssetConverter
    {
        public static webModel.ProductAssetBase ToWebModel(this moduleModel.ItemAsset asset, IBlobUrlResolver blobUrlResolver)
        {
            webModel.ProductAssetBase retVal = new webModel.ProductImage();
            if (asset.Type == moduleModel.ItemAssetType.File)
            {
                var productAsset  = new webModel.ProductAsset();

				productAsset.Name = HttpUtility.UrlDecode(System.IO.Path.GetFileName(asset.Url));
				productAsset.MimeType = MimeTypeResolver.ResolveContentType(productAsset.Name);		

				retVal = productAsset;
            }
            retVal.InjectFrom(asset);
			if (!Uri.IsWellFormedUriString(asset.Url, UriKind.Absolute))
			{
				retVal.Url = blobUrlResolver.GetAbsoluteUrl(asset.Url);

			}
			retVal.RelativeUrl = asset.Url;
            return retVal;
        }

		public static moduleModel.ItemAsset ToModuleModel(this webModel.ProductAssetBase assetBase)
        {
            var retVal = new moduleModel.ItemAsset();
            retVal.InjectFrom(assetBase);
			retVal.Url = assetBase.RelativeUrl;
            if (String.IsNullOrEmpty(retVal.Group))
            {
                retVal.Group = "default";
            }
            return retVal;
        }
    }
}
