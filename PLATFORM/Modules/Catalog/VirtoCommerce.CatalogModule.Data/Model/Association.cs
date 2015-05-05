using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CatalogModule.Data.Model
{
	public class Association : AuditableEntity
	{
		public Association()
		{
			Id = Guid.NewGuid().ToString("N");
		}
		/// <summary>
		/// Gets or sets the type of the association. The examples association types are: optional, required. AssociationTypes.Required or AssociationTypes.Optional
		/// </summary>
		/// <value>
		/// The type of the association.
		/// </value>
		[StringLength(128)]
		[Required]
		public string AssociationType { get; set; }

		public int Priority { get; set; }

		#region Navigation Properties

		[StringLength(128)]
		public string AssociationGroupId { get; set; }
		[Parent]
		[ForeignKey("AssociationGroupId")]
		public virtual AssociationGroup AssociationGroup { get; set; }

		[StringLength(128)]
		public string ItemId { get; set; }

		[ForeignKey("ItemId")]
		public virtual Item CatalogItem { get; set; }
		#endregion
	}
}
