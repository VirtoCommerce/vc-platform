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
	public static class LinkConverter
	{
		public static webModel.CategoryLink ToWebModel(this moduleModel.CategoryLink link)
		{
			var retVal = new webModel.CategoryLink();
			retVal.InjectFrom(link);
			return retVal;
		}

		public static moduleModel.CategoryLink ToModuleModel(this webModel.CategoryLink link)
		{
			var retVal = new moduleModel.CategoryLink();
			retVal.InjectFrom(link);
			return retVal;
		}


	}
}
