using System;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Marketing.Model;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
    [Serializable]
    public class ActionCatalogItemGiftNumItemOfSku : PromotionExpressionBlock
    {
        private readonly UserInputElement _quantityEl;
        private readonly ItemsInSku _itemsInSKUEl;

		public ActionCatalogItemGiftNumItemOfSku(IExpressionViewModel expressionViewModel)
			: base("Gift [] of Sku []", expressionViewModel)
        {
            WithLabel("Gift");
            _quantityEl = WithUserInput(1, 0) as UserInputElement;
			_itemsInSKUEl = WithElement(new ItemsInSku(expressionViewModel)) as ItemsInSku;
        }

        public decimal Quantity
        {
            get
            {
                return Convert.ToDecimal(_quantityEl.InputValue);
            }
            set
            {
                _quantityEl.InputValue = value;
            }
        }


        public string SelectedSkuId
        {
            get
            {
                return _itemsInSKUEl.SelectedSkuId;
            }
            set
            {
                _itemsInSKUEl.SelectedSkuId = value;
            }
        }

        public override PromotionReward[] GetPromotionRewards()
        {
            var retVal = new CatalogItemReward { SkuId = SelectedSkuId, QuantityLimit = Quantity, AmountTypeId = (int)RewardAmountType.Gift };
            return new PromotionReward[] { retVal };
        }
    }
}
