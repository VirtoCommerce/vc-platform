using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.OrderModule.Data.Converters;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Model;
using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Common.Events;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Domain.Catalog.Services;

namespace VirtoCommerce.OrderModule.Data.Services
{
	public class CustomerOrderServiceImpl : ServiceBase, ICustomerOrderService
	{
		private readonly Func<IOrderRepository> _repositoryFactory;
		private readonly IOperationNumberGenerator _operationNumberGenerator;
		private readonly IShoppingCartService _shoppingCartService;
		private readonly IItemService _productService;
		private readonly IEventPublisher<OrderChangeEvent> _eventPublisher;

		public CustomerOrderServiceImpl(Func<IOrderRepository> orderRepositoryFactory, IOperationNumberGenerator operationNumberGenerator, IEventPublisher<OrderChangeEvent> eventPublisher, IShoppingCartService shoppingCartService, IItemService productService)
		{
			_repositoryFactory = orderRepositoryFactory;
			_shoppingCartService = shoppingCartService;
			_operationNumberGenerator = operationNumberGenerator;
			_eventPublisher = eventPublisher;
			_productService = productService;
		}

		#region ICustomerOrderService Members

		public virtual CustomerOrder GetById(string orderId, CustomerOrderResponseGroup respGroup = CustomerOrderResponseGroup.Full)
		{
			CustomerOrder retVal = null;
			using (var repository = _repositoryFactory())
			{
				var orderEntity = repository.GetCustomerOrderById(orderId, respGroup);
				if (orderEntity != null)
				{
					retVal = orderEntity.ToCoreModel();
				}
				if((respGroup & CustomerOrderResponseGroup.WithProducts) == CustomerOrderResponseGroup.WithProducts)
				{
					var productIds = retVal.Items.Select(x => x.ProductId).ToArray();
					var products = _productService.GetByIds(productIds, Domain.Catalog.Model.ItemResponseGroup.ItemInfo);
					foreach(var lineItem in retVal.Items)
					{
						var product = products.FirstOrDefault(x => x.Id == lineItem.ProductId);
						if(product != null)
						{
							lineItem.Product = product;
						}
					}
				}
			}
			_eventPublisher.Publish(new OrderChangeEvent(EntryState.Unchanged, null, retVal));
			return retVal;
		}

		public virtual CustomerOrder Create(CustomerOrder order)
		{
			EnsureThatAllOperationsHasNumber(order);

			_eventPublisher.Publish(new OrderChangeEvent(EntryState.Added, null, order));

			var orderEntity = order.ToDataModel();
			

			CustomerOrder retVal = null;
			using (var repository = _repositoryFactory())
			{
				repository.Add(orderEntity);
				CommitChanges(repository);
			}
			retVal = GetById(orderEntity.Id);
			return retVal;

		}

		public virtual CustomerOrder CreateByShoppingCart(string cartId)
		{
			var shoppingCart = _shoppingCartService.GetById(cartId);

			if (shoppingCart == null)
			{
				throw new OperationCanceledException("cart not found");
			}
			var customerOrder = shoppingCart.ToCustomerOrder();
			var retVal = Create(customerOrder);

			return retVal;
		}

		public void Update(CustomerOrder[] orders)
		{
			using (var repository = _repositoryFactory())
			{
				foreach (var order in orders)
				{
					EnsureThatAllOperationsHasNumber(order);
					var origOrder = GetById(order.Id, CustomerOrderResponseGroup.Full);

					//Do business logic on temporary  order object
					_eventPublisher.Publish(new OrderChangeEvent(EntryState.Modified, origOrder, order));

					var sourceOrderEntity = order.ToDataModel();
					var targetOrderEntity = repository.GetCustomerOrderById(order.Id, CustomerOrderResponseGroup.Full);

					if (targetOrderEntity == null)
					{
						throw new NullReferenceException("targetOrderEntity");
					}

					using (var changeTracker = GetChangeTracker(repository))
					{
						changeTracker.Attach(targetOrderEntity);
						sourceOrderEntity.Patch(targetOrderEntity);
					}
				}
				CommitChanges(repository);
			}

		}

		public void Delete(string[] oderIds)
		{
			throw new NotImplementedException();
		}
		#endregion

		private void EnsureThatAllOperationsHasNumber(CustomerOrder order)
		{
			foreach (var operation in order.Traverse<Operation>(x => x.ChildrenOperations))
			{
				if (operation.Number == null)
				{
					operation.Number = _operationNumberGenerator.GenerateNumber(operation);
				}
			}

		}
	}
}
