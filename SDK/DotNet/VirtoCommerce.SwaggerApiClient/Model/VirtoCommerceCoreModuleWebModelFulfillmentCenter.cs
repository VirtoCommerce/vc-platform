using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// Represent fulfillment center information
  /// </summary>
  [DataContract]
  public class VirtoCommerceCoreModuleWebModelFulfillmentCenter {
    
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
    /// Gets or Sets MaxReleasesPerPickBatch
    /// </summary>
    [DataMember(Name="maxReleasesPerPickBatch", EmitDefaultValue=false)]
    public int? MaxReleasesPerPickBatch { get; set; }

    
    /// <summary>
    /// Gets or Sets PickDelay
    /// </summary>
    [DataMember(Name="pickDelay", EmitDefaultValue=false)]
    public int? PickDelay { get; set; }

    
    /// <summary>
    /// Gets or Sets DaytimePhoneNumber
    /// </summary>
    [DataMember(Name="daytimePhoneNumber", EmitDefaultValue=false)]
    public string DaytimePhoneNumber { get; set; }

    
    /// <summary>
    /// Gets or Sets Line1
    /// </summary>
    [DataMember(Name="line1", EmitDefaultValue=false)]
    public string Line1 { get; set; }

    
    /// <summary>
    /// Gets or Sets Line2
    /// </summary>
    [DataMember(Name="line2", EmitDefaultValue=false)]
    public string Line2 { get; set; }

    
    /// <summary>
    /// Gets or Sets City
    /// </summary>
    [DataMember(Name="city", EmitDefaultValue=false)]
    public string City { get; set; }

    
    /// <summary>
    /// Gets or Sets StateProvince
    /// </summary>
    [DataMember(Name="stateProvince", EmitDefaultValue=false)]
    public string StateProvince { get; set; }

    
    /// <summary>
    /// Gets or Sets CountryCode
    /// </summary>
    [DataMember(Name="countryCode", EmitDefaultValue=false)]
    public string CountryCode { get; set; }

    
    /// <summary>
    /// Gets or Sets CountryName
    /// </summary>
    [DataMember(Name="countryName", EmitDefaultValue=false)]
    public string CountryName { get; set; }

    
    /// <summary>
    /// Gets or Sets PostalCode
    /// </summary>
    [DataMember(Name="postalCode", EmitDefaultValue=false)]
    public string PostalCode { get; set; }

    
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
      sb.Append("class VirtoCommerceCoreModuleWebModelFulfillmentCenter {\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
      sb.Append("  Description: ").Append(Description).Append("\n");
      
      sb.Append("  MaxReleasesPerPickBatch: ").Append(MaxReleasesPerPickBatch).Append("\n");
      
      sb.Append("  PickDelay: ").Append(PickDelay).Append("\n");
      
      sb.Append("  DaytimePhoneNumber: ").Append(DaytimePhoneNumber).Append("\n");
      
      sb.Append("  Line1: ").Append(Line1).Append("\n");
      
      sb.Append("  Line2: ").Append(Line2).Append("\n");
      
      sb.Append("  City: ").Append(City).Append("\n");
      
      sb.Append("  StateProvince: ").Append(StateProvince).Append("\n");
      
      sb.Append("  CountryCode: ").Append(CountryCode).Append("\n");
      
      sb.Append("  CountryName: ").Append(CountryName).Append("\n");
      
      sb.Append("  PostalCode: ").Append(PostalCode).Append("\n");
      
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
