using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Cart.Model;
using webModel = VirtoCommerce.CartModule.Web.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CartModule.Web.Converters
{
	public static class DiscountConverter
	{
		public static webModel.Discount ToWebModel(this coreModel.Discount discount)
		{
			var retVal = new webModel.Discount();
			retVal.InjectFrom(discount);
			retVal.Currency = discount.Currency;
			return retVal;
		}

		public static coreModel.Discount ToCoreModel(this webModel.Discount discount)
		{
			var retVal = new coreModel.Discount();
			retVal.InjectFrom(discount);
			retVal.Currency = discount.Currency;
			return retVal;
		}


	}
}
