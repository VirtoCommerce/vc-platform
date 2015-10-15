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
  public class VirtoCommerceCustomerModuleWebModelContact {
    
    /// <summary>
    /// Gets or Sets DisplayName
    /// </summary>
    [DataMember(Name="displayName", EmitDefaultValue=false)]
    public string DisplayName { get; set; }

    
    /// <summary>
    /// Gets or Sets FullName
    /// </summary>
    [DataMember(Name="fullName", EmitDefaultValue=false)]
    public string FullName { get; set; }

    
    /// <summary>
    /// Gets or Sets TimeZone
    /// </summary>
    [DataMember(Name="timeZone", EmitDefaultValue=false)]
    public string TimeZone { get; set; }

    
    /// <summary>
    /// Language Culture Name (en-US,fr-FR  etc)
    /// </summary>
    /// <value>Language Culture Name (en-US,fr-FR  etc)</value>
    [DataMember(Name="defaultLanguage", EmitDefaultValue=false)]
    public string DefaultLanguage { get; set; }

    
    /// <summary>
    /// Gets or Sets BirthDate
    /// </summary>
    [DataMember(Name="birthDate", EmitDefaultValue=false)]
    public DateTime? BirthDate { get; set; }

    
    /// <summary>
    /// Not documented
    /// </summary>
    /// <value>Not documented</value>
    [DataMember(Name="taxpayerId", EmitDefaultValue=false)]
    public string TaxpayerId { get; set; }

    
    /// <summary>
    /// Gets or Sets PreferredDelivery
    /// </summary>
    [DataMember(Name="preferredDelivery", EmitDefaultValue=false)]
    public string PreferredDelivery { get; set; }

    
    /// <summary>
    /// Gets or Sets PreferredCommunication
    /// </summary>
    [DataMember(Name="preferredCommunication", EmitDefaultValue=false)]
    public string PreferredCommunication { get; set; }

    
    /// <summary>
    /// Gets or Sets Salutation
    /// </summary>
    [DataMember(Name="salutation", EmitDefaultValue=false)]
    public string Salutation { get; set; }

    
    /// <summary>
    /// Not documented
    /// </summary>
    /// <value>Not documented</value>
    [DataMember(Name="organizations", EmitDefaultValue=false)]
    public List<string> Organizations { get; set; }

    
    /// <summary>
    /// String representation of member type (Organization or Contact). Used as Discriminator
    /// </summary>
    /// <value>String representation of member type (Organization or Contact). Used as Discriminator</value>
    [DataMember(Name="memberType", EmitDefaultValue=false)]
    public string MemberType { get; set; }

    
    /// <summary>
    /// Gets or Sets Addresses
    /// </summary>
    [DataMember(Name="addresses", EmitDefaultValue=false)]
    public List<VirtoCommerceCustomerModuleWebModelAddress> Addresses { get; set; }

    
    /// <summary>
    /// Gets or Sets Phones
    /// </summary>
    [DataMember(Name="phones", EmitDefaultValue=false)]
    public List<string> Phones { get; set; }

    
    /// <summary>
    /// Gets or Sets Emails
    /// </summary>
    [DataMember(Name="emails", EmitDefaultValue=false)]
    public List<string> Emails { get; set; }

    
    /// <summary>
    /// Additional information about the member
    /// </summary>
    /// <value>Additional information about the member</value>
    [DataMember(Name="notes", EmitDefaultValue=false)]
    public List<VirtoCommerceCustomerModuleWebModelNote> Notes { get; set; }

    
    /// <summary>
    /// Not documented
    /// </summary>
    /// <value>Not documented</value>
    [DataMember(Name="objectType", EmitDefaultValue=false)]
    public string ObjectType { get; set; }

    
    /// <summary>
    /// Some additional properties
    /// </summary>
    /// <value>Some additional properties</value>
    [DataMember(Name="dynamicProperties", EmitDefaultValue=false)]
    public List<VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty> DynamicProperties { get; set; }

    
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
      sb.Append("class VirtoCommerceCustomerModuleWebModelContact {\n");
      
      sb.Append("  DisplayName: ").Append(DisplayName).Append("\n");
      
      sb.Append("  FullName: ").Append(FullName).Append("\n");
      
      sb.Append("  TimeZone: ").Append(TimeZone).Append("\n");
      
      sb.Append("  DefaultLanguage: ").Append(DefaultLanguage).Append("\n");
      
      sb.Append("  BirthDate: ").Append(BirthDate).Append("\n");
      
      sb.Append("  TaxpayerId: ").Append(TaxpayerId).Append("\n");
      
      sb.Append("  PreferredDelivery: ").Append(PreferredDelivery).Append("\n");
      
      sb.Append("  PreferredCommunication: ").Append(PreferredCommunication).Append("\n");
      
      sb.Append("  Salutation: ").Append(Salutation).Append("\n");
      
      sb.Append("  Organizations: ").Append(Organizations).Append("\n");
      
      sb.Append("  MemberType: ").Append(MemberType).Append("\n");
      
      sb.Append("  Addresses: ").Append(Addresses).Append("\n");
      
      sb.Append("  Phones: ").Append(Phones).Append("\n");
      
      sb.Append("  Emails: ").Append(Emails).Append("\n");
      
      sb.Append("  Notes: ").Append(Notes).Append("\n");
      
      sb.Append("  ObjectType: ").Append(ObjectType).Append("\n");
      
      sb.Append("  DynamicProperties: ").Append(DynamicProperties).Append("\n");
      
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
