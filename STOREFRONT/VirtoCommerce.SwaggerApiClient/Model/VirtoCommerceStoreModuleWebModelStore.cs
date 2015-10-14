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
  public class VirtoCommerceStoreModuleWebModelStore {
    
    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    public string Description { get; set; }

    
    /// <summary>
    /// Url of store storefront, required
    /// </summary>
    /// <value>Url of store storefront, required</value>
    [DataMember(Name="url", EmitDefaultValue=false)]
    public string Url { get; set; }

    
    /// <summary>
    /// State of store
    /// </summary>
    /// <value>State of store</value>
    [DataMember(Name="storeState", EmitDefaultValue=false)]
    public string StoreState { get; set; }

    
    /// <summary>
    /// Gets or Sets TimeZone
    /// </summary>
    [DataMember(Name="timeZone", EmitDefaultValue=false)]
    public string TimeZone { get; set; }

    
    /// <summary>
    /// Gets or Sets Country
    /// </summary>
    [DataMember(Name="country", EmitDefaultValue=false)]
    public string Country { get; set; }

    
    /// <summary>
    /// Gets or Sets Region
    /// </summary>
    [DataMember(Name="region", EmitDefaultValue=false)]
    public string Region { get; set; }

    
    /// <summary>
    /// Default locale of store
    /// </summary>
    /// <value>Default locale of store</value>
    [DataMember(Name="defaultLanguage", EmitDefaultValue=false)]
    public string DefaultLanguage { get; set; }

    
    /// <summary>
    /// Default currency of store. Use ISO 4217 currency codes
    /// </summary>
    /// <value>Default currency of store. Use ISO 4217 currency codes</value>
    [DataMember(Name="defaultCurrency", EmitDefaultValue=false)]
    public string DefaultCurrency { get; set; }

    
    /// <summary>
    /// Product catalog id of store
    /// </summary>
    /// <value>Product catalog id of store</value>
    [DataMember(Name="catalog", EmitDefaultValue=false)]
    public string Catalog { get; set; }

    
    /// <summary>
    /// Gets or Sets CreditCardSavePolicy
    /// </summary>
    [DataMember(Name="creditCardSavePolicy", EmitDefaultValue=false)]
    public bool? CreditCardSavePolicy { get; set; }

    
    /// <summary>
    /// Secure url of store, must use https protocol, required
    /// </summary>
    /// <value>Secure url of store, must use https protocol, required</value>
    [DataMember(Name="secureUrl", EmitDefaultValue=false)]
    public string SecureUrl { get; set; }

    
    /// <summary>
    /// Contact email of store
    /// </summary>
    /// <value>Contact email of store</value>
    [DataMember(Name="email", EmitDefaultValue=false)]
    public string Email { get; set; }

    
    /// <summary>
    /// Administrator contact email of store
    /// </summary>
    /// <value>Administrator contact email of store</value>
    [DataMember(Name="adminEmail", EmitDefaultValue=false)]
    public string AdminEmail { get; set; }

    
    /// <summary>
    /// If true - store shows product with status out of stock
    /// </summary>
    /// <value>If true - store shows product with status out of stock</value>
    [DataMember(Name="displayOutOfStock", EmitDefaultValue=false)]
    public bool? DisplayOutOfStock { get; set; }

    
    /// <summary>
    /// Gets or Sets FulfillmentCenter
    /// </summary>
    [DataMember(Name="fulfillmentCenter", EmitDefaultValue=false)]
    public VirtoCommerceStoreModuleWebModelFulfillmentCenter FulfillmentCenter { get; set; }

    
    /// <summary>
    /// Gets or Sets ReturnsFulfillmentCenter
    /// </summary>
    [DataMember(Name="returnsFulfillmentCenter", EmitDefaultValue=false)]
    public VirtoCommerceStoreModuleWebModelFulfillmentCenter ReturnsFulfillmentCenter { get; set; }

    
    /// <summary>
    /// Gets or Sets Languages
    /// </summary>
    [DataMember(Name="languages", EmitDefaultValue=false)]
    public List<string> Languages { get; set; }

    
    /// <summary>
    /// Gets or Sets Currencies
    /// </summary>
    [DataMember(Name="currencies", EmitDefaultValue=false)]
    public List<string> Currencies { get; set; }

    
    /// <summary>
    /// Gets or Sets ObjectType
    /// </summary>
    [DataMember(Name="objectType", EmitDefaultValue=false)]
    public string ObjectType { get; set; }

    
    /// <summary>
    /// Gets or Sets DynamicProperties
    /// </summary>
    [DataMember(Name="dynamicProperties", EmitDefaultValue=false)]
    public List<VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty> DynamicProperties { get; set; }

    
    /// <summary>
    /// Gets or Sets PaymentMethods
    /// </summary>
    [DataMember(Name="paymentMethods", EmitDefaultValue=false)]
    public List<VirtoCommerceStoreModuleWebModelPaymentMethod> PaymentMethods { get; set; }

    
    /// <summary>
    /// Gets or Sets ShippingMethods
    /// </summary>
    [DataMember(Name="shippingMethods", EmitDefaultValue=false)]
    public List<VirtoCommerceStoreModuleWebModelShippingMethod> ShippingMethods { get; set; }

    
    /// <summary>
    /// Gets or Sets TaxProviders
    /// </summary>
    [DataMember(Name="taxProviders", EmitDefaultValue=false)]
    public List<VirtoCommerceStoreModuleWebModelTaxProvider> TaxProviders { get; set; }

    
    /// <summary>
    /// Gets or Sets SeoInfos
    /// </summary>
    [DataMember(Name="seoInfos", EmitDefaultValue=false)]
    public List<VirtoCommerceDomainCommerceModelSeoInfo> SeoInfos { get; set; }

    
    /// <summary>
    /// Gets or Sets SecurityScopes
    /// </summary>
    [DataMember(Name="securityScopes", EmitDefaultValue=false)]
    public List<string> SecurityScopes { get; set; }

    
    /// <summary>
    /// Gets or Sets CreatedDate
    /// </summary>
    [DataMember(Name="createdDate", EmitDefaultValue=false)]
    public DateTime? CreatedDate { get; set; }

    
    /// <summary>
    /// Gets or Sets ModifiedDate
    /// </summary>
    [DataMember(Name="modifiedDate", EmitDefaultValue=false)]
    public DateTime? ModifiedDate { get; set; }

    
    /// <summary>
    /// Gets or Sets CreatedBy
    /// </summary>
    [DataMember(Name="createdBy", EmitDefaultValue=false)]
    public string CreatedBy { get; set; }

    
    /// <summary>
    /// Gets or Sets ModifiedBy
    /// </summary>
    [DataMember(Name="modifiedBy", EmitDefaultValue=false)]
    public string ModifiedBy { get; set; }

    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceStoreModuleWebModelStore {\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Description: ").Append(Description).Append("\n");
      
      sb.Append("  Url: ").Append(Url).Append("\n");
      
      sb.Append("  StoreState: ").Append(StoreState).Append("\n");
      
      sb.Append("  TimeZone: ").Append(TimeZone).Append("\n");
      
      sb.Append("  Country: ").Append(Country).Append("\n");
      
      sb.Append("  Region: ").Append(Region).Append("\n");
      
      sb.Append("  DefaultLanguage: ").Append(DefaultLanguage).Append("\n");
      
      sb.Append("  DefaultCurrency: ").Append(DefaultCurrency).Append("\n");
      
      sb.Append("  Catalog: ").Append(Catalog).Append("\n");
      
      sb.Append("  CreditCardSavePolicy: ").Append(CreditCardSavePolicy).Append("\n");
      
      sb.Append("  SecureUrl: ").Append(SecureUrl).Append("\n");
      
      sb.Append("  Email: ").Append(Email).Append("\n");
      
      sb.Append("  AdminEmail: ").Append(AdminEmail).Append("\n");
      
      sb.Append("  DisplayOutOfStock: ").Append(DisplayOutOfStock).Append("\n");
      
      sb.Append("  FulfillmentCenter: ").Append(FulfillmentCenter).Append("\n");
      
      sb.Append("  ReturnsFulfillmentCenter: ").Append(ReturnsFulfillmentCenter).Append("\n");
      
      sb.Append("  Languages: ").Append(Languages).Append("\n");
      
      sb.Append("  Currencies: ").Append(Currencies).Append("\n");
      
      sb.Append("  ObjectType: ").Append(ObjectType).Append("\n");
      
      sb.Append("  DynamicProperties: ").Append(DynamicProperties).Append("\n");
      
      sb.Append("  PaymentMethods: ").Append(PaymentMethods).Append("\n");
      
      sb.Append("  ShippingMethods: ").Append(ShippingMethods).Append("\n");
      
      sb.Append("  TaxProviders: ").Append(TaxProviders).Append("\n");
      
      sb.Append("  SeoInfos: ").Append(SeoInfos).Append("\n");
      
      sb.Append("  SecurityScopes: ").Append(SecurityScopes).Append("\n");
      
      sb.Append("  CreatedDate: ").Append(CreatedDate).Append("\n");
      
      sb.Append("  ModifiedDate: ").Append(ModifiedDate).Append("\n");
      
      sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
      
      sb.Append("  ModifiedBy: ").Append(ModifiedBy).Append("\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
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
