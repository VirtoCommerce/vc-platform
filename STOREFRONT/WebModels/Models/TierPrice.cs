using DotLiquid;

namespace VirtoCommerce.Web.Models
{
    public class TierPrice : Drop
    {
        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}