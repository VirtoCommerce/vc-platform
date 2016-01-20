﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Pricing.Model
{
	public class PriceEvaluationContext : EvaluationContextBase
	{
		public string StoreId { get; set; } 
		public string CatalogId { get; set; }
		public string[] ProductIds { get; set; }
		public string[] PricelistIds { get; set; }
		public decimal Quantity { get; set; }
		public string CustomerId { get; set; }
		public string OrganizationId { get; set; }
		public DateTime? CertainDate { get; set; }
		public string Currency { get; set; }
		public string[] Tags { get; set; }
	}
}
