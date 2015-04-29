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
using VirtoCommerce.OrderModule.Data.Workflow;
using VirtoCommerce.OrderModule.Data.Model;
using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Data.Services
{
	public class CustomerOrderServiceImpl : ServiceBase, ICustomerOrderService
	{
		private const string _workflowName = "OrderRecalculate";
		private readonly Func<IOrderRepository> _repositoryFactory;
		private readonly IOperationNumberGenerator _operationNumberGenerator;
		private readonly IShoppingCartService _shoppingCartService;
		private readonly IWorkflowService _workflowService;
		public CustomerOrderServiceImpl(Func<IOrderRepository> orderRepositoryFactory, IOperationNumberGenerator operationNumberGenerator, IWorkflowService workflowService, IShoppingCartService shoppingCartService)
		{
			_repositoryFactory = orderRepositoryFactory;
			_shoppingCartService = shoppingCartService;
			_operationNumberGenerator = operationNumberGenerator;
			_workflowService = workflowService;
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
			}
			return retVal;
		}

		public virtual CustomerOrder Create(CustomerOrder order)
		{
			RecalculateOrder(new CustomerOrderStateBasedEvalContext(EntryState.Added, null, order));

			EnsureThatAllOperationsHasNumber(order);

			//TODO: for approved sipments need decrease inventory

			var orderEntity = order.ToEntity();
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

			//Need a call business logic for changes and persist changes
			using (var repository = _repositoryFactory())
			{
				foreach (var order in orders)
				{
					EnsureThatAllOperationsHasNumber(order);
					var origOrder = GetById(order.Id, CustomerOrderResponseGroup.Full);

					//Do business logic on temporary  order object
					RecalculateOrder(new CustomerOrderStateBasedEvalContext(EntryState.Modified, origOrder, order));

					var sourceOrderEntity = order.ToEntity();
					var targetOrderEntity = repository.GetCustomerOrderById(order.Id, CustomerOrderResponseGroup.Full);

					if (targetOrderEntity == null)
					{
						throw new NullReferenceException("targetOrderEntity");
					}

					using (var changeTracker = GetChangeTracker(repository, targetOrderEntity))
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


		private ObservableChangeTracker GetChangeTracker(IRepository repository, CustomerOrderEntity customerOrderEntity)
		{
			var retVal = new ObservableChangeTracker
			{
				RemoveAction = (x) =>
				{
					repository.Remove(x);
				},
				AddAction = (x) =>
				{
					var address = x as AddressEntity;
					var shipment = x as ShipmentEntity;
					var lineItem = x as LineItemEntity;

					if (address != null)
					{
						address.CustomerOrder = customerOrderEntity;
					}
					if (shipment != null)
					{
						foreach (var shipmentItem in shipment.Items)
						{
							var orderLineItem = customerOrderEntity.Items.FirstOrDefault(item => item.Id == shipmentItem.Id);
							if (orderLineItem != null)
							{
								orderLineItem.Shipment = shipment;
							}
							else
							{
								shipmentItem.CustomerOrder = customerOrderEntity;
							}
						}
						shipment.Items = new NullCollection<LineItemEntity>();
					}
					if (lineItem != null)
					{
						lineItem.CustomerOrder = customerOrderEntity;
						lineItem.CustomerOrderId = customerOrderEntity.Id;
					}
					repository.Add(x);
				}
			};

			return retVal;
		}
		private void RecalculateOrder(CustomerOrderStateBasedEvalContext context)
		{
			_workflowService.RunWorkflow(context);
		}

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
