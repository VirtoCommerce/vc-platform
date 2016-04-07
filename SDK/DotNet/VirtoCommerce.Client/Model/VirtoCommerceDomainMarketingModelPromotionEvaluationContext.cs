using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;



namespace VirtoCommerce.Client.Model
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceDomainMarketingModelPromotionEvaluationContext :  IEquatable<VirtoCommerceDomainMarketingModelPromotionEvaluationContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommerceDomainMarketingModelPromotionEvaluationContext" /> class.
        /// Initializes a new instance of the <see cref="VirtoCommerceDomainMarketingModelPromotionEvaluationContext" />class.
        /// </summary>
        /// <param name="RefusedGiftIds">RefusedGiftIds.</param>
        /// <param name="StoreId">StoreId.</param>
        /// <param name="Currency">Currency.</param>
        /// <param name="CustomerId">CustomerId.</param>
        /// <param name="IsRegisteredUser">IsRegisteredUser.</param>
        /// <param name="IsFirstTimeBuyer">IsFirstTimeBuyer.</param>
        /// <param name="IsEveryone">IsEveryone.</param>
        /// <param name="CartTotal">CartTotal.</param>
        /// <param name="ShipmentMethodCode">ShipmentMethodCode.</param>
        /// <param name="ShipmentMethodPrice">ShipmentMethodPrice.</param>
        /// <param name="AvailableShipmentMethodCodes">AvailableShipmentMethodCodes.</param>
        /// <param name="Coupon">Coupon.</param>
        /// <param name="CartPromoEntries">CartPromoEntries.</param>
        /// <param name="PromoEntries">PromoEntries.</param>
        /// <param name="PromoEntry">PromoEntry.</param>
        /// <param name="ContextObject">ContextObject.</param>
        /// <param name="GeoCity">GeoCity.</param>
        /// <param name="GeoState">GeoState.</param>
        /// <param name="GeoCountry">GeoCountry.</param>
        /// <param name="GeoContinent">GeoContinent.</param>
        /// <param name="GeoZipCode">GeoZipCode.</param>
        /// <param name="GeoConnectionType">GeoConnectionType.</param>
        /// <param name="GeoTimeZone">GeoTimeZone.</param>
        /// <param name="GeoIpRoutingType">GeoIpRoutingType.</param>
        /// <param name="GeoIspSecondLevel">GeoIspSecondLevel.</param>
        /// <param name="GeoIspTopLevel">GeoIspTopLevel.</param>
        /// <param name="ShopperAge">ShopperAge.</param>
        /// <param name="ShopperGender">ShopperGender.</param>
        /// <param name="Language">Language.</param>
        /// <param name="ShopperSearchedPhraseInStore">ShopperSearchedPhraseInStore.</param>
        /// <param name="ShopperSearchedPhraseOnInternet">ShopperSearchedPhraseOnInternet.</param>
        /// <param name="CurrentUrl">CurrentUrl.</param>
        /// <param name="ReferredUrl">ReferredUrl.</param>

        public VirtoCommerceDomainMarketingModelPromotionEvaluationContext(List<string> RefusedGiftIds = null, string StoreId = null, string Currency = null, string CustomerId = null, bool? IsRegisteredUser = null, bool? IsFirstTimeBuyer = null, bool? IsEveryone = null, double? CartTotal = null, string ShipmentMethodCode = null, double? ShipmentMethodPrice = null, List<string> AvailableShipmentMethodCodes = null, string Coupon = null, List<VirtoCommerceDomainMarketingModelProductPromoEntry> CartPromoEntries = null, List<VirtoCommerceDomainMarketingModelProductPromoEntry> PromoEntries = null, VirtoCommerceDomainMarketingModelProductPromoEntry PromoEntry = null, Object ContextObject = null, string GeoCity = null, string GeoState = null, string GeoCountry = null, string GeoContinent = null, string GeoZipCode = null, string GeoConnectionType = null, string GeoTimeZone = null, string GeoIpRoutingType = null, string GeoIspSecondLevel = null, string GeoIspTopLevel = null, int? ShopperAge = null, string ShopperGender = null, string Language = null, string ShopperSearchedPhraseInStore = null, string ShopperSearchedPhraseOnInternet = null, string CurrentUrl = null, string ReferredUrl = null)
        {
            this.RefusedGiftIds = RefusedGiftIds;
            this.StoreId = StoreId;
            this.Currency = Currency;
            this.CustomerId = CustomerId;
            this.IsRegisteredUser = IsRegisteredUser;
            this.IsFirstTimeBuyer = IsFirstTimeBuyer;
            this.IsEveryone = IsEveryone;
            this.CartTotal = CartTotal;
            this.ShipmentMethodCode = ShipmentMethodCode;
            this.ShipmentMethodPrice = ShipmentMethodPrice;
            this.AvailableShipmentMethodCodes = AvailableShipmentMethodCodes;
            this.Coupon = Coupon;
            this.CartPromoEntries = CartPromoEntries;
            this.PromoEntries = PromoEntries;
            this.PromoEntry = PromoEntry;
            this.ContextObject = ContextObject;
            this.GeoCity = GeoCity;
            this.GeoState = GeoState;
            this.GeoCountry = GeoCountry;
            this.GeoContinent = GeoContinent;
            this.GeoZipCode = GeoZipCode;
            this.GeoConnectionType = GeoConnectionType;
            this.GeoTimeZone = GeoTimeZone;
            this.GeoIpRoutingType = GeoIpRoutingType;
            this.GeoIspSecondLevel = GeoIspSecondLevel;
            this.GeoIspTopLevel = GeoIspTopLevel;
            this.ShopperAge = ShopperAge;
            this.ShopperGender = ShopperGender;
            this.Language = Language;
            this.ShopperSearchedPhraseInStore = ShopperSearchedPhraseInStore;
            this.ShopperSearchedPhraseOnInternet = ShopperSearchedPhraseOnInternet;
            this.CurrentUrl = CurrentUrl;
            this.ReferredUrl = ReferredUrl;
            
        }

        /// <summary>
        /// Gets or Sets RefusedGiftIds
        /// </summary>
        [DataMember(Name="refusedGiftIds", EmitDefaultValue=false)]
        public List<string> RefusedGiftIds { get; set; }

        /// <summary>
        /// Gets or Sets StoreId
        /// </summary>
        [DataMember(Name="storeId", EmitDefaultValue=false)]
        public string StoreId { get; set; }

        /// <summary>
        /// Gets or Sets Currency
        /// </summary>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or Sets CustomerId
        /// </summary>
        [DataMember(Name="customerId", EmitDefaultValue=false)]
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or Sets IsRegisteredUser
        /// </summary>
        [DataMember(Name="isRegisteredUser", EmitDefaultValue=false)]
        public bool? IsRegisteredUser { get; set; }

        /// <summary>
        /// Gets or Sets IsFirstTimeBuyer
        /// </summary>
        [DataMember(Name="isFirstTimeBuyer", EmitDefaultValue=false)]
        public bool? IsFirstTimeBuyer { get; set; }

        /// <summary>
        /// Gets or Sets IsEveryone
        /// </summary>
        [DataMember(Name="isEveryone", EmitDefaultValue=false)]
        public bool? IsEveryone { get; set; }

        /// <summary>
        /// Gets or Sets CartTotal
        /// </summary>
        [DataMember(Name="cartTotal", EmitDefaultValue=false)]
        public double? CartTotal { get; set; }

        /// <summary>
        /// Gets or Sets ShipmentMethodCode
        /// </summary>
        [DataMember(Name="shipmentMethodCode", EmitDefaultValue=false)]
        public string ShipmentMethodCode { get; set; }

        /// <summary>
        /// Gets or Sets ShipmentMethodPrice
        /// </summary>
        [DataMember(Name="shipmentMethodPrice", EmitDefaultValue=false)]
        public double? ShipmentMethodPrice { get; set; }

        /// <summary>
        /// Gets or Sets AvailableShipmentMethodCodes
        /// </summary>
        [DataMember(Name="availableShipmentMethodCodes", EmitDefaultValue=false)]
        public List<string> AvailableShipmentMethodCodes { get; set; }

        /// <summary>
        /// Gets or Sets Coupon
        /// </summary>
        [DataMember(Name="coupon", EmitDefaultValue=false)]
        public string Coupon { get; set; }

        /// <summary>
        /// Gets or Sets CartPromoEntries
        /// </summary>
        [DataMember(Name="cartPromoEntries", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainMarketingModelProductPromoEntry> CartPromoEntries { get; set; }

        /// <summary>
        /// Gets or Sets PromoEntries
        /// </summary>
        [DataMember(Name="promoEntries", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainMarketingModelProductPromoEntry> PromoEntries { get; set; }

        /// <summary>
        /// Gets or Sets PromoEntry
        /// </summary>
        [DataMember(Name="promoEntry", EmitDefaultValue=false)]
        public VirtoCommerceDomainMarketingModelProductPromoEntry PromoEntry { get; set; }

        /// <summary>
        /// Gets or Sets ContextObject
        /// </summary>
        [DataMember(Name="contextObject", EmitDefaultValue=false)]
        public Object ContextObject { get; set; }

        /// <summary>
        /// Gets or Sets GeoCity
        /// </summary>
        [DataMember(Name="geoCity", EmitDefaultValue=false)]
        public string GeoCity { get; set; }

        /// <summary>
        /// Gets or Sets GeoState
        /// </summary>
        [DataMember(Name="geoState", EmitDefaultValue=false)]
        public string GeoState { get; set; }

        /// <summary>
        /// Gets or Sets GeoCountry
        /// </summary>
        [DataMember(Name="geoCountry", EmitDefaultValue=false)]
        public string GeoCountry { get; set; }

        /// <summary>
        /// Gets or Sets GeoContinent
        /// </summary>
        [DataMember(Name="geoContinent", EmitDefaultValue=false)]
        public string GeoContinent { get; set; }

        /// <summary>
        /// Gets or Sets GeoZipCode
        /// </summary>
        [DataMember(Name="geoZipCode", EmitDefaultValue=false)]
        public string GeoZipCode { get; set; }

        /// <summary>
        /// Gets or Sets GeoConnectionType
        /// </summary>
        [DataMember(Name="geoConnectionType", EmitDefaultValue=false)]
        public string GeoConnectionType { get; set; }

        /// <summary>
        /// Gets or Sets GeoTimeZone
        /// </summary>
        [DataMember(Name="geoTimeZone", EmitDefaultValue=false)]
        public string GeoTimeZone { get; set; }

        /// <summary>
        /// Gets or Sets GeoIpRoutingType
        /// </summary>
        [DataMember(Name="geoIpRoutingType", EmitDefaultValue=false)]
        public string GeoIpRoutingType { get; set; }

        /// <summary>
        /// Gets or Sets GeoIspSecondLevel
        /// </summary>
        [DataMember(Name="geoIspSecondLevel", EmitDefaultValue=false)]
        public string GeoIspSecondLevel { get; set; }

        /// <summary>
        /// Gets or Sets GeoIspTopLevel
        /// </summary>
        [DataMember(Name="geoIspTopLevel", EmitDefaultValue=false)]
        public string GeoIspTopLevel { get; set; }

        /// <summary>
        /// Gets or Sets ShopperAge
        /// </summary>
        [DataMember(Name="shopperAge", EmitDefaultValue=false)]
        public int? ShopperAge { get; set; }

        /// <summary>
        /// Gets or Sets ShopperGender
        /// </summary>
        [DataMember(Name="shopperGender", EmitDefaultValue=false)]
        public string ShopperGender { get; set; }

        /// <summary>
        /// Gets or Sets Language
        /// </summary>
        [DataMember(Name="language", EmitDefaultValue=false)]
        public string Language { get; set; }

        /// <summary>
        /// Gets or Sets ShopperSearchedPhraseInStore
        /// </summary>
        [DataMember(Name="shopperSearchedPhraseInStore", EmitDefaultValue=false)]
        public string ShopperSearchedPhraseInStore { get; set; }

        /// <summary>
        /// Gets or Sets ShopperSearchedPhraseOnInternet
        /// </summary>
        [DataMember(Name="shopperSearchedPhraseOnInternet", EmitDefaultValue=false)]
        public string ShopperSearchedPhraseOnInternet { get; set; }

        /// <summary>
        /// Gets or Sets CurrentUrl
        /// </summary>
        [DataMember(Name="currentUrl", EmitDefaultValue=false)]
        public string CurrentUrl { get; set; }

        /// <summary>
        /// Gets or Sets ReferredUrl
        /// </summary>
        [DataMember(Name="referredUrl", EmitDefaultValue=false)]
        public string ReferredUrl { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceDomainMarketingModelPromotionEvaluationContext {\n");
            sb.Append("  RefusedGiftIds: ").Append(RefusedGiftIds).Append("\n");
            sb.Append("  StoreId: ").Append(StoreId).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  CustomerId: ").Append(CustomerId).Append("\n");
            sb.Append("  IsRegisteredUser: ").Append(IsRegisteredUser).Append("\n");
            sb.Append("  IsFirstTimeBuyer: ").Append(IsFirstTimeBuyer).Append("\n");
            sb.Append("  IsEveryone: ").Append(IsEveryone).Append("\n");
            sb.Append("  CartTotal: ").Append(CartTotal).Append("\n");
            sb.Append("  ShipmentMethodCode: ").Append(ShipmentMethodCode).Append("\n");
            sb.Append("  ShipmentMethodPrice: ").Append(ShipmentMethodPrice).Append("\n");
            sb.Append("  AvailableShipmentMethodCodes: ").Append(AvailableShipmentMethodCodes).Append("\n");
            sb.Append("  Coupon: ").Append(Coupon).Append("\n");
            sb.Append("  CartPromoEntries: ").Append(CartPromoEntries).Append("\n");
            sb.Append("  PromoEntries: ").Append(PromoEntries).Append("\n");
            sb.Append("  PromoEntry: ").Append(PromoEntry).Append("\n");
            sb.Append("  ContextObject: ").Append(ContextObject).Append("\n");
            sb.Append("  GeoCity: ").Append(GeoCity).Append("\n");
            sb.Append("  GeoState: ").Append(GeoState).Append("\n");
            sb.Append("  GeoCountry: ").Append(GeoCountry).Append("\n");
            sb.Append("  GeoContinent: ").Append(GeoContinent).Append("\n");
            sb.Append("  GeoZipCode: ").Append(GeoZipCode).Append("\n");
            sb.Append("  GeoConnectionType: ").Append(GeoConnectionType).Append("\n");
            sb.Append("  GeoTimeZone: ").Append(GeoTimeZone).Append("\n");
            sb.Append("  GeoIpRoutingType: ").Append(GeoIpRoutingType).Append("\n");
            sb.Append("  GeoIspSecondLevel: ").Append(GeoIspSecondLevel).Append("\n");
            sb.Append("  GeoIspTopLevel: ").Append(GeoIspTopLevel).Append("\n");
            sb.Append("  ShopperAge: ").Append(ShopperAge).Append("\n");
            sb.Append("  ShopperGender: ").Append(ShopperGender).Append("\n");
            sb.Append("  Language: ").Append(Language).Append("\n");
            sb.Append("  ShopperSearchedPhraseInStore: ").Append(ShopperSearchedPhraseInStore).Append("\n");
            sb.Append("  ShopperSearchedPhraseOnInternet: ").Append(ShopperSearchedPhraseOnInternet).Append("\n");
            sb.Append("  CurrentUrl: ").Append(CurrentUrl).Append("\n");
            sb.Append("  ReferredUrl: ").Append(ReferredUrl).Append("\n");
            
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            return this.Equals(obj as VirtoCommerceDomainMarketingModelPromotionEvaluationContext);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainMarketingModelPromotionEvaluationContext instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainMarketingModelPromotionEvaluationContext to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainMarketingModelPromotionEvaluationContext other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.RefusedGiftIds == other.RefusedGiftIds ||
                    this.RefusedGiftIds != null &&
                    this.RefusedGiftIds.SequenceEqual(other.RefusedGiftIds)
                ) && 
                (
                    this.StoreId == other.StoreId ||
                    this.StoreId != null &&
                    this.StoreId.Equals(other.StoreId)
                ) && 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
                ) && 
                (
                    this.CustomerId == other.CustomerId ||
                    this.CustomerId != null &&
                    this.CustomerId.Equals(other.CustomerId)
                ) && 
                (
                    this.IsRegisteredUser == other.IsRegisteredUser ||
                    this.IsRegisteredUser != null &&
                    this.IsRegisteredUser.Equals(other.IsRegisteredUser)
                ) && 
                (
                    this.IsFirstTimeBuyer == other.IsFirstTimeBuyer ||
                    this.IsFirstTimeBuyer != null &&
                    this.IsFirstTimeBuyer.Equals(other.IsFirstTimeBuyer)
                ) && 
                (
                    this.IsEveryone == other.IsEveryone ||
                    this.IsEveryone != null &&
                    this.IsEveryone.Equals(other.IsEveryone)
                ) && 
                (
                    this.CartTotal == other.CartTotal ||
                    this.CartTotal != null &&
                    this.CartTotal.Equals(other.CartTotal)
                ) && 
                (
                    this.ShipmentMethodCode == other.ShipmentMethodCode ||
                    this.ShipmentMethodCode != null &&
                    this.ShipmentMethodCode.Equals(other.ShipmentMethodCode)
                ) && 
                (
                    this.ShipmentMethodPrice == other.ShipmentMethodPrice ||
                    this.ShipmentMethodPrice != null &&
                    this.ShipmentMethodPrice.Equals(other.ShipmentMethodPrice)
                ) && 
                (
                    this.AvailableShipmentMethodCodes == other.AvailableShipmentMethodCodes ||
                    this.AvailableShipmentMethodCodes != null &&
                    this.AvailableShipmentMethodCodes.SequenceEqual(other.AvailableShipmentMethodCodes)
                ) && 
                (
                    this.Coupon == other.Coupon ||
                    this.Coupon != null &&
                    this.Coupon.Equals(other.Coupon)
                ) && 
                (
                    this.CartPromoEntries == other.CartPromoEntries ||
                    this.CartPromoEntries != null &&
                    this.CartPromoEntries.SequenceEqual(other.CartPromoEntries)
                ) && 
                (
                    this.PromoEntries == other.PromoEntries ||
                    this.PromoEntries != null &&
                    this.PromoEntries.SequenceEqual(other.PromoEntries)
                ) && 
                (
                    this.PromoEntry == other.PromoEntry ||
                    this.PromoEntry != null &&
                    this.PromoEntry.Equals(other.PromoEntry)
                ) && 
                (
                    this.ContextObject == other.ContextObject ||
                    this.ContextObject != null &&
                    this.ContextObject.Equals(other.ContextObject)
                ) && 
                (
                    this.GeoCity == other.GeoCity ||
                    this.GeoCity != null &&
                    this.GeoCity.Equals(other.GeoCity)
                ) && 
                (
                    this.GeoState == other.GeoState ||
                    this.GeoState != null &&
                    this.GeoState.Equals(other.GeoState)
                ) && 
                (
                    this.GeoCountry == other.GeoCountry ||
                    this.GeoCountry != null &&
                    this.GeoCountry.Equals(other.GeoCountry)
                ) && 
                (
                    this.GeoContinent == other.GeoContinent ||
                    this.GeoContinent != null &&
                    this.GeoContinent.Equals(other.GeoContinent)
                ) && 
                (
                    this.GeoZipCode == other.GeoZipCode ||
                    this.GeoZipCode != null &&
                    this.GeoZipCode.Equals(other.GeoZipCode)
                ) && 
                (
                    this.GeoConnectionType == other.GeoConnectionType ||
                    this.GeoConnectionType != null &&
                    this.GeoConnectionType.Equals(other.GeoConnectionType)
                ) && 
                (
                    this.GeoTimeZone == other.GeoTimeZone ||
                    this.GeoTimeZone != null &&
                    this.GeoTimeZone.Equals(other.GeoTimeZone)
                ) && 
                (
                    this.GeoIpRoutingType == other.GeoIpRoutingType ||
                    this.GeoIpRoutingType != null &&
                    this.GeoIpRoutingType.Equals(other.GeoIpRoutingType)
                ) && 
                (
                    this.GeoIspSecondLevel == other.GeoIspSecondLevel ||
                    this.GeoIspSecondLevel != null &&
                    this.GeoIspSecondLevel.Equals(other.GeoIspSecondLevel)
                ) && 
                (
                    this.GeoIspTopLevel == other.GeoIspTopLevel ||
                    this.GeoIspTopLevel != null &&
                    this.GeoIspTopLevel.Equals(other.GeoIspTopLevel)
                ) && 
                (
                    this.ShopperAge == other.ShopperAge ||
                    this.ShopperAge != null &&
                    this.ShopperAge.Equals(other.ShopperAge)
                ) && 
                (
                    this.ShopperGender == other.ShopperGender ||
                    this.ShopperGender != null &&
                    this.ShopperGender.Equals(other.ShopperGender)
                ) && 
                (
                    this.Language == other.Language ||
                    this.Language != null &&
                    this.Language.Equals(other.Language)
                ) && 
                (
                    this.ShopperSearchedPhraseInStore == other.ShopperSearchedPhraseInStore ||
                    this.ShopperSearchedPhraseInStore != null &&
                    this.ShopperSearchedPhraseInStore.Equals(other.ShopperSearchedPhraseInStore)
                ) && 
                (
                    this.ShopperSearchedPhraseOnInternet == other.ShopperSearchedPhraseOnInternet ||
                    this.ShopperSearchedPhraseOnInternet != null &&
                    this.ShopperSearchedPhraseOnInternet.Equals(other.ShopperSearchedPhraseOnInternet)
                ) && 
                (
                    this.CurrentUrl == other.CurrentUrl ||
                    this.CurrentUrl != null &&
                    this.CurrentUrl.Equals(other.CurrentUrl)
                ) && 
                (
                    this.ReferredUrl == other.ReferredUrl ||
                    this.ReferredUrl != null &&
                    this.ReferredUrl.Equals(other.ReferredUrl)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)
                
                if (this.RefusedGiftIds != null)
                    hash = hash * 59 + this.RefusedGiftIds.GetHashCode();
                
                if (this.StoreId != null)
                    hash = hash * 59 + this.StoreId.GetHashCode();
                
                if (this.Currency != null)
                    hash = hash * 59 + this.Currency.GetHashCode();
                
                if (this.CustomerId != null)
                    hash = hash * 59 + this.CustomerId.GetHashCode();
                
                if (this.IsRegisteredUser != null)
                    hash = hash * 59 + this.IsRegisteredUser.GetHashCode();
                
                if (this.IsFirstTimeBuyer != null)
                    hash = hash * 59 + this.IsFirstTimeBuyer.GetHashCode();
                
                if (this.IsEveryone != null)
                    hash = hash * 59 + this.IsEveryone.GetHashCode();
                
                if (this.CartTotal != null)
                    hash = hash * 59 + this.CartTotal.GetHashCode();
                
                if (this.ShipmentMethodCode != null)
                    hash = hash * 59 + this.ShipmentMethodCode.GetHashCode();
                
                if (this.ShipmentMethodPrice != null)
                    hash = hash * 59 + this.ShipmentMethodPrice.GetHashCode();
                
                if (this.AvailableShipmentMethodCodes != null)
                    hash = hash * 59 + this.AvailableShipmentMethodCodes.GetHashCode();
                
                if (this.Coupon != null)
                    hash = hash * 59 + this.Coupon.GetHashCode();
                
                if (this.CartPromoEntries != null)
                    hash = hash * 59 + this.CartPromoEntries.GetHashCode();
                
                if (this.PromoEntries != null)
                    hash = hash * 59 + this.PromoEntries.GetHashCode();
                
                if (this.PromoEntry != null)
                    hash = hash * 59 + this.PromoEntry.GetHashCode();
                
                if (this.ContextObject != null)
                    hash = hash * 59 + this.ContextObject.GetHashCode();
                
                if (this.GeoCity != null)
                    hash = hash * 59 + this.GeoCity.GetHashCode();
                
                if (this.GeoState != null)
                    hash = hash * 59 + this.GeoState.GetHashCode();
                
                if (this.GeoCountry != null)
                    hash = hash * 59 + this.GeoCountry.GetHashCode();
                
                if (this.GeoContinent != null)
                    hash = hash * 59 + this.GeoContinent.GetHashCode();
                
                if (this.GeoZipCode != null)
                    hash = hash * 59 + this.GeoZipCode.GetHashCode();
                
                if (this.GeoConnectionType != null)
                    hash = hash * 59 + this.GeoConnectionType.GetHashCode();
                
                if (this.GeoTimeZone != null)
                    hash = hash * 59 + this.GeoTimeZone.GetHashCode();
                
                if (this.GeoIpRoutingType != null)
                    hash = hash * 59 + this.GeoIpRoutingType.GetHashCode();
                
                if (this.GeoIspSecondLevel != null)
                    hash = hash * 59 + this.GeoIspSecondLevel.GetHashCode();
                
                if (this.GeoIspTopLevel != null)
                    hash = hash * 59 + this.GeoIspTopLevel.GetHashCode();
                
                if (this.ShopperAge != null)
                    hash = hash * 59 + this.ShopperAge.GetHashCode();
                
                if (this.ShopperGender != null)
                    hash = hash * 59 + this.ShopperGender.GetHashCode();
                
                if (this.Language != null)
                    hash = hash * 59 + this.Language.GetHashCode();
                
                if (this.ShopperSearchedPhraseInStore != null)
                    hash = hash * 59 + this.ShopperSearchedPhraseInStore.GetHashCode();
                
                if (this.ShopperSearchedPhraseOnInternet != null)
                    hash = hash * 59 + this.ShopperSearchedPhraseOnInternet.GetHashCode();
                
                if (this.CurrentUrl != null)
                    hash = hash * 59 + this.CurrentUrl.GetHashCode();
                
                if (this.ReferredUrl != null)
                    hash = hash * 59 + this.ReferredUrl.GetHashCode();
                
                return hash;
            }
        }

    }


}
