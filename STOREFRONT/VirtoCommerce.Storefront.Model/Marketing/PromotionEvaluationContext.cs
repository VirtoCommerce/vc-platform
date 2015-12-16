using System.Collections.Generic;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Marketing
{
    /// <summary>
    /// Represents context object for promotion evaluation
    /// </summary>
    public class PromotionEvaluationContext
    {
        public PromotionEvaluationContext()
        {
            AvailableShipmentMethodCodes = new List<string>();
            CartPromoEntries = new List<PromotionProductEntry>();
            PromoEntries = new List<PromotionProductEntry>();
            RefusedGiftIds = new List<string>();
        }

        public ICollection<string> AvailableShipmentMethodCodes { get; set; }

        public ICollection<PromotionProductEntry> CartPromoEntries { get; set; }

        public Money CartTotal { get; set; }

        public object ContextObject { get; set; }

        public string Coupon { get; set; }

        public Currency Currency { get; set; }

        public string CurrentUrl { get; set; }

        public string CustomerId { get; set; }

        public string GeoCity { get; set; }

        public string GeoConnectionType { get; set; }

        public string GeoContinent { get; set; }

        public string GeoCountry { get; set; }

        public string GeoIpRoutingType { get; set; }

        public string GeoIspSecondLevel { get; set; }

        public string GeoIspTopLevel { get; set; }

        public string GeoState { get; set; }

        public string GeoTimeZone { get; set; }

        public string GeoZipCode { get; set; }

        public bool? IsEveryone { get; set; }

        public bool? IsFirstTimeBuyer { get; set; }

        public bool IsRegisteredUser { get; set; }

        public Language Language { get; set; }

        public ICollection<PromotionProductEntry> PromoEntries { get; set; }

        public PromotionProductEntry PromoEntry { get; set; }

        public string ReferredUrl { get; set; }

        public ICollection<string> RefusedGiftIds { get; set; }

        public string ShipmentMethodCode { get; set; }

        public Money ShipmentMethodPrice { get; set; }

        public int? ShopperAge { get; set; }

        public string ShopperGender { get; set; }

        public string ShopperSearchedPhraseInStore { get; set; }

        public string ShopperSearchedPhraseOnInternet { get; set; }

        public string StoreId { get; set; }
    }
}