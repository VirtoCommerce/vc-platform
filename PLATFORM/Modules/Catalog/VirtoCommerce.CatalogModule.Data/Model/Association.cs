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
		public string AssociationGroupId { get; set; }
		public virtual AssociationGroup AssociationGroup { get; set; }

		public string ItemId { get; set; }
		public virtual Item CatalogItem { get; set; }
		#endregion
	}
}
