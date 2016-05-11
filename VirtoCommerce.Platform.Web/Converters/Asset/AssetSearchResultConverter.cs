using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Asset;
using webModel = VirtoCommerce.Platform.Web.Model.Asset;
using coreModel = VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.Platform.Web.Converters.Asset
{
    public static class AssetSearchResultConverter
    {
        public static webModel.AssetListItem[] ToWebModel(this BlobSearchResult searchResult)
        {
            var retVal = new List<webModel.AssetListItem>();
            retVal.AddRange(searchResult.Folders.Select(ToWebModel));
            retVal.AddRange(searchResult.Items.Select(ToWebModel));
            return retVal.ToArray();
        }

        public static webModel.AssetListItem ToWebModel(this coreModel.BlobFolder assetFolder)
        {
            var retVal = new webModel.AssetListItem
            {
                Name = assetFolder.Name,
                ParentUrl = assetFolder.ParentUrl,
                Url = assetFolder.Url,
                Type = "folder"
            };
            return retVal;
        }

        public static webModel.AssetListItem ToWebModel(this coreModel.BlobInfo blobInfo)
        {
            var retVal = new webModel.AssetListItem
            {
                Name = blobInfo.FileName,
                Url = blobInfo.Url,
                Size = blobInfo.Size.ToHumanReadableSize(),
                ContentType = blobInfo.ContentType,
                Type = "blob",
                ModifiedDate = blobInfo.ModifiedDate
            };
            return retVal;
        }
    }
}