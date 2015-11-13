using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.OrderModule.Data.Notifications;
using VirtoCommerce.Domain.Payment.Model;

namespace VirtoCommerce.OrderModule.Data.Observers
{
    public class OrderNotificationObserver : IObserver<OrderChangeEvent>
    {
        private readonly INotificationManager _notificationManager;
        private readonly IStoreService _storeService;
        private readonly IContactService _contactService;

        public OrderNotificationObserver(INotificationManager notificationManager, IStoreService storeService, IContactService contactService)
        {
            _notificationManager = notificationManager;
            _storeService = storeService;
            _contactService = contactService;
        }

        public void OnCompleted()
        {
            
        }

        public void OnError(Exception error)
        {
            
        }

        public void OnNext(OrderChangeEvent value)
        {
            //Collection of order notifications
            var notifications = new List<EmailNotification>();

            if(IsOrderCanceled(value))
            {
                var notification = _notificationManager.GetNewNotification<CancelOrderEmailNotification>(value.ModifiedOrder.StoreId, "Store", "en-US");
                notification.OrderNumber = value.ModifiedOrder.Number;
                notification.CancelationReason = value.ModifiedOrder.CancelReason;

                notifications.Add(notification);
            }

            if(value.ChangeState == EntryState.Added)
            {
                var notification = _notificationManager.GetNewNotification<OrderCreateEmailNotification>(value.ModifiedOrder.StoreId, "Store", "en-US");
                notification.OrderNumber = value.ModifiedOrder.Number;

                notifications.Add(notification);
            }

            if(IsNewStatus(value))
            {
                var notification = _notificationManager.GetNewNotification<NewOrderStatusEmailNotification>(value.ModifiedOrder.StoreId, "Store", "en-US");
                notification.OrderNumber = value.ModifiedOrder.Number;
                notification.NewStatus = value.ModifiedOrder.Status;
                notification.OldStatus = value.OrigOrder.Status;

                notifications.Add(notification);
            }

            if(IsOrderPaid(value))
            {
                var notification = _notificationManager.GetNewNotification<OrderPaidEmailNotification>(value.ModifiedOrder.StoreId, "Store", "en-US");
                notification.OrderNumber = value.ModifiedOrder.Number;
                notification.FullPrice = value.ModifiedOrder.Sum;
                notification.PaidDate = DateTime.UtcNow;
                notification.Currency = value.ModifiedOrder.Currency.ToString();

                notifications.Add(notification);
            }

            if(IsOrderSent(value))
            {
                var notification = _notificationManager.GetNewNotification<OrderSentEmailNotification>(value.ModifiedOrder.StoreId, "Store", "en-US");
                notification.OrderNumber = value.ModifiedOrder.Number;
                notification.SentOrderDate = DateTime.UtcNow;
                notification.NumberOfShipments = value.ModifiedOrder.Shipments.Count(i => i.Status == "Send");
                notification.ShipmentsNumbers = value.ModifiedOrder.Shipments.Select(i => i.Number).ToArray();

                notifications.Add(notification);
            }

            foreach(var notification in notifications)
            {
                SetNotificationParameters(notification, value);
                _notificationManager.ScheduleSendNotification(notification);
            }
        }

        /// <summary>
        /// Is order was canceled
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsOrderCanceled(OrderChangeEvent value)
        {
            var retVal = false;

            retVal = value.OrigOrder != null &&
                     value.OrigOrder.IsCancelled != value.ModifiedOrder.IsCancelled &&
                     value.ModifiedOrder.IsCancelled;

            return retVal;
        }

        /// <summary>
        /// Is order gets new status
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsNewStatus(OrderChangeEvent value)
        {
            var retVal = false;

            retVal = value.OrigOrder != null &&
                     value.OrigOrder.Status != value.ModifiedOrder.Status;

            return retVal;
        }

        /// <summary>
        /// Is order fully paid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsOrderPaid(OrderChangeEvent value)
        {
            var retVal = false;
            foreach(var origPayment in value.OrigOrder.InPayments)
            {
                var modifiedPayment = value.ModifiedOrder.InPayments.FirstOrDefault(i => i.Id == origPayment.Id);
                var paidSum = value.ModifiedOrder.InPayments.Where(i => i.PaymentStatus == PaymentStatus.Paid).Sum(i => i.Sum);
                if (modifiedPayment != null)
                {
                    retVal = modifiedPayment.PaymentStatus == PaymentStatus.Paid && origPayment.PaymentStatus != PaymentStatus.Paid && paidSum == value.ModifiedOrder.Sum;
                }
                if (retVal)
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// Is order fully send
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsOrderSent(OrderChangeEvent value)
        {
            var retVal = false;
            foreach(var origShipment in value.OrigOrder.Shipments)
            {
                var modifiedShipment = value.ModifiedOrder.Shipments.FirstOrDefault(i => i.Id == origShipment.Id);
                if(modifiedShipment != null)
                {
                    retVal = (modifiedShipment.Status != origShipment.Status && modifiedShipment.Status == "Send") || (retVal && modifiedShipment.Status == "Send");
                }
            }

            return retVal;
        }

        /// <summary>
        /// Set base notificaiton parameters (sender, recipient, isActive)
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="value"></param>
        private void SetNotificationParameters(EmailNotification notification, OrderChangeEvent value)
        {

            var store = _storeService.GetById(value.ModifiedOrder.StoreId);
            notification.Sender = store.Email;
            notification.IsActive = true;

            var contact = _contactService.GetById(value.ModifiedOrder.CustomerId);
            if (contact != null)
            {
                var email = contact.Emails.FirstOrDefault();
                if (!string.IsNullOrEmpty(email))
                {
                    notification.Recipient = email;
                }
            }
            if (string.IsNullOrEmpty(notification.Recipient))
            {
                if (value.ModifiedOrder.Addresses.Count > 0)
                {
                    var address = value.ModifiedOrder.Addresses.FirstOrDefault();
                    if (address != null)
                    {
                        notification.Recipient = address.Email;
                    }
                }
            }
        }
    }
}
