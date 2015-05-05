using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Data.Model
{
	public class ItemRelation : AuditableEntity
	{
		public ItemRelation()
		{
			Id = Guid.NewGuid().ToString("N");
		}
		[StringLength(64)]
		public string RelationTypeId { get; set; }

		public decimal Quantity { get; set; }

		[StringLength(64)]
		[Required]
		public string GroupName { get; set; }

		public int Priority { get; set; }

		#region Navigation Properties

		[StringLength(128)]
		[ForeignKey("ChildItem")]
		[Required]
		public string ChildItemId { get; set; }
		public virtual Item ChildItem { get; set; }

		[StringLength(128)]
		[ForeignKey("ParentItem")]
		[Required]
		public string ParentItemId { get; set; }

		public virtual Item ParentItem { get; set; }
		#endregion
	}
}
