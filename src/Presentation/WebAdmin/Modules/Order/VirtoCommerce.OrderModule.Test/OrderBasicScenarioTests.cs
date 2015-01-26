using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;
namespace VirtoCommerce.OrderModule.Tests
{
	[TestClass]
	public class OrderBasicScenarioTests
	{

		//they operation 
		[TestMethod]
		public void ProcessPaymentForOrder()
		{
			ICustomerOrderService customerOrderService = null;
			var order = customerOrderService.GetById("");
			var notApprovedPayments = order.InPayments.Where(x => !x.IsApproved);
			foreach (var inPayment in notApprovedPayments)
			{
				//var paymentGateway = paymentGatewayFactory.GetGateway(inPayment.PaymentGatewayCode);
				//var transactionInfo = paymentGateway.GetTransactionInfo(activePayment.IncomingNumber);
				inPayment.IsApproved = true;
			}

			customerOrderService.Update(new CustomerOrder[] { order });
		}

		//
		[TestMethod]
		public void CancelWholeOrderItem()
		{
			ICustomerOrderService operationService = null;

			var order = operationService.GetById("") as CustomerOrder;
		
			//TODO: get a status from metadata only for customer business process
			order.StatusId = "canceled";
		
			//if order has approved payment need create payment out
			if(order.InPayments.Any(x=>x.IsApproved))
			{
				//Create refund operation with comming out payment
				var paymentOut = new PaymentOut
				{
					 SourceAgentId = order.TargetAgentId
				};
				foreach(var paymentIn in order.InPayments)
				{
					paymentOut.Sum += paymentIn.Sum;
				}
				order.OutPayments.Add(paymentOut);
			}

			operationService.Update(new CustomerOrder[] { order });
		}

		[TestMethod]
		public void DescreaseOrderItem()
		{
			ICustomerOrderService operationService = null;
			var order = operationService.GetById("") as CustomerOrder;
			var orderItem = order.Items.Where(x => x.Quantity > 1).First();
			orderItem.Quantity = 1;
			//TODO: Return money manually
			operationService.Update(new CustomerOrder[] { order });
		}

		[TestMethod]
		public void AddNewOrderItem()
		{
		}

		[TestMethod]
		public void ApplyCoupon()
		{
		}

		[TestMethod]
		public void FulfilOrderWithSingleShipment()
		{
			ICustomerOrderService customerOrderService = null;
			var order = customerOrderService.GetById("");
			//Check that inventory changed
			var shipment = new Shipment
			{
				IsApproved = true
			};
			shipment.Items.AddRange(order.Items);
			order.Shipments.Add(shipment);

			customerOrderService.Update(new CustomerOrder[] { order });
		}

		[TestMethod]
		public void FulfilOrderWithMultipleShipment()
		{
		}

		[TestMethod]
		public void FulfilOrderPartialy()
		{
		}

	}
}
