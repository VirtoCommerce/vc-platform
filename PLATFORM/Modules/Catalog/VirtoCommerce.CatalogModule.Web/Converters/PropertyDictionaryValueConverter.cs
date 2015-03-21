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
	public static class PropertyDictionaryValue
	{
		public static webModel.PropertyDictionaryValue ToWebModel(this moduleModel.PropertyDictionaryValue propDictValue)
		{
			var retVal = new webModel.PropertyDictionaryValue();
			retVal.InjectFrom(propDictValue);

			return retVal;
		}

		public static moduleModel.PropertyDictionaryValue ToModuleModel(this webModel.PropertyDictionaryValue propDictValue)
		{
			var retVal = new moduleModel.PropertyDictionaryValue();
			retVal.InjectFrom(propDictValue);
			return retVal;
		}


	}
}
