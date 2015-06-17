using System;
using System.Web;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class AssetConverter
    {
        #region Public Methods and Operators

      	public static webModel.ItemImage ToImageWebModel(this moduleModel.ItemAsset asset, IBlobUrlResolver blobUrlResolver)
        {
            var retVal = new webModel.ItemImage();
            retVal.InjectFrom(asset);
			retVal.Src = blobUrlResolver.GetAbsoluteUrl(asset.Url);
            retVal.ThumbSrc = retVal.Src;
            retVal.Name = asset.Group;
            return retVal;
        }

		public static webModel.Asset ToAssetWebModel(this moduleModel.ItemAsset asset, IBlobUrlResolver blobUrlResolver)
		{
			var retVal = new webModel.Asset();
			retVal.InjectFrom(asset);

			retVal.Name = HttpUtility.UrlDecode(System.IO.Path.GetFileName(asset.Url));
			retVal.MimeType = MimeTypeResolver.ResolveContentType(retVal.Name);

			retVal.Url = blobUrlResolver.GetAbsoluteUrl(asset.Url);
			return retVal;
		}

        #endregion
    }
}
