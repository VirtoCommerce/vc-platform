using System;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Localization;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.AppConfig.Model
{
    [Serializable]
    public class DisplayTemplateExpressionBlock : TypedExpressionElementBase, IExpressionAdaptor
    {
        private ConditionAndOrBlock _ConditionBlock;
        public ConditionAndOrBlock ConditionBlock
        {
            get
            {
                return _ConditionBlock;
            }
            private set
            {
                _ConditionBlock = value;
                Children.Add(_ConditionBlock);
            }
        }

        #region ctor
        public DisplayTemplateExpressionBlock(IExpressionViewModel templateViewModel)
            : base(null, templateViewModel)
        {
            InitializeFirstChild();
            //InitializeAvailableExpressions();
        }
        #endregion

        private void InitializeFirstChild()
        {
            this.ConditionBlock = new ConditionAndOrBlock("if".Localize(), ExpressionViewModel, "of these conditions are true".Localize());
        }

        private void InitializeAvailableExpressions()
        {
            var availableElements = new Func<CompositeElement>[] { 
				()=> { //Items conditions menu items
					var group = new CompositeElement { DisplayName = "Item conditions".Localize() };
					group.AvailableChildrenGetters.AddRange(new Func<ExpressionElement>[] {
						()=> new ConditionEntryIs(this.ExpressionViewModel),
						()=> new ConditionItemTypeIs(this.ExpressionViewModel),
						()=> new ConditionItemPropertyIs(this.ExpressionViewModel),
					});
					return group;
				},
				()=> { //Category conditions menu items
					var group = new CompositeElement { DisplayName = "Category conditions".Localize() };
					group.AvailableChildrenGetters.AddRange(new Func<ExpressionElement>[] {
						()=> new ConditionCategoryIs(this.ExpressionViewModel),
					});
					return group;
				},
            };
            ConditionBlock.WithAvailabeChildren(availableElements);
            ConditionBlock.NewChildLabel = "+ add condition".Localize(null, LocalizationScope.DefaultCategory);
        }

        public void ResetChildren()
        {
            this.Children.Clear();
            InitializeFirstChild();

        }

        public void InitializeAvailableExpressions(TargetTypes targetType)
        {
            ConditionBlock.AvailableChildrenGetters.Clear();
            switch (targetType)
            {
                case TargetTypes.Item:
                    //Items conditions menu items;
                    var availableItemElements = new Func<CompositeElement>[] { 
						()=> new ConditionEntryIs(this.ExpressionViewModel),
						()=> new ConditionItemTypeIs(this.ExpressionViewModel),
						()=> new ConditionItemPropertyIs(this.ExpressionViewModel),
						()=> new ConditionItemsCategoryIs(this.ExpressionViewModel),
						()=> new ConditionItemStoreIs(this.ExpressionViewModel),
					};
                    ConditionBlock.WithAvailabeChildren(availableItemElements);
                    break;
                case TargetTypes.Category:
                    //Category conditions menu items;
                    var availableCategoryElements = new Func<CompositeElement>[] { 
						()=> new ConditionCategoryIs(this.ExpressionViewModel),
						()=> new ConditionCategoryPropertyIs(this.ExpressionViewModel),
						()=> new ConditionCategoryIsSubcategory(this.ExpressionViewModel),
						()=> new ConditionCategoryStoreIs(this.ExpressionViewModel),
					};
                    ConditionBlock.WithAvailabeChildren(availableCategoryElements);
                    break;
            }

            ConditionBlock.NewChildLabel = "+ add condition".Localize(null, LocalizationScope.DefaultCategory);
        }

        public override void InitializeAfterDeserialized(IExpressionViewModel parentViewModel)
        {
            base.InitializeAfterDeserialized(parentViewModel);
            InitializeAvailableExpressions();

        }

        public linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
        {
            var retVal = ConditionBlock.GetExpression();
            return retVal;
        }

    }
}
