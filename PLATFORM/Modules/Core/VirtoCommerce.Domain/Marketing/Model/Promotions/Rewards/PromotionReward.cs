using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public abstract class PromotionReward 
	{
		public PromotionReward()
		{
		}
		//Copy constructor
		protected PromotionReward(PromotionReward other)
		{
			IsValid = other.IsValid;
			Description = other.Description;
			CouponAmount = other.CouponAmount;
			Coupon = other.Coupon;
			CouponMinOrderAmount = other.CouponMinOrderAmount;
			Promotion = other.Promotion;
			IsExclusive = other.IsExclusive;
		}

		/// <summary>
		/// Flag for applicability
		/// </summary>
		public bool IsValid { get; set; }
		/// <summary>
		/// Promo information. (user instructions, cuurent promo description)
		/// </summary>
		public string Description { get; set; }
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
		public Promotion Promotion { get; set; }

		public bool IsExclusive { get; set; }

		public abstract PromotionReward Clone();

		public override string ToString()
		{
			return String.Format("{0} of promotion {1} -  {2}", this.GetType().Name, Promotion != null ? Promotion.Id : "undef", IsValid ? "Valid" : "Invalid");
		}
	}
}
