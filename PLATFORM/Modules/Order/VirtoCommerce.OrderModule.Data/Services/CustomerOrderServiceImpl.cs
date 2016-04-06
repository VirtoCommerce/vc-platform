using System;
using System.Linq;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Domain.Shipping.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.OrderModule.Data.Converters;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.OrderModule.Data.Services
{
    public class CustomerOrderServiceImpl : ServiceBase, ICustomerOrderService
    {
        private readonly Func<IOrderRepository> _repositoryFactory;
        private readonly IUniqueNumberGenerator _uniqueNumberGenerator;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IItemService _productService;
        private readonly IEventPublisher<OrderChangeEvent> _eventPublisher;
        private readonly IDynamicPropertyService _dynamicPropertyService;
        private readonly IShippingMethodsService _shippingMethodsService;
        private readonly IPaymentMethodsService _paymentMethodsService;
        private readonly IStoreService _storeService;

        public CustomerOrderServiceImpl(Func<IOrderRepository> orderRepositoryFactory, IUniqueNumberGenerator uniqueNumberGenerator, IEventPublisher<OrderChangeEvent> eventPublisher, IShoppingCartService shoppingCartService, IItemService productService, 
                                       IDynamicPropertyService dynamicPropertyService, IShippingMethodsService shippingMethodsService, IPaymentMethodsService paymentMethodsService,
                                       IStoreService storeService)
        {
            _repositoryFactory = orderRepositoryFactory;
            _shoppingCartService = shoppingCartService;
            _uniqueNumberGenerator = uniqueNumberGenerator;
            _eventPublisher = eventPublisher;
            _productService = productService;
            _dynamicPropertyService = dynamicPropertyService;
            _shippingMethodsService = shippingMethodsService;
            _paymentMethodsService = paymentMethodsService;
            _storeService = storeService;
        }

        #region ICustomerOrderService Members

        public virtual CustomerOrder GetById(string orderId, CustomerOrderResponseGroup respGroup)
        {
            CustomerOrder retVal = null;
            using (var repository = _repositoryFactory())
            {
                var orderEntity = repository.GetCustomerOrderById(orderId, respGroup);
                if (orderEntity != null)
                {
                    retVal = orderEntity.ToCoreModel(_shippingMethodsService.GetAllShippingMethods(), _paymentMethodsService.GetAllPaymentMethods());

                    if ((respGroup & CustomerOrderResponseGroup.WithProducts) == CustomerOrderResponseGroup.WithProducts)
                    {
                        var productIds = retVal.Items.Select(x => x.ProductId).ToArray();
                        var products = _productService.GetByIds(productIds, Domain.Catalog.Model.ItemResponseGroup.ItemInfo);
                        foreach (var lineItem in retVal.Items)
                        {
                            var product = products.FirstOrDefault(x => x.Id == lineItem.ProductId);
                            if (product != null)
                            {
                                lineItem.Product = product;
                            }
                        }
                    }
                }
            }

            _dynamicPropertyService.LoadDynamicPropertyValues(retVal);

            _eventPublisher.Publish(new OrderChangeEvent(EntryState.Unchanged, retVal, retVal));
            return retVal;
        }

        public virtual CustomerOrder GetByOrderNumber(string orderNumber, CustomerOrderResponseGroup respGroup = CustomerOrderResponseGroup.Full)
        {
            CustomerOrder retVal = null;
            using (var repository = _repositoryFactory())
            {
                var orderEntity = repository.GetCustomerOrderByNumber(orderNumber, CustomerOrderResponseGroup.WithAddresses);
                if (orderEntity != null)
                {
                    retVal = GetById(orderEntity.Id, respGroup);
                }
            }
            return retVal;
        }

        public virtual CustomerOrder Create(CustomerOrder order)
        {
            EnsureThatAllOperationsHaveNumber(order);

            _eventPublisher.Publish(new OrderChangeEvent(EntryState.Added, order, order));
            var pkMap = new PrimaryKeyResolvingMap(); 
            var entity = order.ToDataModel(pkMap);

            using (var repository = _repositoryFactory())
            {
                repository.Add(entity);
                CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();
            }

            _dynamicPropertyService.SaveDynamicPropertyValues(order);

            var retVal = GetById(entity.Id, CustomerOrderResponseGroup.Full);
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
            _dynamicPropertyService.LoadDynamicPropertyValues(customerOrder);

            //Copy properties values with same name  from cart lineItems to order lineItems
            foreach (var cartItem in shoppingCart.Items)
            {
                var orderItem = customerOrder.Items.FirstOrDefault(x => x.ProductId == cartItem.ProductId && x.Quantity == cartItem.Quantity);
                if(orderItem != null)
                {
                    orderItem.CopyPropertyValuesFrom(cartItem);
                }
            }
         
            var retVal = Create(customerOrder);
   
            return retVal;
        }

        public void Update(CustomerOrder[] orders)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _repositoryFactory())
            {
                foreach (var order in orders)
                {
                    EnsureThatAllOperationsHaveNumber(order);
                    var origOrder = GetById(order.Id, CustomerOrderResponseGroup.Full);

                    // Do business logic on temporary order object
                    _eventPublisher.Publish(new OrderChangeEvent(EntryState.Modified, origOrder, order));

                    var sourceOrderEntity = order.ToDataModel(pkMap);
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
                pkMap.ResolvePrimaryKeys();
            }

            //Save dynamic properties
            foreach (var order in orders)
            {
                _dynamicPropertyService.SaveDynamicPropertyValues(order);
            }
        }

        public void Delete(string[] orderIds)
        {
            using (var repository = _repositoryFactory())
            {

                foreach (var orderId in orderIds)
                {
                    var dbOrder = repository.GetCustomerOrderById(orderId, CustomerOrderResponseGroup.Full);
                    if (dbOrder != null)
                    {
                        var order = GetById(orderId, CustomerOrderResponseGroup.Full);

                        _eventPublisher.Publish(new OrderChangeEvent(Platform.Core.Common.EntryState.Deleted, order, order));

                        _dynamicPropertyService.DeleteDynamicPropertyValues(order);
                        repository.Remove(dbOrder);
                    }
                }
                repository.UnitOfWork.Commit();
            }
        }
        #endregion

        private void EnsureThatAllOperationsHaveNumber(CustomerOrder order)
        {
            var store = _storeService.GetById(order.StoreId);
            foreach (var operation in order.GetFlatObjectsListWithInterface<IOperation>())
            {
                if (operation.Number == null)
                {
                    var objectTypeName = operation.GetType().Name;
                    // take uppercase chars to form operation type, or just take 2 first chars. (CustomerOrder => CO, PaymentIn => PI, Shipment => SH)
                    var objectType = string.Concat(objectTypeName.Select(c => char.IsUpper(c) ? c.ToString() : ""));
                    if (objectType.Length < 2)
                    {
                        objectType = objectTypeName.Substring(0, 2).ToUpper();
                    }
                    var numberTemplate = store.Settings.GetSettingValue("Order." + objectTypeName + "NewNumberTemplate", objectType + "{0:yyMMdd}-{1:D5}");
                    operation.Number = _uniqueNumberGenerator.GenerateNumber(numberTemplate);
                }
            }

        }
    }
}
