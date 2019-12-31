using System.Collections.Generic;
using System.Linq;
using coreModel = VirtoCommerce.Platform.Core.Assets;
using webModel = VirtoCommerce.Platform.Web.Model.Asset;

namespace VirtoCommerce.Platform.Web.Converters.Asset
{
    public static class AssetSearchResultConverter
    {
        public static webModel.AssetListItem[] ToWebModel(this coreModel.BlobSearchResult searchResult)
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
                Type = "folder",
                RelativeUrl = assetFolder.RelativeUrl
            };
            return retVal;
        }

        public static webModel.AssetListItem ToWebModel(this coreModel.BlobInfo blobInfo)
        {
            var retVal = new webModel.AssetListItem
            {
                Name = blobInfo.FileName,
                Url = blobInfo.Url,
                Size = blobInfo.Size,
                ContentType = blobInfo.ContentType,
                Type = "blob",
                ModifiedDate = blobInfo.ModifiedDate,
                RelativeUrl = blobInfo.RelativeUrl
            };
            return retVal;
        }
    }
}
