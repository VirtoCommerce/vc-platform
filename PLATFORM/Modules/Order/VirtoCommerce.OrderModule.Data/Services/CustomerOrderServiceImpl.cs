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
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Workflow.Services;
using VirtoCommerce.Foundation.Data.Infrastructure;

namespace VirtoCommerce.OrderModule.Data.Services
{
    public class CustomerOrderServiceImpl : ServiceBase, ICustomerOrderService
    {
        private const string _workflowName = "OrderRecalculate";
        private readonly Func<IOrderRepository> _repositoryFactory;
        private readonly IOperationNumberGenerator _operationNumberGenerator;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IWorkflowService _workflowService;
        public CustomerOrderServiceImpl(Func<IOrderRepository> orderRepositoryFactory, IOperationNumberGenerator operationNumberGenerator, IWorkflowService workflowService)
        {
            _repositoryFactory = orderRepositoryFactory;
            //_shoppingCartService = shoppingCartService;
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
            RecalculateOrder(order);

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
            var retVal = new CustomerOrder
            {
                Currency = shoppingCart.Currency,
                CustomerId = shoppingCart.CustomerId,
                StoreId = shoppingCart.StoreId,
                OrganizationId = shoppingCart.OrganizationId
            };

            retVal.Items = new List<LineItem>();
            foreach (var cartItem in shoppingCart.Items)
            {
                var orderItem = new LineItem
                {
                    Name = cartItem.Name,
                    BasePrice = cartItem.ListPrice,
                    CatalogId = cartItem.CatalogId,
                    CategoryId = cartItem.CategoryId,
                    ProductId = cartItem.ProductId,
                    Price = cartItem.PlacedPrice,
                    ShippingMethodCode = cartItem.ShipmentMethodCode,
                    Tax = cartItem.TaxTotal,
                    IsGift = cartItem.IsGift,
                    Quantity = cartItem.Quantity,
                    FulfilmentLocationCode = cartItem.FulfilmentLocationCode
                };
                retVal.Items.Add(orderItem);
            }
            //TODO: split shipment if it not exist
            retVal.Shipments = new List<Shipment>();
            foreach (var cartShipment in shoppingCart.Shipments)
            {
                var shipment = new Shipment
                {
                    Currency = cartShipment.Currency,
                    Sum = cartShipment.ShippingPrice,

                    DeliveryAddress = new Address
                    {
                        AddressType = Domain.Order.Model.AddressType.Shipping,
                        City = cartShipment.DeliveryAddress.City,
                        Phone = cartShipment.DeliveryAddress.Phone,
                        PostalCode = cartShipment.DeliveryAddress.PostalCode,
                        CountryCode = cartShipment.DeliveryAddress.CountryCode,
                        Email = cartShipment.DeliveryAddress.Email,
                        FirstName = cartShipment.DeliveryAddress.FirstName,
                        LastName = cartShipment.DeliveryAddress.LastName,
                        Line1 = cartShipment.DeliveryAddress.Line1,
                        Organization = cartShipment.DeliveryAddress.Organization
                    }
                };
                retVal.Shipments.Add(shipment);
            }

            retVal = Create(retVal);

            //Clear shopping cart
            _shoppingCartService.Delete(new string[] { cartId });


            return retVal;
        }

        public void Update(CustomerOrder[] orders)
        {
            var changedOrders = new List<CustomerOrder>();
            // incoming order may contains only changed fields and we need load whole order and apply changes to it

            foreach (var order in orders)
            {
                //Apply changes to temporary order object
                var targetOrder = GetById(order.Id, CustomerOrderResponseGroup.Full);
                if (targetOrder == null)
                {
                    throw new NullReferenceException("targetOrder");
                }
                var sourceOrderEntity = order.ToEntity();
                var targetOrderEntity = targetOrder.ToEntity();
                sourceOrderEntity.Patch(targetOrderEntity);
                var changedOrder = targetOrderEntity.ToCoreModel();
                changedOrders.Add(changedOrder);
            }


            //Need a call business logic for changes and persist changes
            using (var repository = _repositoryFactory())
            using (var changeTracker = base.GetChangeTracker(repository))
            {
                foreach (var changedOrder in changedOrders)
                {
                    //Do business logic on temporary  order object
                    RecalculateOrder(changedOrder);

                    EnsureThatAllOperationsHasNumber(changedOrder);


                    var sourceOrderEntity = changedOrder.ToEntity();
                    var targetOrderEntity = repository.GetCustomerOrderById(changedOrder.Id, CustomerOrderResponseGroup.Full);
                    if (targetOrderEntity == null)
                    {
                        throw new NullReferenceException("targetOrderEntity");
                    }

                    changeTracker.Attach(targetOrderEntity);
                    sourceOrderEntity.Patch(targetOrderEntity);
                }
                CommitChanges(repository);
            }

        }

        public void Delete(string[] oderIds)
        {
            throw new NotImplementedException();
        }
        #endregion


        private void RecalculateOrder(CustomerOrder order)
        {
            var parameters = new Dictionary<string, object>();
            parameters["order"] = order;
            _workflowService.RunWorkflow(_workflowName, parameters, null);
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
