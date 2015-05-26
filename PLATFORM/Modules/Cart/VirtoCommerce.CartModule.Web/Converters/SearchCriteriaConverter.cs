using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Cart.Model;
using webModel = VirtoCommerce.CartModule.Web.Model;

namespace VirtoCommerce.CartModule.Web.Converters
{
	public static class SearchCriteriaConverter
	{
		public static webModel.SearchCriteria ToWebModel(this coreModel.SearchCriteria criteria)
		{
			var retVal = new webModel.SearchCriteria();
			retVal.InjectFrom(criteria);

			return retVal;
		}

		public static coreModel.SearchCriteria ToCoreModel(this webModel.SearchCriteria criteria)
		{
			var retVal = new coreModel.SearchCriteria();
			retVal.InjectFrom(criteria);
			return retVal;
		}


	}
}
