using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public abstract class AmountBasedReward : PromotionReward
	{
		public AmountBasedReward()
		{
		}

		protected AmountBasedReward (AmountBasedReward other)
			:base(other)
		{
			AmountType = other.AmountType;
			Amount = other.Amount;
			Quantity = other.Quantity;
		}

		public RewardAmountType AmountType { get; set; }

		public decimal Amount { get; set; }

		public int Quantity { get; set; }

		public decimal CalculateDiscountAmount(decimal price, int quantity = 1)
		{
			var retVal = Amount;
			if (AmountType == RewardAmountType.Relative)
			{
				retVal = Math.Floor(price * Amount) * Math.Min(quantity, Quantity == 0 ? quantity : Quantity);
			}
			return FinanceRound(retVal);
		}

		private static decimal FinanceRound(decimal value)
		{
			return Math.Round(value, 2, MidpointRounding.AwayFromZero);
		}

		public override string ToString()
		{
			return String.Format(" {0} {1}{2}{3}", base.ToString(),  Amount,  AmountType == RewardAmountType.Absolute ? "$"  : "%", Quantity > 0 ? "for "+ Quantity + " pcs" : String.Empty);
		}
	
	}
}
