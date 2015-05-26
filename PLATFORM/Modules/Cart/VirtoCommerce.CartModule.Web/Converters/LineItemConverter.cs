using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Cart.Model;
using webModel = VirtoCommerce.CartModule.Web.Model;

namespace VirtoCommerce.CartModule.Web.Converters
{
	public static class LineItemConverter
	{
		public static webModel.LineItem ToWebModel(this coreModel.LineItem cartItem)
		{
			var retVal = new webModel.LineItem();
			retVal.InjectFrom(cartItem);
			retVal.Currency = cartItem.Currency;
			if (cartItem.Discounts != null)
				retVal.Discounts = cartItem.Discounts.Select(x => x.ToWebModel()).ToList();
			return retVal;
		}

		public static coreModel.LineItem ToCoreModel(this webModel.LineItem cartItem)
		{
			var retVal = new coreModel.LineItem();
			retVal.InjectFrom(cartItem);

			retVal.Currency = cartItem.Currency;

			if(cartItem.Discounts != null)
				retVal.Discounts = cartItem.Discounts.Select(x => x.ToCoreModel()).ToList();
			return retVal;
		}


	}
}
