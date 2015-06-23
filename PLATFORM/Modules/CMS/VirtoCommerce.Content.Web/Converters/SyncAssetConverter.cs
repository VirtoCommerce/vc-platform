using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Omu.ValueInjecter;
using VirtoCommerce.Content.Data.Utility;
using VirtoCommerce.Content.Web.Models;

namespace VirtoCommerce.Content.Web.Converters
{
    public static class SyncAssetConverter
    {
        public static SyncAsset ToSyncModel(this ThemeAsset item)
        {
            var retVal = new SyncAsset();
            retVal.InjectFrom(item);
            return retVal;
        }

        public static SyncAsset ToSyncModel(this Page item)
        {
            var retVal = new SyncAsset();
            retVal.InjectFrom(item);
            retVal.Updated = item.ModifiedDate;

            retVal.Id = ContentTypeUtility.IsImageContentType(item.ContentType) ? item.Name : String.Format("{0}/{1}", item.Language, item.Name);
            return retVal;
        }
    }
}