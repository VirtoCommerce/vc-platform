#region
using DotLiquid;
using DotLiquid.Util;
using Xunit;

#endregion

namespace Web.Tests
{
    public class TemplateScenarios
    {
        #region Public Methods and Operators
        public static void AssertTemplateResult(string expected, string template, Hash localVariables)
        {
            Assert.Equal(expected, Template.Parse(template).Render(localVariables));
        }

        [Fact]
        public void Can_process_template()
        {
            //var f = "t: price: sale_price";
            //var f = "num_footer_columns | plus: 1 ";
            //@" truncatewords: 1, '' "
            //"{| t: count: article.comments_count }"
            var f = "t: count: cart.item_count, asdasd";
            //(?<!:)

            var completeExpression = @"(?:(?<=^)\w+:)|,\s*|""[^""]*""|'[^']*'|(?:[^,\|'""]|""[^""]*""|'[^']*')+";
            var QuotedFragment = string.Format(R.Q(@"{0}|(?:[^,\|'""]|{0})+"), Liquid.QuotedString);
            var filterArgs = R.Scan(
                f,
                string.Format(
                    R.Q(@"(?:{0}|{1})\s*({2})"),
                    Liquid.FilterArgumentSeparator,
                    Liquid.ArgumentSeparator,
                    QuotedFragment));
            var filterArgs2 = R.Scan(f, string.Format(R.Q(completeExpression)));
            //Liquid.ArgumentSeparator

            var parsed = Template.Parse("{{ 'layout.cart.items_count' | t: count: cart.item_count }}");
        }

        [Fact]
        public void Can_times()
        {
            //            AssertTemplateResult("12", "{{ 3 | times:4 }}", null);
            AssertTemplateResult("12", "{{ 24 | times:0.5 }}", null);
        }
        #endregion
    }
}