using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Marketing
{
    public class PromotionEvaluationContext
    {
        public string StoreId { get; set; }

        public string Currency { get; set; }

        public string CustomerId { get; set; }

        public bool IsRegisteredUser { get; set; }

        public bool IsFirstTimeBuyer { get; set; }

        public bool IsEveryone { get; set; }

        public decimal CartTotal { get; set; }

        public string ShipmentCode { get; set; }

        public decimal ShipmentMethodPrice { get; set; }

        public ICollection<string> AvailableShipmentMethodCodes { get; set; }

        public string Coupon { get; set; }

        public ICollection<string> RefusedGiftIds { get; set; }

        public ICollection<ProductPromoEntry> CartPromoEntries { get; set; }

        public ICollection<ProductPromoEntry> PromoEntries { get; set; }

        public ProductPromoEntry PromoEntry { get; set; }

        public IDictionary<string, string> Attributes { get; set; }
    }
}