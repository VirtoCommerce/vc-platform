using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webModels = VirtoCommerce.Content.Web.Models;
using coreModels = VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Platform.Core.Common;
using System.Text;
using VirtoCommerce.Content.Data.Utility;
using VirtoCommerce.Platform.Core.Asset;

namespace VirtoCommerce.Content.Web.Converters
{
	public static class PageConverter
	{
	    public static webModels.Page ToPageWebModel(this BlobInfo item)
        {
            var retVal = new webModels.Page();
            retVal.Name = item.FileName;
            //Get language from file name file format name.lang.extension
            var fileNameParts = item.FileName.Split('.');
            if(fileNameParts.Count() == 3)
            {
                retVal.Language = fileNameParts[1];
                retVal.Name = fileNameParts[0] + "." + fileNameParts[2];
            }
            retVal.ContentType = item.ContentType;
            retVal.ModifiedDate = item.ModifiedDate ?? default(DateTime);
            return retVal;
        }
    }
}