using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.PricingModule.Data.Model
{
	public class PricelistAssignment : AuditableEntity
	{
		[StringLength(128)]
		[Required]
		public string Name { get; set; }

		[StringLength(512)]
		public string Description { get; set; }

		public int Priority { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public string ConditionExpression { get; set; }

		public string PredicateVisualTreeSerialized { get; set; }

		[StringLength(128)]
		[Required]
		public string CatalogId { get; set; }


		#region Navigation Properties
		public string PricelistId { get; set; }

		public virtual Pricelist Pricelist { get; set; }
	
		#endregion

	}
}
