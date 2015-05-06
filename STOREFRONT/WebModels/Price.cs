using DotLiquid;

namespace VirtoCommerce.Web.Models
{
    public class Price : Drop
    {
        public string PricelistId { get; set; }
        public string ProductId { get; set; }
        public decimal? Sale { get; set; }
        public decimal List { get; set; }
        public int MinQuantity { get; set; }
    }
}