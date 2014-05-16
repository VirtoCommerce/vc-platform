using System;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
	[Serializable]
	public class ActionShippingGetOfRelShippingMethod : PromotionExpressionBlock
	{
		private readonly UserInputElement _amountEl;
		private readonly ShippingMethodSelector _shippingMethodSelectorEl;

		public ActionShippingGetOfRelShippingMethod(IExpressionViewModel expressionViewModel)
			: base("Get [] % off shipping".Localize(), expressionViewModel)
		{
			WithLabel("Get".Localize());
			_amountEl = WithUserInput(0, 0, 100) as UserInputElement;
			WithLabel("% off shipping ".Localize());
			_shippingMethodSelectorEl = WithElement(new ShippingMethodSelector(expressionViewModel)) as ShippingMethodSelector;
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

		public string SelectedShippingMethodId
		{
			get
			{
				return Convert.ToString(_shippingMethodSelectorEl.InputValue);
			}
		}

		public override PromotionReward[] GetPromotionRewards()
		{
			var retVal = new ShipmentReward
			{
				Amount = Amount,
				AmountTypeId = (int)RewardAmountType.Relative,
				ShippingMethodId = SelectedShippingMethodId
			};
			return new PromotionReward[] { retVal };
		}

		public override void InitializeAfterDeserialized(IExpressionViewModel promotionViewModel)
		{
			base.InitializeAfterDeserialized(promotionViewModel);
			_shippingMethodSelectorEl.InitializeAvailableValues(promotionViewModel);
		}
	}
}