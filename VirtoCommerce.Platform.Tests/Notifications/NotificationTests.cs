using System.Collections.Generic;
using Moq;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Notifications;
using VirtoCommerce.Platform.Tests.Notifications.Models;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Notifications
{
    public class NotificationTests
    {
        [Fact]
        public void ResolveOrderNotificationTest()
        {
            var gw = new Mock<INotificationSendingGateway>();
            var notification = new OrderNotification(gw.Object);

            notification.CustomerOrder = new CustomerOrder
            {
                Addresses = new List<Address>
                {
                    new Address
                    {
                        City = "City",
                        FirstName = "First Name",
                        LastName = "Last Name",
                        CountryName = "Country"
                    }
                }
            };

            var notificationTemplateResolver = new LiquidNotificationTemplateResolver();
            notificationTemplateResolver.ResolveTemplate(notification);

            Assert.Contains("First Name", notification.Body);
            Assert.Contains("Last Name", notification.Body);
        }

        [Fact]
        public void ResolveOrderInvoiceNotificationTest()
        {
            var gw = new Mock<INotificationSendingGateway>();
            var notification = new OrderInvoiceNotification(gw.Object);

            notification.CustomerOrder = new CustomerOrder
            {
                Items = new List<LineItem>
                {
                    new LineItem
                    {
                        Name = "Test product",
                        Sku = "0000011",
                        PlacedPrice = 500,
                        Quantity = 1
                    }
                }
            };

            var notificationTemplateResolver = new LiquidNotificationTemplateResolver();
            notificationTemplateResolver.ResolveTemplate(notification);

            Assert.Contains("Test product", notification.Body);
            Assert.Contains("0000011", notification.Body);
        }

        [Fact]
        public void ResolveAddressNotificationTest()
        {
            var gw = new Mock<INotificationSendingGateway>();
            var notification = new AddressNotification(gw.Object);

            notification.Item = new LineItem
            {
                Name = "Test product",
                Sku = "0000011",
                PlacedPrice = 500,
                Quantity = 1
            };
            notification.Address = new Address
            {
                City = "City",
                FirstName = "First Name",
                LastName = "Last Name",
                CountryName = "Country"
            };

            var notificationTemplateResolver = new LiquidNotificationTemplateResolver();
            notificationTemplateResolver.ResolveTemplate(notification);

            Assert.Contains("Test product", notification.Body);
            Assert.Contains("0000011", notification.Body);
        }
    }
}