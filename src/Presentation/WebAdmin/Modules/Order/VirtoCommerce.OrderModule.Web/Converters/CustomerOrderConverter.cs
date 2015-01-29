using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.OrderModule.Web.Converters
{
	public static class CustomerOrderConverter
	{
		public static webModel.CustomerOrder ToWebModel(this coreModel.CustomerOrder customerOrder)
		{
			var retVal = new webModel.CustomerOrder();
			retVal.Childrens = new List<webModel.OperationTreeNode>();
			retVal.InjectFrom(customerOrder);

			retVal.CustomerId =  customerOrder.TargetAgentId;
			retVal.SiteId = customerOrder.SourceStoreId;
			retVal.OrganizationId = customerOrder.TargetStoreId;
			retVal.EmployeeId = customerOrder.SourceAgentId;

			//TODO: resolve id to names
			retVal.Customer = retVal.CustomerId;
			retVal.Site = retVal.SiteId;
			retVal.OrganizationId = retVal.OrganizationId;
			retVal.EmployeeId = retVal.EmployeeId;

			if (customerOrder.Shipments != null)
			{
				retVal.Shipments = customerOrder.Shipments.Select(x => x.ToWebModel()).ToList();
				retVal.Childrens.AddRange(retVal.Shipments);
			}
			if (customerOrder.InPayments != null)
			{
				retVal.InPayments = customerOrder.InPayments.Select(x => x.ToWebModel()).ToList();
				retVal.Childrens.AddRange(retVal.InPayments);
			}
			if (customerOrder.Items != null)
			{
				retVal.Items = customerOrder.Items.Select(x => x.ToWebModel()).ToList();
			}
			if (customerOrder.Addresses != null)
			{
				retVal.Addresses = customerOrder.Addresses.Select(x => x.ToWebModel()).ToList();
			}
		
		
			return retVal;
		}

		public static coreModel.CustomerOrder ToCoreModel(this webModel.CustomerOrder customerOrder)
		{
			var retVal = new coreModel.CustomerOrder();
			retVal.InjectFrom(customerOrder);
			retVal.Currency = customerOrder.Currency;

			
			retVal.TargetAgentId = customerOrder.CustomerId;
			retVal.SourceStoreId = customerOrder.SiteId;
			retVal.TargetStoreId = customerOrder.OrganizationId;
			retVal.SourceAgentId = customerOrder.EmployeeId;

			if (customerOrder.Items != null)
				retVal.Items = customerOrder.Items.Select(x => x.ToCoreModel()).ToList();
			if (customerOrder.Addresses != null)
				retVal.Addresses = customerOrder.Addresses.Select(x => x.ToCoreModel()).ToList();
			if (customerOrder.Shipments != null)
				retVal.Shipments = customerOrder.Shipments.Select(x => x.ToCoreModel()).ToList();
			if (customerOrder.InPayments != null)
				retVal.InPayments = customerOrder.InPayments.Select(x => x.ToCoreModel()).ToList();

			if (customerOrder.Discount != null)
				retVal.Discount = customerOrder.Discount.ToCoreModel();

			return retVal;
		}


	}
}