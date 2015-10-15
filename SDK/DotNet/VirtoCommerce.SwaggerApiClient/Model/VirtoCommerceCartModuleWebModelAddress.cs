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
  public class VirtoCommerceCartModuleWebModelAddress {
    
    /// <summary>
    /// Gets or sets the value of address type
    /// </summary>
    /// <value>Gets or sets the value of address type</value>
    [DataMember(Name="type", EmitDefaultValue=false)]
    public string Type { get; set; }

    
    /// <summary>
    /// Gets or sets the value of organization name
    /// </summary>
    /// <value>Gets or sets the value of organization name</value>
    [DataMember(Name="organization", EmitDefaultValue=false)]
    public string Organization { get; set; }

    
    /// <summary>
    /// Gets or sets the value of country code
    /// </summary>
    /// <value>Gets or sets the value of country code</value>
    [DataMember(Name="countryCode", EmitDefaultValue=false)]
    public string CountryCode { get; set; }

    
    /// <summary>
    /// Gets or sets the value of country name
    /// </summary>
    /// <value>Gets or sets the value of country name</value>
    [DataMember(Name="countryName", EmitDefaultValue=false)]
    public string CountryName { get; set; }

    
    /// <summary>
    /// Gets or sets the value of city name
    /// </summary>
    /// <value>Gets or sets the value of city name</value>
    [DataMember(Name="city", EmitDefaultValue=false)]
    public string City { get; set; }

    
    /// <summary>
    /// Gets or sets the value of postal code
    /// </summary>
    /// <value>Gets or sets the value of postal code</value>
    [DataMember(Name="postalCode", EmitDefaultValue=false)]
    public string PostalCode { get; set; }

    
    /// <summary>
    /// Gets or sets the value of zip code
    /// </summary>
    /// <value>Gets or sets the value of zip code</value>
    [DataMember(Name="zip", EmitDefaultValue=false)]
    public string Zip { get; set; }

    
    /// <summary>
    /// Gets or sets the value of address line 1
    /// </summary>
    /// <value>Gets or sets the value of address line 1</value>
    [DataMember(Name="line1", EmitDefaultValue=false)]
    public string Line1 { get; set; }

    
    /// <summary>
    /// Gets or sets the value of address line 2
    /// </summary>
    /// <value>Gets or sets the value of address line 2</value>
    [DataMember(Name="line2", EmitDefaultValue=false)]
    public string Line2 { get; set; }

    
    /// <summary>
    /// Gets or sets the value of region code
    /// </summary>
    /// <value>Gets or sets the value of region code</value>
    [DataMember(Name="regionId", EmitDefaultValue=false)]
    public string RegionId { get; set; }

    
    /// <summary>
    /// Gets or sets the value of region name
    /// </summary>
    /// <value>Gets or sets the value of region name</value>
    [DataMember(Name="regionName", EmitDefaultValue=false)]
    public string RegionName { get; set; }

    
    /// <summary>
    /// Gets or sets the value of first name
    /// </summary>
    /// <value>Gets or sets the value of first name</value>
    [DataMember(Name="firstName", EmitDefaultValue=false)]
    public string FirstName { get; set; }

    
    /// <summary>
    /// Gets or sets the value of middle name
    /// </summary>
    /// <value>Gets or sets the value of middle name</value>
    [DataMember(Name="middleName", EmitDefaultValue=false)]
    public string MiddleName { get; set; }

    
    /// <summary>
    /// Gets or sets the value of last name
    /// </summary>
    /// <value>Gets or sets the value of last name</value>
    [DataMember(Name="lastName", EmitDefaultValue=false)]
    public string LastName { get; set; }

    
    /// <summary>
    /// Gets or sets the value of phone number
    /// </summary>
    /// <value>Gets or sets the value of phone number</value>
    [DataMember(Name="phone", EmitDefaultValue=false)]
    public string Phone { get; set; }

    
    /// <summary>
    /// Gets or sets the value of E-mail address
    /// </summary>
    /// <value>Gets or sets the value of E-mail address</value>
    [DataMember(Name="email", EmitDefaultValue=false)]
    public string Email { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCartModuleWebModelAddress {\n");
      
      sb.Append("  Type: ").Append(Type).Append("\n");
      
      sb.Append("  Organization: ").Append(Organization).Append("\n");
      
      sb.Append("  CountryCode: ").Append(CountryCode).Append("\n");
      
      sb.Append("  CountryName: ").Append(CountryName).Append("\n");
      
      sb.Append("  City: ").Append(City).Append("\n");
      
      sb.Append("  PostalCode: ").Append(PostalCode).Append("\n");
      
      sb.Append("  Zip: ").Append(Zip).Append("\n");
      
      sb.Append("  Line1: ").Append(Line1).Append("\n");
      
      sb.Append("  Line2: ").Append(Line2).Append("\n");
      
      sb.Append("  RegionId: ").Append(RegionId).Append("\n");
      
      sb.Append("  RegionName: ").Append(RegionName).Append("\n");
      
      sb.Append("  FirstName: ").Append(FirstName).Append("\n");
      
      sb.Append("  MiddleName: ").Append(MiddleName).Append("\n");
      
      sb.Append("  LastName: ").Append(LastName).Append("\n");
      
      sb.Append("  Phone: ").Append(Phone).Append("\n");
      
      sb.Append("  Email: ").Append(Email).Append("\n");
      
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
