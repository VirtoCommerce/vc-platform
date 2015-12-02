using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class TaxLine : Drop
    {
        public string Title { get; set; }

        public decimal Price { get; set; }
        public decimal Rate { get; set; }
        public decimal RatePercentage
        {
            get
            {
                return Rate * 100;
            }
        }
    }
}
