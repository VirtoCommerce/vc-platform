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
		public static webModel.CustomerOrderItem ToWebModel(this coreModel.CustomerOrderItem orderItem)
		{
			var retVal = new webModel.CustomerOrderItem();
			retVal.InjectFrom(orderItem);
		
			return retVal;
		}

		public static coreModel.CustomerOrderItem ToCoreModel(this webModel.CustomerOrderItem orderItem)
		{
			var retVal = new coreModel.CustomerOrderItem();
			retVal.InjectFrom(orderItem);
		
			if (retVal.IsTransient())
			{
				retVal.Id = Guid.NewGuid().ToString();
			}

			if(orderItem.Discount != null)
			{
				retVal.Discount = orderItem.Discount.ToCoreModel();
			}
			return retVal;
		}


	}
}
