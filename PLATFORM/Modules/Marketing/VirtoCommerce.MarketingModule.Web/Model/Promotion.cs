using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Web.Converters;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Web.Model
{
	/// <summary>
	/// Represent marketing promotion, define applicable rules and rewards amount in marketing system
	/// </summary>
	public class Promotion : AuditableEntity
	{
		/// <summary>
		/// It contains the name of realizing this type promotion.
		/// DynamicPromotion is build in implementation allow to construct promotion with dynamic conditions and rewards.
		/// For complex custom scenarios user may define personal 'hard-coded' promotion types
		/// </summary>
		public string Type { get; set; }
		public string Name { get; set; }
		/// <summary>
		/// Store id that is covered by this promotion
		/// </summary>
		public string Store { get; set; }
		/// <summary>
		/// Catalog id that is covered by this promotion
		/// </summary>
		public string Catalog { get; set; }

		public string Description { get; set; }
		public bool IsActive { get; set; }
		/// <summary>
		/// Maximum promotion usage count
		/// </summary>
		public int MaxUsageCount { get; set; }
		public int MaxPersonalUsageCount { get; set; }
		/// <summary>
		/// List of coupons codes which may be used for activate that promotion
		/// </summary>
		public string[] Coupons { get; set; }
		/// <summary>
		/// Used for choosing in combination 
		/// </summary>
		public int Priority { get; set; }

		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		/// <summary>
		/// Dynamic conditions tree determine the applicability of this promotion and reward definition
		/// </summary>
		public PromoDynamicExpressionTree DynamicExpression { get; set; }
	}
}