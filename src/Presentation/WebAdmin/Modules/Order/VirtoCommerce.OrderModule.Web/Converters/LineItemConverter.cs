using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Money;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;

namespace VirtoCommerce.OrderModule.Web.Converters
{
	public static class CusgtomerOrderItemConverter
	{
		public static webModel.LineItem ToWebModel(this coreModel.LineItem orderItem)
		{
			var retVal = new webModel.LineItem();
			retVal.InjectFrom(orderItem);
		
			return retVal;
		}

		public static coreModel.LineItem ToCoreModel(this webModel.LineItem orderItem)
		{
			var retVal = new coreModel.LineItem();
			retVal.InjectFrom(orderItem);
			if(orderItem.Discount != null)
			{
				retVal.Discount = orderItem.Discount.ToCoreModel();
			}
			return retVal;
		}


	}
}
