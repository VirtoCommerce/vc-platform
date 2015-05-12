using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Pricing.Model
{
	public class Price : AuditableEntity
	{
		public string PricelistId { get; set; }
		public CurrencyCodes Currency { get; set; }
		public string ProductId { get; set; }
		public decimal? Sale { get; set; }
		public decimal List { get; set; }
		public int MinQuantity { get; set; }

		public decimal	EffectiveValue 
		{ 
			get
			{
				return Sale ?? List;
			}
		}
	}
}
