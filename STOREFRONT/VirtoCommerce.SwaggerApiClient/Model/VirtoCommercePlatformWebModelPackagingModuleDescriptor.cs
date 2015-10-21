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
  public class VirtoCommercePlatformWebModelPackagingModuleDescriptor {
    
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public string Id { get; set; }

    
    /// <summary>
    /// Gets or Sets Version
    /// </summary>
    [DataMember(Name="version", EmitDefaultValue=false)]
    public string Version { get; set; }

    
    /// <summary>
    /// Gets or Sets PlatformVersion
    /// </summary>
    [DataMember(Name="platformVersion", EmitDefaultValue=false)]
    public string PlatformVersion { get; set; }

    
    /// <summary>
    /// Gets or Sets Title
    /// </summary>
    [DataMember(Name="title", EmitDefaultValue=false)]
    public string Title { get; set; }

    
    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    public string Description { get; set; }

    
    /// <summary>
    /// Gets or Sets Authors
    /// </summary>
    [DataMember(Name="authors", EmitDefaultValue=false)]
    public List<string> Authors { get; set; }

    
    /// <summary>
    /// Gets or Sets Owners
    /// </summary>
    [DataMember(Name="owners", EmitDefaultValue=false)]
    public List<string> Owners { get; set; }

    
    /// <summary>
    /// Gets or Sets LicenseUrl
    /// </summary>
    [DataMember(Name="licenseUrl", EmitDefaultValue=false)]
    public string LicenseUrl { get; set; }

    
    /// <summary>
    /// Gets or Sets ProjectUrl
    /// </summary>
    [DataMember(Name="projectUrl", EmitDefaultValue=false)]
    public string ProjectUrl { get; set; }

    
    /// <summary>
    /// Gets or Sets IconUrl
    /// </summary>
    [DataMember(Name="iconUrl", EmitDefaultValue=false)]
    public string IconUrl { get; set; }

    
    /// <summary>
    /// Gets or Sets RequireLicenseAcceptance
    /// </summary>
    [DataMember(Name="requireLicenseAcceptance", EmitDefaultValue=false)]
    public bool? RequireLicenseAcceptance { get; set; }

    
    /// <summary>
    /// Gets or Sets ReleaseNotes
    /// </summary>
    [DataMember(Name="releaseNotes", EmitDefaultValue=false)]
    public string ReleaseNotes { get; set; }

    
    /// <summary>
    /// Gets or Sets Copyright
    /// </summary>
    [DataMember(Name="copyright", EmitDefaultValue=false)]
    public string Copyright { get; set; }

    
    /// <summary>
    /// Gets or Sets Tags
    /// </summary>
    [DataMember(Name="tags", EmitDefaultValue=false)]
    public string Tags { get; set; }

    
    /// <summary>
    /// Gets or Sets Dependencies
    /// </summary>
    [DataMember(Name="dependencies", EmitDefaultValue=false)]
    public List<VirtoCommercePlatformWebModelPackagingModuleIdentity> Dependencies { get; set; }

    
    /// <summary>
    /// Gets or Sets ValidationErrors
    /// </summary>
    [DataMember(Name="validationErrors", EmitDefaultValue=false)]
    public List<string> ValidationErrors { get; set; }

    
    /// <summary>
    /// Gets or Sets IsRemovable
    /// </summary>
    [DataMember(Name="isRemovable", EmitDefaultValue=false)]
    public bool? IsRemovable { get; set; }

    
    /// <summary>
    /// Module package file name
    /// </summary>
    /// <value>Module package file name</value>
    [DataMember(Name="fileName", EmitDefaultValue=false)]
    public string FileName { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommercePlatformWebModelPackagingModuleDescriptor {\n");
      
      sb.Append("  Id: ").Append(Id).Append("\n");
      
      sb.Append("  Version: ").Append(Version).Append("\n");
      
      sb.Append("  PlatformVersion: ").Append(PlatformVersion).Append("\n");
      
      sb.Append("  Title: ").Append(Title).Append("\n");
      
      sb.Append("  Description: ").Append(Description).Append("\n");
      
      sb.Append("  Authors: ").Append(Authors).Append("\n");
      
      sb.Append("  Owners: ").Append(Owners).Append("\n");
      
      sb.Append("  LicenseUrl: ").Append(LicenseUrl).Append("\n");
      
      sb.Append("  ProjectUrl: ").Append(ProjectUrl).Append("\n");
      
      sb.Append("  IconUrl: ").Append(IconUrl).Append("\n");
      
      sb.Append("  RequireLicenseAcceptance: ").Append(RequireLicenseAcceptance).Append("\n");
      
      sb.Append("  ReleaseNotes: ").Append(ReleaseNotes).Append("\n");
      
      sb.Append("  Copyright: ").Append(Copyright).Append("\n");
      
      sb.Append("  Tags: ").Append(Tags).Append("\n");
      
      sb.Append("  Dependencies: ").Append(Dependencies).Append("\n");
      
      sb.Append("  ValidationErrors: ").Append(ValidationErrors).Append("\n");
      
      sb.Append("  IsRemovable: ").Append(IsRemovable).Append("\n");
      
      sb.Append("  FileName: ").Append(FileName).Append("\n");
      
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
