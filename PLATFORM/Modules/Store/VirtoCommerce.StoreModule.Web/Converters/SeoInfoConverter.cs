using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Store.Model;
using webModel = VirtoCommerce.StoreModule.Web.Model;

namespace VirtoCommerce.StoreModule.Web.Converters
{
	public static class SeoInfoConverter
	{
		public static webModel.SeoInfo ToWebModel(this coreModel.SeoInfo seoInfo)
		{
			var retVal = new webModel.SeoInfo();
			retVal.InjectFrom(seoInfo);
			return retVal;
		}

		public static coreModel.SeoInfo ToCoreModel(this webModel.SeoInfo seoInfo)
		{
			var retVal = new coreModel.SeoInfo();
			retVal.InjectFrom(seoInfo);
			return retVal;
		}


	}
}
