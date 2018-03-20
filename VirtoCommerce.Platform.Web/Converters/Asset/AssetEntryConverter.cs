using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using coreModel = VirtoCommerce.Platform.Core.Assets;
using webModel = VirtoCommerce.Platform.Web.Model.Asset;

namespace VirtoCommerce.Platform.Web.Converters.Asset
{
    public static class AssetEntryConverter
    {
        public static coreModel.AssetEntry ToCoreModel(this webModel.AssetEntry item)
        {
            var retVal = AbstractTypeFactory<coreModel.AssetEntry>.TryCreateInstance();
            retVal.InjectFrom(item);
            retVal.BlobInfo = item.BlobInfo.ToCoreModel();
            return retVal;
        }

        public static coreModel.BlobInfo ToCoreModel(this webModel.BlobInfo item)
        {
            var retVal = AbstractTypeFactory<coreModel.BlobInfo>.TryCreateInstance();
            retVal.InjectFrom(item);
            retVal.ContentType = item.MimeType;
            retVal.FileName = item.Name;
            return retVal;
        }

        public static webModel.AssetEntrySearchResult ToWebModel(this coreModel.AssetEntrySearchResult searchResult)
        {
            var retVal = new webModel.AssetEntrySearchResult();
            retVal.TotalCount = searchResult.TotalCount;
            retVal.Assets = searchResult.Results.Select(ToWebModel).ToArray();

            return retVal;
        }

        public static webModel.AssetEntry ToWebModel(this coreModel.AssetEntry item)
        {
            var retVal = new webModel.AssetEntry();
            retVal.InjectFrom(item);
            retVal.BlobInfo = item.BlobInfo.ToWebModel2();
            return retVal;
        }

        public static webModel.BlobInfo ToWebModel2(this coreModel.BlobInfo item)
        {
            var retVal = new webModel.BlobInfo();
            retVal.InjectFrom(item);
            retVal.MimeType = item.ContentType;
            retVal.Name = item.FileName;
            retVal.Size = item.Size.ToString();
            return retVal;
        }
    }
}
