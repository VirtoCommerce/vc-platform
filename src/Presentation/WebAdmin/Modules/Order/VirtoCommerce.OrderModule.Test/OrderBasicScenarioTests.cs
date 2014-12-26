using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.OrderModule.Services;
using System.Linq;
using VirtoCommerce.OrderModule.Data.RecalculationRules;
using VirtoCommerce.OrderModule.Model;

namespace VirtoCommerce.OrderModule.Tests
{
	[TestClass]
	public class OrderBasicScenarioTests
	{

		[TestMethod]
		public void OrderRecalculation()
		{
		}

		//they operation 
		[TestMethod]
		public void ProcessPaymentForOrder()
		{
			IOrderService orderService = null;
			var order = orderService.GetById("");
			var allActivePayments = order.Payments.Where(x => x.IsActive);
			foreach(var activePayment in allActivePayments)
			{
				//var paymentGateway = paymentGatewayFactory.GetGateway(activePayment.PaymentGatewayCode);
				//var transactionInfo = paymentGateway.GetTransactionInfo(activePayment.OuterId);
				//activePayment.Status = transactionInfo.Status;
				//activePayment.AmountCollected = transationInfo.AmountCollected;

			}
			var calcTotals = new CalcTotalsRule();
			calcTotals.Recalculate(order);
			orderService.Update(new Order[] { order });
		}

		//
		[TestMethod]
		public void CancelWholeOrderItem()
		{
			IOrderService orderService = null;
			var order = orderService.GetById("");
			order.CanceledDate = DateTime.UtcNow;

			//TODO: Return money manually
			orderService.Update(new Order[] { order });
		}

		[TestMethod]
		public void DescreaseOrderItem()
		{
			IOrderService orderService = null;
			var order = orderService.GetById("");
			var orderItem = order.OrderItems.Where(x=>x.Quantity > 1).First();
			orderItem.Quantity = 1;
			//TODO: Return money manually
			orderService.Update(new Order[] { order });
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
