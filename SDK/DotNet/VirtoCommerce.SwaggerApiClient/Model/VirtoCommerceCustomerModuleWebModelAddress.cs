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
  public class VirtoCommerceCustomerModuleWebModelAddress {
    
    /// <summary>
    /// Type of address.
    /// </summary>
    /// <value>Type of address.</value>
    [DataMember(Name="addressType", EmitDefaultValue=false)]
    public string AddressType { get; set; }

    
    /// <summary>
    /// Not documented
    /// </summary>
    /// <value>Not documented</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    
    /// <summary>
    /// Not documented
    /// </summary>
    /// <value>Not documented</value>
    [DataMember(Name="organization", EmitDefaultValue=false)]
    public string Organization { get; set; }

    
    /// <summary>
    /// ISO 3166-1 alpha-3
    /// </summary>
    /// <value>ISO 3166-1 alpha-3</value>
    [DataMember(Name="countryCode", EmitDefaultValue=false)]
    public string CountryCode { get; set; }

    
    /// <summary>
    /// Gets or Sets CountryName
    /// </summary>
    [DataMember(Name="countryName", EmitDefaultValue=false)]
    public string CountryName { get; set; }

    
    /// <summary>
    /// Gets or Sets City
    /// </summary>
    [DataMember(Name="city", EmitDefaultValue=false)]
    public string City { get; set; }

    
    /// <summary>
    /// Gets or Sets PostalCode
    /// </summary>
    [DataMember(Name="postalCode", EmitDefaultValue=false)]
    public string PostalCode { get; set; }

    
    /// <summary>
    /// Gets or Sets Zip
    /// </summary>
    [DataMember(Name="zip", EmitDefaultValue=false)]
    public string Zip { get; set; }

    
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
    /// Code of Region (AL - Alabama)
    /// </summary>
    /// <value>Code of Region (AL - Alabama)</value>
    [DataMember(Name="regionId", EmitDefaultValue=false)]
    public string RegionId { get; set; }

    
    /// <summary>
    /// Gets or Sets RegionName
    /// </summary>
    [DataMember(Name="regionName", EmitDefaultValue=false)]
    public string RegionName { get; set; }

    
    /// <summary>
    /// Gets or Sets FirstName
    /// </summary>
    [DataMember(Name="firstName", EmitDefaultValue=false)]
    public string FirstName { get; set; }

    
    /// <summary>
    /// Gets or Sets MiddleName
    /// </summary>
    [DataMember(Name="middleName", EmitDefaultValue=false)]
    public string MiddleName { get; set; }

    
    /// <summary>
    /// Gets or Sets LastName
    /// </summary>
    [DataMember(Name="lastName", EmitDefaultValue=false)]
    public string LastName { get; set; }

    
    /// <summary>
    /// Gets or Sets Phone
    /// </summary>
    [DataMember(Name="phone", EmitDefaultValue=false)]
    public string Phone { get; set; }

    
    /// <summary>
    /// Gets or Sets Email
    /// </summary>
    [DataMember(Name="email", EmitDefaultValue=false)]
    public string Email { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceCustomerModuleWebModelAddress {\n");
      
      sb.Append("  AddressType: ").Append(AddressType).Append("\n");
      
      sb.Append("  Name: ").Append(Name).Append("\n");
      
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
