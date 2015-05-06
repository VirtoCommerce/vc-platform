using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
	public class PromotionReward
	{
		/// <summary>
		/// Flag for applicability (applied or potential)
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

		public string PromotionId { get; set; }
		//Promotion 
		public Promotion Promotion { get; set; }

		public string RewardType { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public RewardAmountType AmountType { get; set; }

		public decimal Amount { get; set; }

		public int Quantity { get; set; }

		public string LineItemId { get; set; }
		public string ProductId { get; set; }
		public string CategoryId { get; set; }

		public string MeasureUnit { get; set; }

		public string ImageUrl { get; set; }
	}
}