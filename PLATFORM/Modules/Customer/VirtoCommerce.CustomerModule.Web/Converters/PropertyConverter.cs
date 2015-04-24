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
	public static class PropertyConverter
	{
		public static webModel.Property ToWebModel(this coreModel.Property property)
		{
			var retVal = new webModel.Property();
			retVal.InjectFrom(property);
			if(property.Value != null)
				retVal.Value = property.Value.ToString();
			return retVal;
		}

		public static coreModel.Property ToCoreModel(this webModel.Property property)
		{
			var retVal = new coreModel.Property();
			retVal.InjectFrom(property);
			retVal.Value = property.Value;
			return retVal;
		}


	}
}
