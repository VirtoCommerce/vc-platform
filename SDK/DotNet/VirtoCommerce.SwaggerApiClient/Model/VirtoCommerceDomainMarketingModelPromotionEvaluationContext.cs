using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class VirtoCommerceDomainMarketingModelPromotionEvaluationContext {
    
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
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
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
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}


}
