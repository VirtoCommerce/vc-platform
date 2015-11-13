using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.Client.Model {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class VirtoCommerceDomainPricingModelPriceEvaluationContext {
    
    /// <summary>
    /// Gets or Sets StoreId
    /// </summary>
    [DataMember(Name="storeId", EmitDefaultValue=false)]
    public string StoreId { get; set; }

    
    /// <summary>
    /// Gets or Sets CatalogId
    /// </summary>
    [DataMember(Name="catalogId", EmitDefaultValue=false)]
    public string CatalogId { get; set; }

    
    /// <summary>
    /// Gets or Sets ProductIds
    /// </summary>
    [DataMember(Name="productIds", EmitDefaultValue=false)]
    public List<string> ProductIds { get; set; }

    
    /// <summary>
    /// Gets or Sets PricelistIds
    /// </summary>
    [DataMember(Name="pricelistIds", EmitDefaultValue=false)]
    public List<string> PricelistIds { get; set; }

    
    /// <summary>
    /// Gets or Sets Quantity
    /// </summary>
    [DataMember(Name="quantity", EmitDefaultValue=false)]
    public double? Quantity { get; set; }

    
    /// <summary>
    /// Gets or Sets CustomerId
    /// </summary>
    [DataMember(Name="customerId", EmitDefaultValue=false)]
    public string CustomerId { get; set; }

    
    /// <summary>
    /// Gets or Sets OrganizationId
    /// </summary>
    [DataMember(Name="organizationId", EmitDefaultValue=false)]
    public string OrganizationId { get; set; }

    
    /// <summary>
    /// Gets or Sets CertainDate
    /// </summary>
    [DataMember(Name="certainDate", EmitDefaultValue=false)]
    public DateTime? CertainDate { get; set; }

    
    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    public string Currency { get; set; }

    
    /// <summary>
    /// Gets or Sets Tags
    /// </summary>
    [DataMember(Name="tags", EmitDefaultValue=false)]
    public List<string> Tags { get; set; }

    
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
      sb.Append("class VirtoCommerceDomainPricingModelPriceEvaluationContext {\n");
      
      sb.Append("  StoreId: ").Append(StoreId).Append("\n");
      
      sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
      
      sb.Append("  ProductIds: ").Append(ProductIds).Append("\n");
      
      sb.Append("  PricelistIds: ").Append(PricelistIds).Append("\n");
      
      sb.Append("  Quantity: ").Append(Quantity).Append("\n");
      
      sb.Append("  CustomerId: ").Append(CustomerId).Append("\n");
      
      sb.Append("  OrganizationId: ").Append(OrganizationId).Append("\n");
      
      sb.Append("  CertainDate: ").Append(CertainDate).Append("\n");
      
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      
      sb.Append("  Tags: ").Append(Tags).Append("\n");
      
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
