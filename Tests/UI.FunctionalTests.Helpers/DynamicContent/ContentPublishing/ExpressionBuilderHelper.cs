using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.DynamicContent.Model;

namespace UI.FunctionalTests.Helpers.DynaminContent.ContentPublishing
{
    public class TestContentPublishingExpressionBuilder
    {
        private readonly ExpressionElement _child;

        private TestContentPublishingExpressionBuilder(ExpressionElement child)
        {
            _child = child;
        }

        public static TestContentPublishingExpressionBuilder BuildContentPublishingExpressionBuilder(ExpressionElement child)
        {
            return new TestContentPublishingExpressionBuilder(child);
        }

        public TestContentPublishingExpressionBuilder AddCartTotalElement(
            IExpressionViewModel expressionViewModel)
        {
            ((ConditionAndOrBlock)_child).Children.Add(
                new CartTotalElement(expressionViewModel));

            return this;
        }

        public TestContentPublishingExpressionBuilder AddConditionAddOrBlock(
            IExpressionViewModel expressionViewModel)
        {
            ((ConditionAndOrBlock)_child).Children.Add(
                new ConditionAndOrBlock("default_prefix", expressionViewModel,
                    "default_suffix"));

            return this;
        }

        public ExpressionElement GetChild()
        {
            return _child;
        }

    }
}
