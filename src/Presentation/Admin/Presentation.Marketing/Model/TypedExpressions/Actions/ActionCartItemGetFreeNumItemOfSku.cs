using System;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Marketing.Model;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
    [Serializable]
	public class ActionCartItemGetFreeNumItemOfSku : PromotionExpressionBlock
    {
        private readonly UserInputElement _numItemEl;
        private readonly ItemsInSku _itemsInSkuEl;

		public ActionCartItemGetFreeNumItemOfSku(IExpressionViewModel promotionViewModel)
            : base("Get [] free items of SKU []", promotionViewModel)
        {
            WithLabel("Get");
            _numItemEl = WithUserInput(1) as UserInputElement;
            WithLabel("free");
			_itemsInSkuEl = WithElement(new ItemsInSku(promotionViewModel)) as ItemsInSku;
        }

        public string SelectedSkuId
        {
            get
            {
                return _itemsInSkuEl.SelectedSkuId;
            }
            set
            {
                _itemsInSkuEl.SelectedSkuId = value;
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
                Amount = 100,
                AmountTypeId = (int)RewardAmountType.Gift,
                QuantityLimit = NumItem,
                SkuId = SelectedSkuId
            };
            return new PromotionReward[] { retVal };
        }
    }
}
