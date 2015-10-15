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
  public class VirtoCommerceStoreModuleWebModelFulfillmentCenter {
    
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
    /// Part of fulfillment center address, line1
    /// </summary>
    /// <value>Part of fulfillment center address, line1</value>
    [DataMember(Name="line1", EmitDefaultValue=false)]
    public string Line1 { get; set; }

    
    /// <summary>
    /// Part of fulfillment center address, line2
    /// </summary>
    /// <value>Part of fulfillment center address, line2</value>
    [DataMember(Name="line2", EmitDefaultValue=false)]
    public string Line2 { get; set; }

    
    /// <summary>
    /// Part of fulfillment center address, city
    /// </summary>
    /// <value>Part of fulfillment center address, city</value>
    [DataMember(Name="city", EmitDefaultValue=false)]
    public string City { get; set; }

    
    /// <summary>
    /// Part of fulfillment center address, state province
    /// </summary>
    /// <value>Part of fulfillment center address, state province</value>
    [DataMember(Name="stateProvince", EmitDefaultValue=false)]
    public string StateProvince { get; set; }

    
    /// <summary>
    /// Part of fulfillment center address, country code
    /// </summary>
    /// <value>Part of fulfillment center address, country code</value>
    [DataMember(Name="countryCode", EmitDefaultValue=false)]
    public string CountryCode { get; set; }

    
    /// <summary>
    /// Part of fulfillment center address, country name
    /// </summary>
    /// <value>Part of fulfillment center address, country name</value>
    [DataMember(Name="countryName", EmitDefaultValue=false)]
    public string CountryName { get; set; }

    
    /// <summary>
    /// Part of fulfillment center address, postal code
    /// </summary>
    /// <value>Part of fulfillment center address, postal code</value>
    [DataMember(Name="postalCode", EmitDefaultValue=false)]
    public string PostalCode { get; set; }

    
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
      sb.Append("class VirtoCommerceStoreModuleWebModelFulfillmentCenter {\n");
      
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
