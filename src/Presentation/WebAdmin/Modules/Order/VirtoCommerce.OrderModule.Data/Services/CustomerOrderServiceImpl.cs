using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Repositories;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.OrderModule.Data.Converters;

namespace VirtoCommerce.OrderModule.Data.Services
{
	public class CustomerOrderServiceImpl : ICustomerOrderService
	{
		private readonly IInventoryService _inventoryService;
		private readonly IOrderRepository _repository;
		private readonly IOperationNumberGenerator _operationNumberGenerator;
		private readonly IShoppingCartService _shoppingCartService;
		public CustomerOrderServiceImpl(IOrderRepository orderRepository, IInventoryService inventoryService, 
									    IShoppingCartService shoppingCartService, IOperationNumberGenerator operationNumberGenerator)
		{
			_inventoryService = inventoryService;
			_repository = orderRepository;
			_shoppingCartService = shoppingCartService;
			_operationNumberGenerator = operationNumberGenerator;
		}

		#region ICustomerOrderService Members

		public virtual CustomerOrder GetById(string orderId, ResponseGroup respGroup = ResponseGroup.WithItems)
		{
			return _repository.GetCustomerOrderById(orderId, respGroup);
		}

		public virtual CustomerOrder Create(CustomerOrder order)
		{
			order.CalculateTotals();
			if(order.Number == null)
			{
				order.Number = _operationNumberGenerator.GenerateNumber(order);
			}
			//TODO: for approved sipments need decrease inventory
			_repository.Add(order);

			_repository.UnitOfWork.Commit();
			return order;
		}

		public virtual CustomerOrder CreateByShoppingCart(string cartId)
		{
			var shoppingCart = _shoppingCartService.GetById(cartId);
			var customerOrder = new CustomerOrder
			{
				Currency = shoppingCart.Currency,
				TargetAgentId = shoppingCart.CustomerId,
				SourceStoreId = shoppingCart.SiteId,
			};

			customerOrder.Number = _operationNumberGenerator.GenerateNumber(customerOrder);
			foreach (var cartItem in shoppingCart.Items)
			{
				var orderItem = new CustomerOrderItem
				{
					Name = cartItem.Name,
					BasePrice = cartItem.ListPrice,
					CatalogId = cartItem.CatalogId,
					CategoryId = cartItem.CategoryId,
					ProductId = cartItem.ProductId,
					Price = cartItem.PlacedPrice,
					ShippingMethodCode = cartItem.ShipmentMethodCode,
					DisplayName = cartItem.Name,
					Tax = cartItem.TaxTotal,
					IsGift = cartItem.IsGift,
					Quantity = cartItem.Quantity,
					FulfilmentLocationCode = cartItem.FulfilmentLocationCode
				};
				customerOrder.Items.Add(orderItem);
			}
			//TODO: split shipment
			
			//Clear shopping cart
			_shoppingCartService.Delete(new string[] { cartId });

			_repository.Add(customerOrder);
			_repository.UnitOfWork.Commit();
			return customerOrder;
		}

		public void Update(CustomerOrder[] orders)
		{
			foreach (var sourceOrder in orders)
			{
				var targetOrder = GetById(sourceOrder.Id);
				//TODO: for approved sipments need decrease inventory
				sourceOrder.Patch(targetOrder);

				targetOrder.CalculateTotals();

				_repository.UnitOfWork.Commit();
			}
		}

		public void Delete(string[] cartIds)
		{
			throw new NotImplementedException();
		}

		

		#endregion
	}
}
