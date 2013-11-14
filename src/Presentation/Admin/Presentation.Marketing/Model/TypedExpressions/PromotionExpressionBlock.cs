using System;
using System.Linq;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
    [Serializable]
	public abstract class PromotionExpressionBlock : TypedExpressionElementBase, IExpressionMarketingAdaptor
    {
	    protected PromotionExpressionBlock(string label, IExpressionViewModel promotionViewModel)
            : base(label, promotionViewModel)
        {
        }
		
		public override void InitializeAfterDeserialized(IExpressionViewModel promotionViewModel)
        {
            base.InitializeAfterDeserialized(promotionViewModel);
        }


		public virtual PromotionReward[] GetPromotionRewards()
		{
			throw new NotImplementedException();
		}


		public virtual System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			throw new NotImplementedException();
		}


		#region Properties

		public string[] ExcludingCategoryIds
		{
			get
			{
				return AllExcludingElements.OfType<ItemsInCategory>().Select(x => x.SelectedCategoryId).ToArray();
			}
			set
			{
				foreach (var itemsInCategory in value.Select(categoryId => new ItemsInCategory(ExpressionViewModel) {SelectedCategoryId = categoryId}))
				{
					AddExludingElement(itemsInCategory);
				}
			}
		}

		public string[] ExcludingProductIds
		{
			get
			{
				return AllExcludingElements.OfType<ItemsInEntry>().Select(x => x.SelectedItemId).ToArray();
			}
			set
			{
				foreach (var itemsInProduct in value.Select(productId => new ItemsInEntry(ExpressionViewModel) { SelectedItemId = productId }))
				{
					AddExludingElement(itemsInProduct);
				}
			}
		}

		public string[] ExcludingSkuIds
		{
			get
			{
				return AllExcludingElements.OfType<ItemsInSku>().Select(x => x.SelectedSkuId).ToArray();
			}
			set
			{
				foreach (var itemsInSku in value.Select(skuId => new ItemsInSku(ExpressionViewModel) { SelectedSkuId = skuId }))
				{
					AddExludingElement(itemsInSku);
				}
			}
		}
		#endregion
	}
}
