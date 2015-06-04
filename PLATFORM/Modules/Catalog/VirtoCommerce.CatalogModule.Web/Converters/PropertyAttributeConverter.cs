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
	public static class PropertyAttributeConverter
	{
		public static webModel.PropertyAttribute ToWebModel(this moduleModel.PropertyAttribute attribute)
		{
			var retVal = new webModel.PropertyAttribute();
			retVal.InjectFrom(attribute);
			return retVal;
		}

		public static moduleModel.PropertyAttribute ToModuleModel(this webModel.PropertyAttribute attribute)
		{
			var retVal = new moduleModel.PropertyAttribute();
			retVal.InjectFrom(attribute);
			return retVal;
		}


	}
}
