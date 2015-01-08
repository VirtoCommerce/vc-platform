using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Cart.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
	public static class SearchResultConverter
	{
		public static webModel.SearchResult ToWebModel(this coreModel.SearchResult result)
		{
			var retVal = new webModel.SearchResult();
			retVal.InjectFrom(result);

			return retVal;
		}

		public static coreModel.SearchResult ToCoreModel(this webModel.SearchResult result)
		{
			var retVal = new coreModel.SearchResult();
			retVal.InjectFrom(result);
			return retVal;
		}


	}
}
