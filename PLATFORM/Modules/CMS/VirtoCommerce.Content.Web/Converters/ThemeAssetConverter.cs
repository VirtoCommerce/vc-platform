using System.Collections.Generic;
using System.Linq;
using webModels = VirtoCommerce.Content.Web.Models;
using domainModels = VirtoCommerce.Content.Data.Models;
using System.Text;
using VirtoCommerce.Content.Data.Utility;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Asset;
using System;

namespace VirtoCommerce.Content.Web.Converters
{
	public static class ThemeAssetConverter
	{
	
        public static webModels.ThemeAsset ToThemeAssetWebModel(this BlobInfo item)
        {
            var retVal = new webModels.ThemeAsset();
            retVal.Name = item.FileName;
            retVal.ContentType = item.ContentType;
            retVal.Updated = item.ModifiedDate ?? default(DateTime);
            return retVal;
        }

        public static webModels.ThemeAssetFolder ToThemeFolderWebModel(this BlobFolder blobFolder)
        {
            var retVal = new webModels.ThemeAssetFolder();
            retVal.FolderName = blobFolder.Name;
            return retVal;
        
        }
    }
}