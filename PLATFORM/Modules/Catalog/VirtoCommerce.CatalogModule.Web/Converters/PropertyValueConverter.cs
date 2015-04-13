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
	public static class PropertyValueConverter
	{
		public static webModel.PropertyValue ToWebModel(this moduleModel.PropertyValue propValue)
		{
			var retVal = new webModel.PropertyValue();
			retVal.InjectFrom(propValue);
		    retVal.Value = propValue.Value;
			retVal.ValueType = propValue.ValueType;
		
			return retVal;
		}

		public static moduleModel.PropertyValue ToModuleModel(this webModel.PropertyValue propValue)
		{
			var retVal = new moduleModel.PropertyValue();
			retVal.InjectFrom(propValue);
            retVal.Value = propValue.Value;
			retVal.ValueType = (moduleModel.PropertyValueType)(int)propValue.ValueType;
			return retVal;
		}


	}
}
