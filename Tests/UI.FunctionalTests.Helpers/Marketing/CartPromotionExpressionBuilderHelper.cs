using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Marketing.Model;

namespace UI.FunctionalTests.Helpers.Marketing
{
    public class CartPromotionExpressionBuilderHelper
    {
		private readonly CartPromotionExpressionBlock _child;

		private CartPromotionExpressionBuilderHelper(CartPromotionExpressionBlock child)
        {
            _child = child;
        }

		public static CartPromotionExpressionBuilderHelper BuildCartPromotionExpressionBuilder(CartPromotionExpressionBlock child)
        {
			return new CartPromotionExpressionBuilderHelper(child);
        }

		public CartPromotionExpressionBuilderHelper AddEveryoneEligibility(
			IExpressionViewModel expressionViewModel)
		{
			_child.ConditionCutomerSegmentBlock.Children.Add(
				new ConditionIsEveryone(expressionViewModel));

			return this;
		}

		public CartPromotionExpressionBuilderHelper AddIsRegisteredEligibility(
			IExpressionViewModel expressionViewModel)
		{
			_child.ConditionCutomerSegmentBlock.Children.Add(
				new ConditionIsRegisteredUser(expressionViewModel));

			return this;
		}

		public CartPromotionExpressionBuilderHelper AddIsFirstTimeBuyerEligibility(
			IExpressionViewModel expressionViewModel)
		{
			_child.ConditionCutomerSegmentBlock.Children.Add(
				new ConditionIsFirstTimeBuyer(expressionViewModel));

			return this;
		}

		public CartPromotionExpressionBuilderHelper AddNumItemsInCartElement(
            IExpressionViewModel expressionViewModel, bool isExactly)
		{
			var numItemsInCartCondition = new ConditionAtNumItemsInCart(expressionViewModel)
				{
					NumItem = 2,
					ExactlyLeast = {IsExactly = isExactly}
				};
			_child.ConditionCartBlock.Children.Add(
                numItemsInCartCondition);

            return this;
        }

		//public CartPromotionExpressionBuilderHelper AddTotalReward(
		//	IExpressionViewModel expressionViewModel)
		//{
		//	_child.ActionBlock.

		//	return this;
		//}

        public ExpressionElement GetChild()
        {
            return _child;
        }

    }
}
