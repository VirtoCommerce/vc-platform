using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Cart.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CartModule.Web.Converters
{
	public static class AddressConverter
	{
		public static webModel.Address ToWebModel(this coreModel.Address address)
		{
			var retVal = new webModel.Address();
			retVal.InjectFrom(address);
			retVal.Type = address.AddressType;
			return retVal;
		}

		public static coreModel.Address ToCoreModel(this webModel.Address address)
		{
			var retVal = new coreModel.Address();
			retVal.InjectFrom(address);
			retVal.AddressType = address.Type;
			return retVal;
		}


	}
}
