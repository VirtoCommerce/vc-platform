using System;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Assets.Services;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
    public static class AssetConverter
    {
        public static webModel.ProductAssetBase ToWebModel(this moduleModel.ItemAsset asset, IAssetUrlResolver assetUrlResolver)
        {
            webModel.ProductAssetBase retVal = new webModel.ProductImage();
            if (asset.Type == moduleModel.ItemAssetType.File)
            {
                retVal = new webModel.ProductAsset();
            }
            retVal.InjectFrom(asset);
            retVal.Url = assetUrlResolver.GetAbsoluteUrl(asset.Url);
            return retVal;
        }

        public static moduleModel.ItemAsset ToModuleModel(this webModel.ProductAssetBase assetBase, IAssetUrlResolver assetUrlResolver)
        {
            var retVal = new moduleModel.ItemAsset();
            retVal.InjectFrom(assetBase);
            retVal.Url = assetUrlResolver.GetRelativeUrl(assetBase.Url).TrimStart('/');
            if (String.IsNullOrEmpty(retVal.Group))
            {
                retVal.Group = "default";
            }
            return retVal;
        }
    }
}
