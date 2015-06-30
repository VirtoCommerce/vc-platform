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

      	public static webModel.Image ToWebModel(this moduleModel.Image image, IBlobUrlResolver blobUrlResolver)
        {
            var retVal = new webModel.Image();
			retVal.InjectFrom(image);
			retVal.Src = blobUrlResolver.GetAbsoluteUrl(image.Url);
            retVal.ThumbSrc = retVal.Src;
			if(retVal.Name == null)
			{
				retVal.Name = retVal.Group;
			}
            return retVal;
        }

		public static webModel.Asset ToWebModel(this moduleModel.Asset asset, IBlobUrlResolver blobUrlResolver)
		{
			var retVal = new webModel.Asset();
			retVal.InjectFrom(asset);
			if (asset.Name == null)
			{
				retVal.Name = HttpUtility.UrlDecode(System.IO.Path.GetFileName(asset.Url));
			}
			if (asset.MimeType == null)
			{
				retVal.MimeType = MimeTypeResolver.ResolveContentType(retVal.Name);
			}

			retVal.Url = blobUrlResolver.GetAbsoluteUrl(asset.Url);
			return retVal;
		}

        #endregion
    }
}
