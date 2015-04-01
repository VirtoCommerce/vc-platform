using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public abstract class PromotionReward
	{
		/// <summary>
		/// Flag for applicability
		/// </summary>
		public bool IsValid { get; set; }
		/// <summary>
		/// Promo information. (user instructions, cuurent promo description)
		/// </summary>
		public string PromoInformation { get; set; }
		/// <summary>
		/// Coupon amount
		/// </summary>
		public decimal CouponAmount { get; set; }
		/// <summary>
		/// Coupon
		/// </summary>
		public string Coupon { get; set; }

		/// <summary>
		/// Minimal amount in order to apply a coupon
		/// </summary>
		public decimal? CouponMinOrderAmount { get; set; }

		//Promotion 
		public Promotion Promotion { get; private set; }

		public bool IsExclusive { get; set; }
	}
}
