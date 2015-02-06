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
	public static class CatalogConverter
	{
		public static webModel.Catalog ToWebModel(this moduleModel.Catalog catalog)
		{
			var retVal = new webModel.Catalog();
			retVal.InjectFrom(catalog);
			if (catalog.Languages != null)
			{
				retVal.Languages = catalog.Languages.Select(x=>x.ToWebModel()).ToList();
			}
			return retVal;
		}

		public static moduleModel.Catalog ToModuleModel(this webModel.Catalog catalog)
		{
			var retVal = new moduleModel.Catalog();
			retVal.InjectFrom(catalog);
			if (catalog.Languages != null)
			{
				retVal.Languages = catalog.Languages.Select(x => x.ToModuleModel()).ToList();
			}
			return retVal;
		}


	}
}
