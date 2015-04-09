using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public class CartSubtotalReward : PromotionReward
	{
		public CartSubtotalReward()
		{
		}
		//Copy constructor
		protected CartSubtotalReward(CartSubtotalReward other)
			:base(other)
		{
			Amount = other.Amount;
		}
		public decimal Amount { get; set; }

		public override PromotionReward Clone()
		{
			return new CartSubtotalReward(this);
		}


		public override string ToString()
		{
			return String.Format("{0} {1}$", base.ToString(), Amount);
		}
	}
}
