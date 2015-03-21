using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Money;
using coreModel = VirtoCommerce.Domain.Customer.Model;
using webModel = VirtoCommerce.CustomerModule.Web.Model;

namespace VirtoCommerce.CustomerModule.Web.Converters
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
