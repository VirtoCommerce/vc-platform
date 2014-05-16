using System;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
	[Serializable]
	public class ActionCartItemGetOfRelForNum : PromotionExpressionBlock
	{
		private readonly UserInputElement _numItemEl;
		private readonly UserInputElement _amountEl;

		public ActionCartItemGetOfRelForNum(IExpressionViewModel expressionViewModel)
			: base("Get []% off [] items".Localize(), expressionViewModel)
		{
			WithLabel("Get".Localize());
			_amountEl = WithUserInput(0, 0, 100) as UserInputElement;
			WithLabel("% off".Localize());
			_numItemEl = WithUserInput(1, 0) as UserInputElement;
			WithLabel("items".Localize());
			InitializeExcludings(expressionViewModel);
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

		public decimal NumItem
		{
			get
			{
				return Convert.ToDecimal(_numItemEl.InputValue);
			}
			set
			{
				_numItemEl.InputValue = value;
			}
		}


		public override PromotionReward[] GetPromotionRewards()
		{
			var retVal = new CatalogItemReward
			{
				Amount = Amount,
				AmountTypeId = (int)RewardAmountType.Relative,
				QuantityLimit = NumItem,
				ExcludingCategories = String.Join(";", ExcludingCategoryIds),
				ExcludingProducts = String.Join(";", ExcludingProductIds),
				ExcludingSkus = String.Join(";", ExcludingSkuIds)
			};
			return new PromotionReward[] { retVal };
		}

		public override void InitializeAfterDeserialized(IExpressionViewModel expressionViewModel)
		{
			base.InitializeAfterDeserialized(expressionViewModel);
			InitializeExcludings(expressionViewModel);
		}

		private void InitializeExcludings(IExpressionViewModel expressionViewModel)
		{
			WithAvailableExcluding(() => new ItemsInCategory(expressionViewModel));
			WithAvailableExcluding(() => new ItemsInEntry(expressionViewModel));
		}
	}
}
