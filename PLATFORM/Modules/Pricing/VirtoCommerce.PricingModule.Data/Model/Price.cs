using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.PricingModule.Data.Model
{
	public class Price : AuditableEntity
	{
		public decimal? Sale { get; set; }
	
		[Required]
		public decimal List { get; set; }

		[StringLength(128)]
		public string ProductId { get; set; }

		[StringLength(1024)]
		public string ProductName { get; set; }

		public decimal MinQuantity { get; set; }

		#region Navigation Properties
		[StringLength(128)]
		[Required]
		public string PricelistId { get; set; }

        [Parent]
		[ForeignKey("PricelistId")] 
		public virtual Pricelist Pricelist { get; set; }
	

		#endregion

      
	}
}
