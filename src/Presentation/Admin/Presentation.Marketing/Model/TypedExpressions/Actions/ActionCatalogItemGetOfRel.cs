using System;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
	[Serializable]
	public class ActionCatalogItemGetOfRel : PromotionExpressionBlock
	{
		private readonly UserInputElement _amountEl;

		public ActionCatalogItemGetOfRel(IExpressionViewModel expressionViewModel)
			: base("Get [] % off".Localize(), expressionViewModel)
		{
			WithLabel("Get".Localize());
			_amountEl = WithUserInput(0, 0, 100) as UserInputElement;
			WithLabel("% off".Localize());
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
