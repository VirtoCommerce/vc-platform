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
  public class VirtoCommerceMerchandisingModuleWebModelStore {
    
    /// <summary>
    /// Gets or sets the value of store catalog id
    /// </summary>
    /// <value>Gets or sets the value of store catalog id</value>
    [DataMember(Name="catalog", EmitDefaultValue=false)]
    public string Catalog { get; set; }

    
    /// <summary>
    /// Gets or sets the country name where store is located
    /// </summary>
    /// <value>Gets or sets the country name where store is located</value>
    [DataMember(Name="country", EmitDefaultValue=false)]
    public string Country { get; set; }

    
    /// <summary>
    /// Gets or sets the collection of store currencies
    /// </summary>
    /// <value>Gets or sets the collection of store currencies</value>
    [DataMember(Name="currencies", EmitDefaultValue=false)]
    public List<string> Currencies { get; set; }

    
    /// <summary>
    /// Gets or sets the value of store default currency
    /// </summary>
    /// <value>Gets or sets the value of store default currency</value>
    [DataMember(Name="defaultCurrency", EmitDefaultValue=false)]
    public string DefaultCurrency { get; set; }

    
    /// <summary>
    /// Gets or sets the value of store default language
    /// </summary>
    /// <value>Gets or sets the value of store default language</value>
    [DataMember(Name="defaultLanguage", EmitDefaultValue=false)]
    public string DefaultLanguage { get; set; }

    
    /// <summary>
    /// Gets or sets the value of store description
    /// </summary>
    /// <value>Gets or sets the value of store description</value>
    [DataMember(Name="description", EmitDefaultValue=false)]
    public string Description { get; set; }

    
    /// <summary>
    /// Gets or sets the value of store id
    /// </summary>
    /// <value>Gets or sets the value of store id</value>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
    /// <summary>
    /// Gets or sets the collection of store language codes
    /// </summary>
    /// <value>Gets or sets the collection of store language codes</value>
    [DataMember(Name="languages", EmitDefaultValue=false)]
    public List<string> Languages { get; set; }

    
    /// <summary>
    /// Gets or sets the collection of linked stores
    /// </summary>
    /// <value>Gets or sets the collection of linked stores</value>
    [DataMember(Name="linkedStores", EmitDefaultValue=false)]
    public List<string> LinkedStores { get; set; }

    
    /// <summary>
    /// Gets or sets the value of store name
    /// </summary>
    /// <value>Gets or sets the value of store name</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or sets the name of region where store is located
    /// </summary>
    /// <value>Gets or sets the name of region where store is located</value>
    [DataMember(Name="region", EmitDefaultValue=false)]
    public string Region { get; set; }

    
    /// <summary>
    /// Gets or sets the value of store secure absolute URL
    /// </summary>
    /// <value>Gets or sets the value of store secure absolute URL</value>
    [DataMember(Name="secureUrl", EmitDefaultValue=false)]
    public string SecureUrl { get; set; }

    
    /// <summary>
    /// Gets or sets the collection of store SEO parameters
    /// </summary>
    /// <value>Gets or sets the collection of store SEO parameters</value>
    [DataMember(Name="seo", EmitDefaultValue=false)]
    public List<VirtoCommerceMerchandisingModuleWebModelSeoKeyword> Seo { get; set; }

    
    /// <summary>
    /// Gets or sets the dictionary of store settings
    /// </summary>
    /// <value>Gets or sets the dictionary of store settings</value>
    [DataMember(Name="settings", EmitDefaultValue=false)]
    public Dictionary<string, Object> Settings { get; set; }

    
    /// <summary>
    /// Gets or sets the numeric value of store current state
    /// </summary>
    /// <value>Gets or sets the numeric value of store current state</value>
    [DataMember(Name="storeState", EmitDefaultValue=false)]
    public int? StoreState { get; set; }

    
    /// <summary>
    /// Gets or sets the value of store time zone
    /// </summary>
    /// <value>Gets or sets the value of store time zone</value>
    [DataMember(Name="timeZone", EmitDefaultValue=false)]
    public string TimeZone { get; set; }

    
    /// <summary>
    /// Gets or sets the value of store absolute URL (HTTP)
    /// </summary>
    /// <value>Gets or sets the value of store absolute URL (HTTP)</value>
    [DataMember(Name="url", EmitDefaultValue=false)]
    public string Url { get; set; }

    
    /// <summary>
    /// Gets or sets the string value of store current state
    /// </summary>
    /// <value>Gets or sets the string value of store current state</value>
    [DataMember(Name="state", EmitDefaultValue=false)]
    public string State { get; set; }

    
    /// <summary>
    /// Gets or sets the collection of payment methods which are available for store
    /// </summary>
    /// <value>Gets or sets the collection of payment methods which are available for store</value>
    [DataMember(Name="paymentMethods", EmitDefaultValue=false)]
    public List<VirtoCommerceMerchandisingModuleWebModelPaymentMethod> PaymentMethods { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceMerchandisingModuleWebModelStore {\n");
      
      sb.Append("  Catalog: ").Append(Catalog).Append("\n");
      
      sb.Append("  Country: ").Append(Country).Append("\n");
      
      sb.Append("  Currencies: ").Append(Currencies).Append("\n");
      
      sb.Append("  DefaultCurrency: ").Append(DefaultCurrency).Append("\n");
      
      sb.Append("  DefaultLanguage: ").Append(DefaultLanguage).Append("\n");
      
      sb.Append("  Description: ").Append(Description).Append("\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  Languages: ").Append(Languages).Append("\n");
      
      sb.Append("  LinkedStores: ").Append(LinkedStores).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Region: ").Append(Region).Append("\n");
      
      sb.Append("  SecureUrl: ").Append(SecureUrl).Append("\n");
      
      sb.Append("  Seo: ").Append(Seo).Append("\n");
      
      sb.Append("  Settings: ").Append(Settings).Append("\n");
      
      sb.Append("  StoreState: ").Append(StoreState).Append("\n");
      
      sb.Append("  TimeZone: ").Append(TimeZone).Append("\n");
      
      sb.Append("  Url: ").Append(Url).Append("\n");
      
      sb.Append("  State: ").Append(State).Append("\n");
      
      sb.Append("  PaymentMethods: ").Append(PaymentMethods).Append("\n");
      
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
