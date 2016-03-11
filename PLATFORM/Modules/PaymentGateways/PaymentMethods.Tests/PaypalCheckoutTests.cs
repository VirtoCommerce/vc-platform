using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paypal.ExpressCheckout.Managers;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.CartModule.Data.Services;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CoreModule.Data.Services;
using VirtoCommerce.Domain.Cart.Events;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Services;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.DynamicProperties;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Settings;

namespace PaymentMethods.Tests
{
    [TestClass]
    public class PaypalCheckoutTests
    {
        [TestMethod]
        public void CapturePayment()
        {
            var service = GetCustomerOrderService();
            var order = service.GetById("ec2b8124-f061-4997-baa1-ca55c1149a58", CustomerOrderResponseGroup.Full);
            var method = GetMethod();

            var context = new CaptureProcessPaymentEvaluationContext
            {
                Payment = order.InPayments.First()
            };

            var result = method.CaptureProcessPayment(context);

            service = GetCustomerOrderService();
            service.Update(new CustomerOrder[] { order });

            service = GetCustomerOrderService();
            order = service.GetById("ec2b8124-f061-4997-baa1-ca55c1149a58", CustomerOrderResponseGroup.Full);

            Assert.AreEqual(PaymentStatus.Paid, order.InPayments.First().PaymentStatus);
            Assert.IsTrue(order.InPayments.First().IsApproved);
            Assert.IsNotNull(order.InPayments.First().CapturedDate);
        }

        [TestMethod]
        public void VoidPayment()
        {
            var service = GetCustomerOrderService();
            var order = service.GetById("ec2b8124-f061-4997-baa1-ca55c1149a58", CustomerOrderResponseGroup.Full);
            var method = GetMethod();

            var context = new VoidProcessPaymentEvaluationContext
            {
                Payment = order.InPayments.First()
            };

            var result = method.VoidProcessPayment(context);

            service = GetCustomerOrderService();
            service.Update(new CustomerOrder[] { order });

            service = GetCustomerOrderService();
            order = service.GetById("ec2b8124-f061-4997-baa1-ca55c1149a58", CustomerOrderResponseGroup.Full);

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
            Func<IPlatformRepository> platformRepositoryFactory = () => new PlatformRepository("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor(null));
            Func<IOrderRepository> orderRepositoryFactory = () =>
            {
                return new OrderRepositoryImpl("VirtoCommerce", new AuditableInterceptor(null), new EntityPrimaryKeyGeneratorInterceptor());
            };

            Func<ICartRepository> repositoryFactory = () =>
            {
                return new CartRepositoryImpl("VirtoCommerce", new AuditableInterceptor(null));
            };

            var dynamicPropertyService = new DynamicPropertyService(platformRepositoryFactory);
            var orderEventPublisher = new EventPublisher<OrderChangeEvent>(Enumerable.Empty<IObserver<OrderChangeEvent>>().ToArray());
            var cartEventPublisher = new EventPublisher<CartChangeEvent>(Enumerable.Empty<IObserver<CartChangeEvent>>().ToArray());
            var cartService = new ShoppingCartServiceImpl(repositoryFactory, cartEventPublisher, null, dynamicPropertyService);
            var settingManager = new SettingsManager(null, null, null, null);

            var orderService = new CustomerOrderServiceImpl(orderRepositoryFactory, new TimeBasedNumberGeneratorImpl(), orderEventPublisher, cartService, GetItemService(), dynamicPropertyService, settingManager, null, null);
            return orderService;
        }

        private PaypalExpressCheckoutPaymentMethod GetMethod()
        {
            var settings = new Collection<SettingEntry>();
            settings.Add(new SettingEntry
            {
                Name = "Paypal.ExpressCheckout.Mode",
                Value = "Sandbox"
            });
            settings.Add(new SettingEntry
            {
                Name = "Paypal.ExpressCheckout.PaymentMode",
                Value = "PaypalAccount"
            });
            settings.Add(new SettingEntry
            {
                Name = "Paypal.ExpressCheckout.APIUsername",
                Value = "evgokhrimenko_api1.gmail.com"
            });
            settings.Add(new SettingEntry
            {
                Name = "Paypal.ExpressCheckout.APIPassword",
                Value = "XMDRC63XDNDQPXAZ"
            });
            settings.Add(new SettingEntry
            {
                Name = "Paypal.ExpressCheckout.APISignature",
                Value = "AiPC9BjkCyDFQXbSkoZcgqH3hpacAddFA7jQMnRzruCFYMSKx38TE0pt"
            });
            settings.Add(new SettingEntry
            {
                Name = "Paypal.ExpressCheckout.PaymentRedirectRelativePath",
                Value = "checkout/externalpaymentcallback"
            });
            settings.Add(new SettingEntry
            {
                Name = "Paypal.ExpressCheckout.PaymentActionType",
                Value = "AUTHORIZATION"
            });

            var paypalCheckoutPaymentMethod = new PaypalExpressCheckoutPaymentMethod
            {
                Settings = settings
            };

            return paypalCheckoutPaymentMethod;
        }

        private IItemService GetItemService()
        {
            return new ItemServiceImpl(() => { return GetRepository(); }, null);
        }

        private ICatalogRepository GetRepository()
        {
            var retVal = new CatalogRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor(null));
            return retVal;
        }
    }
}
