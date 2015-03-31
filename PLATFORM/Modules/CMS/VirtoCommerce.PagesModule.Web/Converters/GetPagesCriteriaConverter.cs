using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webModels = VirtoCommerce.PagesModule.Web.Models;
using coreModels = VirtoCommerce.Content.Pages.Data.Models;

namespace VirtoCommerce.PagesModule.Web.Converters
{
	public static class GetPagesCriteriaConverter
	{
		public static coreModels.GetPagesCriteria ToCoreModel(this webModels.GetPagesCriteria criteria)
		{
			return new coreModels.GetPagesCriteria
			{
				LastUpdateDate = criteria.LastUpdateDate
			};
		}
	}
}