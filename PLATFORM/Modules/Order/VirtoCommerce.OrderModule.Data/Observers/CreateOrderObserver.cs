using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.OrderModule.Data.Notification;
using VirtoCommerce.Platform.Core.Notification;

namespace VirtoCommerce.OrderModule.Data.Observers
{
	public class CreateOrderObserver : IObserver<OrderChangeEvent>
	{
		private readonly INotificationManager _notificationManager;
		private readonly IStoreService _storeService;
		private readonly IContactService _contactService;

		public CreateOrderObserver(INotificationManager notificationManager, IStoreService storeService, IContactService contactService)
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
			if (value.ChangeState == Platform.Core.Common.EntryState.Added)
			{
				SendCreateOrderNotification(value);
			}
		}

		private void SendCreateOrderNotification(OrderChangeEvent value)
		{
			var notification = _notificationManager.GetNewNotification<OrderCreateEmailNotification>(value.ModifiedOrder.StoreId, "Store", "en-US");
			notification.OrderNumber = value.ModifiedOrder.Number;
			var store = _storeService.GetById(value.ModifiedOrder.StoreId);

			notification.Sender = store.Email;

			var contact = _contactService.GetById(value.ModifiedOrder.CustomerId);
			if(contact != null)
			{
				var email = contact.Emails.FirstOrDefault();
				if(!string.IsNullOrEmpty(email))
				{
					notification.Recipient = email;
				}
			}
			if(string.IsNullOrEmpty(notification.Recipient))
			{
				if(value.ModifiedOrder.Addresses.Count > 0)
				{
					var address = value.ModifiedOrder.Addresses.FirstOrDefault();
					if(address != null)
					{
						notification.Recipient = address.Email;
					}
				}
			}

			notification.IsActive = true;

			_notificationManager.ScheduleSendNotification(notification);
		}
	}
}
