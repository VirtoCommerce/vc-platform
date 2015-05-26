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
	public static class SearchResultConverter
	{
		public static webModel.SearchResult ToWebModel(this coreModel.SearchResult result)
		{
			var retVal = new webModel.SearchResult();

			retVal.InjectFrom(result);
			retVal.ShopingCarts = result.ShopingCarts.Select(x => x.ToWebModel()).ToList();

			return retVal;
		}

	}
}
