using System;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Assets.Services;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class AssetConverter
    {
        #region Public Methods and Operators

        public static moduleModel.ItemAsset ToModuleModel(this webModel.ItemImage itemInage)
        {
            var retVal = new moduleModel.ItemAsset();
            retVal.InjectFrom(itemInage);
            if (String.IsNullOrEmpty(retVal.Group))
            {
                retVal.Group = "default";
            }
            return retVal;
        }

        public static webModel.ItemImage ToWebModel(this moduleModel.ItemAsset assset, IAssetUrl resolve)
        {
            var retVal = new webModel.ItemImage();
            retVal.InjectFrom(assset);
            retVal.Src = resolve.ResolveUrl(assset.Url);
            retVal.ThumbSrc = resolve.ResolveUrl(assset.Url);
            retVal.Name = assset.Group;
            return retVal;
        }

        #endregion
    }
}
