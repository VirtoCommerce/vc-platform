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
	public class SaveCatalogCategoryChangesMessage : IMessage
	{
		[DataMember]
		public CategoryBase Category
		{
			get;
			set;
		}
		public SaveCatalogCategoryChangesMessage(CategoryBase category)
		{
			Category = category;
		}
	}
}
