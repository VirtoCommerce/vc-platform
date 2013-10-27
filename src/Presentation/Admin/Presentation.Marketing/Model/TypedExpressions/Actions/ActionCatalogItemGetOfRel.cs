using System;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Marketing.Model;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
    [Serializable]
    public class ActionCatalogItemGetOfRel : PromotionExpressionBlock
    {
        private readonly UserInputElement _amountEl;

		public ActionCatalogItemGetOfRel(IExpressionViewModel expressionViewModel)
			: base("Get [] % off", expressionViewModel)
        {
            WithLabel("Get");
            _amountEl = WithUserInput(0, 0, 100) as UserInputElement;
            WithLabel("% off");
        }

        public int Amount
        {
            get
            {
                return Convert.ToInt16(_amountEl.InputValue);
            }
            set
            {
                _amountEl.InputValue = value;
            }
        }

        public override PromotionReward[] GetPromotionRewards()
        {
            var retVal = new CatalogItemReward { Amount = Amount, AmountTypeId = (int)RewardAmountType.Relative };
            return new PromotionReward[] { retVal };
        }
    }
}
