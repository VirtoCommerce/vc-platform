using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webModels = VirtoCommerce.Content.Web.Models;
using coreModels = VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Web.Converters
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