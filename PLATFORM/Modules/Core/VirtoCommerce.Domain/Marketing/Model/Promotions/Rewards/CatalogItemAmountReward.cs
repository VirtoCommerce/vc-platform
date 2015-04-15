using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Domain.Marketing.Model
{
	
	public class CatalogItemAmountReward : AmountBasedReward
	{
		//Copy constructor
		protected CatalogItemAmountReward(CatalogItemAmountReward other)
			: base(other)
		{
			ProductId = other.ProductId;
		}

		public string ProductId { get; set; }

		public override PromotionReward Clone()
		{
			return new CatalogItemAmountReward(this);
		}

		public override string ToString()
		{
			return String.Format("{0} {1}", base.ToString(), ProductId != null ? "Product #("+ProductId+")" : String.Empty);
		}
	}
}
