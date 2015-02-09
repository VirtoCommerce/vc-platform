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
	public static class SeoInfoConverter
	{
		public static webModel.SeoInfo ToWebModel(this moduleModel.SeoInfo seoInfo)
		{
			var retVal = new webModel.SeoInfo();
			retVal.InjectFrom(seoInfo);
			return retVal;
		}

		public static moduleModel.SeoInfo ToModuleModel(this webModel.SeoInfo seoInfo)
		{
			var retVal = new moduleModel.SeoInfo();
			retVal.InjectFrom(seoInfo);
			return retVal;
		}


	}
}
