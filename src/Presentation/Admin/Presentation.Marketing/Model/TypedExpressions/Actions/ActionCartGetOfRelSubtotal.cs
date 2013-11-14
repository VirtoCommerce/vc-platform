using System;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Marketing.Model;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
        [Serializable]
	public class ActionCartGetOfRelSubtotal : PromotionExpressionBlock
	{
		private readonly UserInputElement _amountEl;

		public ActionCartGetOfRelSubtotal(IExpressionViewModel expressionViewModel)
			: base("Get [] % off cart subtotal", expressionViewModel)
		{
			WithLabel("Get");
			_amountEl = WithUserInput(0, 0, 100) as UserInputElement;
			WithLabel("% off cart subtotal");
		}

		public decimal Amount
		{
			get
			{
				return Convert.ToDecimal(_amountEl.InputValue);
			}
			set
			{
				_amountEl.InputValue = value;
			}
		}


		public override PromotionReward[] GetPromotionRewards()
		{
			var retVal = new CartSubtotalReward
			{
				Amount = Amount,
				AmountTypeId = (int)RewardAmountType.Relative
			};
			return new PromotionReward[] { retVal };
		}
	}
}