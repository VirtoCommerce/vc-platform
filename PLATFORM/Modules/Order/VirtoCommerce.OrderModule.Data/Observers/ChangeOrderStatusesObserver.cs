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

namespace VirtoCommerce.OrderModule.Data.Observers
{
    public class ChangeOrderStatusesObserver : IObserver<OrderChangeEvent>
    {
        private readonly INotificationManager _notificationManager;
        private readonly IStoreService _storeService;
        private readonly IContactService _contactService;

        public ChangeOrderStatusesObserver(INotificationManager notificationManager, IStoreService storeService, IContactService contactService)
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
            var status = GetNewStatus(value);
            if (status.Equals("NewOrder"))
            {
                CreateOrderNotificationSend(value);
            }
            else if (status.Equals("Paid"))
            {
                OrderPaidNotificationSend(value);
            }
            else if (status.Equals("Sent"))
            {
                OrderSentNotificationSend(value);
            }
            else if (status.Equals("Cancelled"))
            {
                CancelOrderNotificationSend(value);
            }
            else if(!string.IsNullOrEmpty(status))
            {
                NewOrderStatusNotificationSend(value, status);
            }
        }

        private string GetNewStatus(OrderChangeEvent value)
        {
            var retVal = string.Empty;
            if (value.ChangeState == EntryState.Added)
            {
                retVal = "NewOrder";
            }
            else if (value.ChangeState == EntryState.Modified)
            {
                if (value.OrigOrder.Status != value.ModifiedOrder.Status)
                {
                    retVal = value.ModifiedOrder.Status;
                }
                else if (CheckAllPaymentsStatus(value))
                {
                    retVal = "Paid";
                }
                else if (CheckAllShipmentsStatus(value))
                {
                    retVal = "Sent";
                }
            }

            return retVal;
        }

        private bool CheckAllPaymentsStatus(OrderChangeEvent value)
        {
            var retVal = false;
            foreach(var origPayment in value.OrigOrder.InPayments)
            {
                var modifiedPayment = value.ModifiedOrder.InPayments.FirstOrDefault(i => i.Id == origPayment.Id);
                var paidSum = value.ModifiedOrder.InPayments.Where(i => i.Status == "Paid").Sum(i => i.Sum);
                if (modifiedPayment != null)
                {
                    retVal = modifiedPayment.Status == "Paid" && origPayment.Status != "Paid" && paidSum == value.ModifiedOrder.Sum;
                }
                if (retVal)
                    break;
            }

            return retVal;
        }

        private bool CheckAllShipmentsStatus(OrderChangeEvent value)
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
        /// Send notification when order creates
        /// </summary>
        /// <param name="value"></param>
        private void CreateOrderNotificationSend(OrderChangeEvent value)
        {
            var notification = _notificationManager.GetNewNotification<OrderCreateEmailNotification>(value.ModifiedOrder.StoreId, "Store", "en-US");
            notification.OrderNumber = value.ModifiedOrder.Number;

            SetNotificationParameters(notification, value);

            _notificationManager.ScheduleSendNotification(notification);
        }

        /// <summary>
        /// Send notification when order gets new status
        /// </summary>
        /// <param name="value"></param>
        private void NewOrderStatusNotificationSend(OrderChangeEvent value, string status)
        {
            var notification = _notificationManager.GetNewNotification<NewOrderStatusEmailNotification>(value.ModifiedOrder.StoreId, "Store", "en-US");
            notification.OrderNumber = value.ModifiedOrder.Number;
            notification.NewStatus = status;

            SetNotificationParameters(notification, value);

            _notificationManager.ScheduleSendNotification(notification);
        }

        /// <summary>
        /// Send notification when order became fully paid
        /// </summary>
        /// <param name="value"></param>
        private void OrderPaidNotificationSend(OrderChangeEvent value)
        {
            var notification = _notificationManager.GetNewNotification<OrderPaidEmailNotification>(value.ModifiedOrder.StoreId, "Store", "en-US");
            notification.OrderNumber = value.ModifiedOrder.Number;
            notification.FullPrice = value.ModifiedOrder.Sum;
            notification.PaidDate = DateTime.UtcNow;
            notification.Currency = value.ModifiedOrder.Currency.ToString();

            SetNotificationParameters(notification, value);

            _notificationManager.ScheduleSendNotification(notification);
        }

        /// <summary>
        /// Send notification when order became fully sent
        /// </summary>
        /// <param name="value"></param>
        private void OrderSentNotificationSend(OrderChangeEvent value)
        {
            var notification = _notificationManager.GetNewNotification<OrderSentEmailNotification>(value.ModifiedOrder.StoreId, "Store", "en-US");
            notification.OrderNumber = value.ModifiedOrder.Number;
            notification.SentOrderDate = DateTime.UtcNow;
            notification.NumberOfShipments = value.ModifiedOrder.Shipments.Count(i => i.Status == "Send");
            notification.ShipmentsNumbers = value.ModifiedOrder.Shipments.Select(i => i.Number).ToArray();

            SetNotificationParameters(notification, value);

            _notificationManager.ScheduleSendNotification(notification);
        }

        private void CancelOrderNotificationSend(OrderChangeEvent value)
        {
            var notification = _notificationManager.GetNewNotification<CancelOrderEmailNotification>(value.ModifiedOrder.StoreId, "Store", "en-US");
            notification.OrderNumber = value.ModifiedOrder.Number;
            notification.CancelationReason = value.ModifiedOrder.CancelReason;

            SetNotificationParameters(notification, value);

            _notificationManager.ScheduleSendNotification(notification);
        }

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
