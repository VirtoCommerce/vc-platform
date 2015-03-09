using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webModels = VirtoCommerce.ThemeModule.Web.Models;
using coreModels = VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.ThemeModule.Web.Converters
{
	public static class GetThemeAssetsCriteriaConverter
	{
		public static coreModels.GetThemeAssetsCriteria ToCoreModel(this webModels.GetThemeAssetsCriteria criteria)
		{
			var retVal = new coreModels.GetThemeAssetsCriteria();

			retVal.LastUpdateDate = criteria.LastUpdateDate;
			retVal.LoadContent = criteria.LoadContent;

			return retVal;
		}
	}
}