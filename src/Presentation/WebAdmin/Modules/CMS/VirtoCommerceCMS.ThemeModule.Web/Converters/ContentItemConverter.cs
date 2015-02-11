using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webModels = VirtoCommerceCMS.ThemeModule.Web.Models;
using dataModels = VirtoCommerceCMS.Data.Models;

namespace VirtoCommerceCMS.ThemeModule.Web
{
	public static class ContentItemConverter
	{
		public static dataModels.ContentItem ToDomainModel(this webModels.ContentItem item)
		{
			var retVal = new dataModels.ContentItem();

			retVal.Content = item.Content;
			retVal.CreatedDate = DateTime.UtcNow;
			retVal.ContentType = item.Type == webModels.ContentType.Directory ? dataModels.ContentType.Directory : dataModels.ContentType.File;
			retVal.Name = item.Name;
			retVal.Path = item.Path;
			retVal.ParentContentItemId = item.ParentContentItemId;

			return retVal;
		}

		public static webModels.ContentItem ToWebModel(this dataModels.ContentItem item)
		{
			var retVal = new webModels.ContentItem();

			retVal.Content = item.Content;
			retVal.Name = item.Name;
			retVal.Path = item.Path;
			retVal.ParentContentItemId = item.ParentContentItemId;
			retVal.Type = item.ContentType == dataModels.ContentType.Directory ? webModels.ContentType.Directory : webModels.ContentType.File;

			return retVal;
		}
	}
}