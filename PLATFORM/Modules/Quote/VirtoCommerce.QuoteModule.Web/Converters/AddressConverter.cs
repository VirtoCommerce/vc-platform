using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Quote.Model;
using webModel = VirtoCommerce.QuoteModule.Web.Model;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.QuoteModule.Web.Converters
{
	public static class AddressConverter
	{
		public static webModel.Address ToWebModel(this Address address)
		{
			var retVal = new webModel.Address();
			retVal.InjectFrom(address);
            retVal.AddressType = address.AddressType;
			return retVal;
		}

		public static Address ToCoreModel(this webModel.Address address)
		{
			var retVal = new Address();
			retVal.InjectFrom(address);
            retVal.AddressType = address.AddressType;
            return retVal;
		}


	}
}