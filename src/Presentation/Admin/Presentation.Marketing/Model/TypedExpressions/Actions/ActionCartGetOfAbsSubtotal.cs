using System;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Marketing.Model;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
    [Serializable]
	public class ActionCartGetOfAbsSubtotal : PromotionExpressionBlock
    {
        private readonly UserInputElement _amountEl;

        public ActionCartGetOfAbsSubtotal(IExpressionViewModel expressionViewModel)
            : base("Get $[] off cart subtotal", expressionViewModel)
        {
            WithLabel("Get $");
            _amountEl = WithUserInput<decimal>(0, 0) as UserInputElement;
            WithLabel("off cart subtotal");
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
                AmountTypeId = (int)RewardAmountType.Absolute
            };
            return new PromotionReward[] { retVal };
        }
    }
}