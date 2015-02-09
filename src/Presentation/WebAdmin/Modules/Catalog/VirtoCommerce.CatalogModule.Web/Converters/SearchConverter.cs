using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
	public static class SearchConverter
	{
		public static webModel.ListEntrySearchCriteria ToWebModel(this moduleModel.SearchCriteria criteria)
		{
			var retVal = new webModel.ListEntrySearchCriteria();
			retVal.InjectFrom(criteria);
			retVal.ResponseGroup = (webModel.ResponseGroup)(int)criteria.ResponseGroup;
			return retVal;
		}

		public static moduleModel.SearchCriteria ToModuleModel(this webModel.ListEntrySearchCriteria criteria)
		{
			var retVal = new moduleModel.SearchCriteria();
			retVal.InjectFrom(criteria);
			retVal.ResponseGroup = (moduleModel.ResponseGroup)(int)criteria.ResponseGroup;
			
			return retVal;
		}


	}
}
