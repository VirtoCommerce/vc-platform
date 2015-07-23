using System;
using System.Collections.ObjectModel;
using System.Linq;
using Klarna.Checkout.Euro.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.CartModule.Data.Services;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.Domain.Cart.Events;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Common.Events;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Services;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.DynamicProperties;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Repositories;

namespace PaymentMethods.Tests
{
    [TestClass]
    public class KlarnaCheckoutEuroTests
    {
        [TestMethod]
        public void CapturePayment()
        {
            var service = GetCustomerOrderService();
            var order = service.GetById("9ae2ccce-008d-42bd-952b-9dbd9ac88e6a", CustomerOrderResponseGroup.Full);
            var method = GetMethod();

            var context = new CaptureProcessPaymentEvaluationContext
            {
                Payment = order.InPayments.First()
            };

            var result = method.CaptureProcessPayment(context);

            service = GetCustomerOrderService();
            service.Update(new CustomerOrder[] { order });

            service = GetCustomerOrderService();
            order = service.GetById("9ae2ccce-008d-42bd-952b-9dbd9ac88e6a", CustomerOrderResponseGroup.Full);

            Assert.AreEqual(PaymentStatus.Paid, order.InPayments.First().PaymentStatus);
            Assert.IsTrue(order.InPayments.First().IsApproved);
            Assert.IsNotNull(order.InPayments.First().CapturedDate);
        }

        [TestMethod]
        public void VoidPayment()
        {
            var service = GetCustomerOrderService();
            var order = service.GetById("161a7df0-a90f-4beb-a23a-9b043f3e5bcb", CustomerOrderResponseGroup.Full);
            var method = GetMethod();

            var context = new VoidProcessPaymentEvaluationContext
            {
                Payment = order.InPayments.First()
            };

            var result = method.VoidProcessPayment(context);

            service = GetCustomerOrderService();
            service.Update(new CustomerOrder[] { order });

            service = GetCustomerOrderService();
            order = service.GetById("161a7df0-a90f-4beb-a23a-9b043f3e5bcb", CustomerOrderResponseGroup.Full);

            Assert.AreEqual(PaymentStatus.Voided, order.InPayments.First().PaymentStatus);
            Assert.IsTrue(!order.InPayments.First().IsApproved);
            Assert.IsNotNull(order.InPayments.First().VoidedDate);
        }

        [TestMethod]
        public void RefundPayment()
        {

        }

        private CustomerOrderServiceImpl GetCustomerOrderService()
        {
            Func<IPlatformRepository> platformRepositoryFactory = () => new PlatformRepository("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor());
            Func<IOrderRepository> orderRepositoryFactory = () =>
            {
                return new OrderRepositoryImpl("VirtoCommerce", new AuditableInterceptor(), new EntityPrimaryKeyGeneratorInterceptor());
            };

            Func<ICartRepository> repositoryFactory = () =>
            {
                return new CartRepositoryImpl("VirtoCommerce", new AuditableInterceptor());
            };

            var orderEventPublisher = new EventPublisher<OrderChangeEvent>(Enumerable.Empty<IObserver<OrderChangeEvent>>().ToArray());
            var cartEventPublisher = new EventPublisher<CartChangeEvent>(Enumerable.Empty<IObserver<CartChangeEvent>>().ToArray());
            var cartService = new ShoppingCartServiceImpl(repositoryFactory, cartEventPublisher, null);
            var dynamicPropertyService = new DynamicPropertyService(platformRepositoryFactory);

            var orderService = new CustomerOrderServiceImpl(orderRepositoryFactory, new TimeBasedNumberGeneratorImpl(), orderEventPublisher, cartService, GetItemService(), dynamicPropertyService);
            return orderService;
        }

        private KlarnaCheckoutEuroPaymentMethod GetMethod()
        {
            var settings = new Collection<SettingEntry>();
            settings.Add(new SettingEntry
            {
                Name = "Klarna.Checkout.Euro.AppKey",
                ValueType = SettingValueType.Integer,
                Value = "3486"
            });
            settings.Add(new SettingEntry
            {
                Name = "Klarna.Checkout.Euro.SecretKey",
                Value = "EodLR8tBViEpwLo"
            });
            settings.Add(new SettingEntry
            {
                Name = "Klarna.Checkout.Euro.Mode",
                Value = "test"
            });
            settings.Add(new SettingEntry
            {
                Name = "Klarna.Checkout.Euro.TermsUrl",
                Value = "checkout/terms"
            });
            settings.Add(new SettingEntry
            {
                Name = "Klarna.Checkout.Euro.CheckoutUrl",
                Value = "checkout/step1"
            });
            settings.Add(new SettingEntry
            {
                Name = "Klarna.Checkout.Euro.ConfirmationUrl",
                Value = "checkout/externalpaymentcallback"
            });
            settings.Add(new SettingEntry
            {
                Name = "Klarna.Checkout.Euro.PaymentActionType",
                Value = "Authorization/Capture"
            });

            var retVal = new KlarnaCheckoutEuroPaymentMethod
            {
                Settings = settings
            };

            return retVal;
        }

        private IItemService GetItemService()
        {
            return new ItemServiceImpl(() => { return GetRepository(); }, null);
        }

        private ICatalogRepository GetRepository()
        {
            var retVal = new CatalogRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor());
            return retVal;
        }
    }
}
