using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;

namespace VirtoCommerce.OrderModule.Web.Converters
{
	public static class CustomerOrderItemConverter
	{
		public static webModel.LineItem ToWebModel(this coreModel.LineItem orderItem)
		{
			var retVal = new webModel.LineItem();
			retVal.InjectFrom(orderItem);
			retVal.Currency = orderItem.Currency;
           
			retVal.TaxDetails = orderItem.TaxDetails;

			if (orderItem.DynamicProperties != null)
				retVal.DynamicProperties = orderItem.DynamicProperties;

			return retVal;
		}

		public static coreModel.LineItem ToCoreModel(this webModel.LineItem orderItem)
		{
			var retVal = new coreModel.LineItem();
			retVal.InjectFrom(orderItem);
			retVal.Currency = orderItem.Currency;
			if(orderItem.Discount != null)
			{
				retVal.Discount = orderItem.Discount.ToCoreModel();
			}
			retVal.TaxDetails = orderItem.TaxDetails;

			if (orderItem.DynamicProperties != null)
				retVal.DynamicProperties = orderItem.DynamicProperties;
			return retVal;
		}


	}
}
