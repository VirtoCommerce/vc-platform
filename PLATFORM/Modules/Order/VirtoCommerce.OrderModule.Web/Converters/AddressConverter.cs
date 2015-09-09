using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.OrderModule.Web.Converters
{
	public static class AddressConverter
	{
		public static webModel.Address ToWebModel(this Address address)
		{
			var retVal = new webModel.Address();
			retVal.InjectFrom(address);

			return retVal;
		}

		public static Address ToCoreModel(this webModel.Address address)
		{
			var retVal = new Address();
			retVal.InjectFrom(address);
			return retVal;
		}


	}
}