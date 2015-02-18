using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Customer.Model;
using webModel = VirtoCommerce.CustomerModule.Web.Model;

namespace VirtoCommerce.CustomerModule.Web.Converters
{
	public static class SearchResultConverter
	{
		public static webModel.SearchResult ToWebModel(this coreModel.SearchResult result)
		{
			var retVal = new webModel.SearchResult();

			retVal.InjectFrom(result);
			retVal.Contacts = result.Contacts.Select(x => x.ToWebModel()).ToList();

			return retVal;
		}

	}
}
