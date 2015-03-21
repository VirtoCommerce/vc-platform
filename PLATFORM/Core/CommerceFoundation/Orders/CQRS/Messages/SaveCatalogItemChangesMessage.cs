using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks.CQRS;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.Foundation.Orders.CQRS.Messages
{
	[DataContract]
	public class SaveCatalogItemChangesMessage : IMessage
	{
		[DataMember]
		public Item CatalogItem
		{
			get;
			set;
		}

		public SaveCatalogItemChangesMessage(Item item)
		{
			CatalogItem = item;
		}
	}
}
