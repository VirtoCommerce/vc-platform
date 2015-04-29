using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CatalogModule.Data.Model
{
	public class PropertyAttribute : AuditableEntity
	{
		[Required]
		[StringLength(128)]
		public string PropertyAttributeName { get; set; }

		[Required]
		[StringLength(128)]
		public string PropertyAttributeValue { get; set; }

		public int Priority { get; set; }

		#region Navigation Properties

		[StringLength(128)]
		[Required]
		public string PropertyId { get; set; }

		[Parent]
		[ForeignKey("PropertyId")]
		public virtual Property Property { get; set; }

		#endregion
	}
}
