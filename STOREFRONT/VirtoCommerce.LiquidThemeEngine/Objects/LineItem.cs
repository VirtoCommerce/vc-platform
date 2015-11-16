using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class LineItem : Drop
    {
        private readonly Storefront.Model.Cart.LineItem _lineItem;

        public LineItem(Storefront.Model.Cart.LineItem lineItem)
        {
            _lineItem = lineItem;
        }
    }
}