using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model
{
	public class Coupon : AuditableEntity
	{
		[StringLength(64)]
		public string Code { get; set; }

		#region Navigation Properties

		[StringLength(128)]
		[ForeignKey("Promotion")]
		public string PromotionId { get; set; }
		public virtual Promotion Promotion { get; set; }
		#endregion
	}
}
