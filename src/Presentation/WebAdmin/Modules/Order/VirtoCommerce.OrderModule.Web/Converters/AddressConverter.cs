using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;

namespace VirtoCommerce.OrderModule.Web.Converters
{
	public static class AddressConverter
	{
		public static webModel.Address ToWebModel(this coreModel.Address address)
		{
			var retVal = new webModel.Address();
			retVal.InjectFrom(address);

			return retVal;
		}

		public static coreModel.Address ToCoreModel(this webModel.Address address)
		{
			var retVal = new coreModel.Address();
			retVal.InjectFrom(address);
			return retVal;
		}


	}
}