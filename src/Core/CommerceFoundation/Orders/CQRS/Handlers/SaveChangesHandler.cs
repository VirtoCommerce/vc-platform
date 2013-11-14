using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Orders.CQRS.Messages;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Frameworks.CQRS;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;
using VirtoCommerce.Foundation.Catalogs;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.Foundation.Orders.CQRS.Handlers
{
	[MessageType(typeof(SaveOrderGroupChangesMessage))]
	[MessageType(typeof(SavePaymentMethodChangesMessage))]
	[MessageType(typeof(SaveCatalogCategoryChangesMessage))]
	[MessageType(typeof(SaveCatalogChangesMessage))]
	[MessageType(typeof(SaveCatalogItemChangesMessage))]
	public class SaveChangesHandler : IConsume
	{
		protected IMessageSender MessageSender;

		public SaveChangesHandler(IMessageSender messageSender)
		{
			MessageSender = messageSender;
		}

		#region IConsume<SaveOrderChangesMesssage> Members

		public void Consume(IMessage message)
		{
			if (message is SaveOrderGroupChangesMessage)
			{
				var saveOrderMessage = message as SaveOrderGroupChangesMessage;
				//OrderContext.Current.SaveOrderGroups(new OrderGroup[] { saveOrderMessage.OrderGroup });

				// Add order to the indexing
				// MessageSender.Send(AzureConfiguration.Instance.CQRSOrderIndexingQueueName, message);
			}
			else if (message is SavePaymentMethodChangesMessage)
			{
				var savePaymentMethodMessage = message as SavePaymentMethodChangesMessage;
				//OrderContext.Current.SavePaymentMethods(new PaymentMethod[] { savePaymentMethodMessage.PaymentMethod });
			}
			else if (message is SaveCatalogCategoryChangesMessage)
			{
				var saveCatalogCategoryChangesMessage = message as SaveCatalogCategoryChangesMessage;
				//CatalogContext.Current.SaveCategories(new CategoryBase[] {  saveCatalogCategoryChangesMessage.Category });
			}
			else if (message is SaveCatalogChangesMessage)
			{
				var saveCatalogChangesMessage = message as SaveCatalogChangesMessage;
				//CatalogContext.Current.SaveCatalog(saveCatalogChangesMessage.Catalog);
			}
			else if (message is SaveCatalogItemChangesMessage)
			{
				var saveCatalogItemChangesMessage = message as SaveCatalogItemChangesMessage;
				//CatalogContext.Current.SaveCatalogItems(new Item[] { saveCatalogItemChangesMessage.CatalogItem });
			}
			
		}

		#endregion
	}
}
