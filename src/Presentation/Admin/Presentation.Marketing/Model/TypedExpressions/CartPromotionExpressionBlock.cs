using System;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Marketing.Model;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
    [Serializable]
	public class CartPromotionExpressionBlock : PromotionExpressionBlock
    {
		private ConditionAndOrBlock _conditionCustomerSegmentBlock;
        public ConditionAndOrBlock ConditionCutomerSegmentBlock
        {
            get
            {
				return _conditionCustomerSegmentBlock;
            }
            private set
            {
				_conditionCustomerSegmentBlock = value;
                Children.Add(_conditionCustomerSegmentBlock);
            }
        }

        private ConditionAndOrBlock _conditionCartBlock;
        public ConditionAndOrBlock ConditionCartBlock
        {
            get
            {
                return _conditionCartBlock;
            }
            private set
            {
                _conditionCartBlock = value;
                Children.Add(_conditionCartBlock);
            }
        }

        private ActionBlock _actionBlock;
        public ActionBlock ActionBlock
        {
            get
            {
                return _actionBlock;
            }
            private set
            {
                _actionBlock = value;
                Children.Add(_actionBlock);
            }
        }

        public CartPromotionExpressionBlock(IExpressionViewModel promotionViewModel)
            : base(null, promotionViewModel)
        {
            ConditionCutomerSegmentBlock = new ConditionAndOrBlock("For visitor with", promotionViewModel, "of these eligibilities");
            ConditionCartBlock = new ConditionAndOrBlock("if", promotionViewModel, "of these conditions are true");
            ActionBlock = new ActionBlock("They get:", promotionViewModel);

            InitializeAvailableExpressions();
        }

        private void InitializeAvailableExpressions()
        {
            var availableElements = new Func<CompositeElement>[] {
                ()=> new ConditionIsEveryone(ExpressionViewModel),
                ()=> new ConditionIsFirstTimeBuyer(ExpressionViewModel),
                ()=> new ConditionIsRegisteredUser(ExpressionViewModel)
			  };
            ConditionCutomerSegmentBlock.WithAvailabeChildren(availableElements);
            ConditionCutomerSegmentBlock.NewChildLabel = "+ add usergroup";
	        ConditionCutomerSegmentBlock.IsChildrenRequired = true;
	        ConditionCutomerSegmentBlock.ErrorMessage = "Cart promotion requires at least one eligibility";

            availableElements = new Func<CompositeElement>[] {
				()=> new ConditionAtNumItemsInCart(ExpressionViewModel),
				()=> new  ConditionAtNumItemsInCategoryAreInCart(ExpressionViewModel),
				()=> new  ConditionAtNumItemsOfEntryAreInCart(ExpressionViewModel),
				()=> new  ConditionCartSubtotalLeast(ExpressionViewModel),
				()=> new  ConditionCurrencyIs(ExpressionViewModel)
            };
            ConditionCartBlock.WithAvailabeChildren(availableElements);
            ConditionCartBlock.NewChildLabel = "+ add condition";

			availableElements = new Func<CompositeElement>[] {
				()=> { //Cart item discounts menu items
					var group = new CompositeElement { DisplayName = "Cart item discount" };
					group.AvailableChildrenGetters.AddRange(new Func<ExpressionElement>[] {
						()=> new ActionCartItemGetOfRelForNum(ExpressionViewModel),
						()=> new ActionCartItemGetOfAbsForNum(ExpressionViewModel),
						()=> new ActionCartItemGetOfRelForNumInCategory(ExpressionViewModel),
						()=> new ActionCartItemGetOfAbsForNumInCategory(ExpressionViewModel),
						()=> new ActionCartItemGetOfRelForNumInEntry(ExpressionViewModel),
						()=> new ActionCartItemGetOfAbsForNumInEntry(ExpressionViewModel),
						()=> new ActionCartItemGetFreeNumItemOfSku(ExpressionViewModel)});
					return group;
				},
				()=> { //Cart subtotal discounts menu items
					var group = new CompositeElement { DisplayName = "Cart subtotal discount" };
					group.AvailableChildrenGetters.AddRange(new Func<ExpressionElement>[] {
						()=> new ActionCartGetOfRelSubtotal(ExpressionViewModel),
						()=> new ActionCartGetOfAbsSubtotal(ExpressionViewModel)});
					return group;
				},			
				()=> { //Shipping discounts menu items
					var group = new CompositeElement { DisplayName = "Shipping discount" };
					group.AvailableChildrenGetters.AddRange(new Func<ExpressionElement>[] {
						()=> new ActionShippingGetOfRelShippingMethod(ExpressionViewModel),
						()=> new ActionShippingGetOfAbsShippingMethod(ExpressionViewModel)});
					return group;
				} 
			};
			ActionBlock.WithAvailabeChildren(availableElements);
			ActionBlock.NewChildLabel = "+ add effect";
	        ActionBlock.IsChildrenRequired = true;
	        ActionBlock.ErrorMessage = "Promotion requires at least one reward";
        }

		public override PromotionReward[] GetPromotionRewards()
		{
			return ActionBlock.GetPromotionRewards();
		}

        public override System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetExpression()
        {
            var customerExpression = ConditionCutomerSegmentBlock.GetExpression();
            var cartExpression = ConditionCartBlock.GetExpression();

            return customerExpression.And(cartExpression);
        }

        public override void InitializeAfterDeserialized(IExpressionViewModel promotionViewModel)
        {
            base.InitializeAfterDeserialized(promotionViewModel);
            InitializeAvailableExpressions();
        }
    }
}
