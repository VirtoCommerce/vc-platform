using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model
{
	public class PromotionUsage : AuditableEntity
	{
		[StringLength(128)]
		public string MemberId { get; set; }

		[StringLength(256)]
		public string MemberName { get; set; }

		[StringLength(128)]
		public string OrderId { get; set; }
		[StringLength(128)]
		public string OrderNumber { get; set; }

		[StringLength(64)]
		public string CouponCode { get; set; }

		public DateTime? UsageDate { get; set; }
	
		#region Navigation Properties

		[StringLength(128)]
		[ForeignKey("Promotion")]
		public string PromotionId { get; set; }

        public virtual Promotion Promotion { get; set; }

		#endregion

	}
}
