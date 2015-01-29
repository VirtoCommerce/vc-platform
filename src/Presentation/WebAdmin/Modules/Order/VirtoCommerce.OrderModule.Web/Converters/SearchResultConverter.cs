using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;

namespace VirtoCommerce.OrderModule.Web.Converters
{
	public static class SearchResultConverter
	{
		public static webModel.SearchResult ToWebModel(this coreModel.SearchResult result)
		{
			var retVal = new webModel.SearchResult();

			retVal.InjectFrom(result);
			retVal.CustomerOrders = result.CustomerOrders.Select(x => x.ToWebModel()).ToList();

			return retVal;
		}

	}
}
