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
	public class SaveCatalogChangesMessage : IMessage
	{

		[DataMember]
		public CatalogBase Catalog
		{
			get;
			set;
		}

		public SaveCatalogChangesMessage(CatalogBase catalog)
		{
			Catalog = catalog;
		}
	}
}
