using System;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Asset;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class AssetConverter
    {
        #region Public Methods and Operators

        public static moduleModel.ItemAsset ToModuleModel(this webModel.ItemImage itemInage, IBlobUrlResolver blobUrlResolver)
        {
            var retVal = new moduleModel.ItemAsset();
            retVal.InjectFrom(itemInage);
            if (String.IsNullOrEmpty(retVal.Group))
            {
                retVal.Group = "default";
            }
            return retVal;
        }

		public static webModel.ItemImage ToWebModel(this moduleModel.ItemAsset asset, IBlobUrlResolver blobUrlResolver)
        {
            var retVal = new webModel.ItemImage();
            retVal.InjectFrom(asset);
			retVal.Src = blobUrlResolver.GetAbsoluteUrl(asset.Url);
            retVal.ThumbSrc = retVal.Src;
            retVal.Name = asset.Group;
            return retVal;
        }

        #endregion
    }
}
