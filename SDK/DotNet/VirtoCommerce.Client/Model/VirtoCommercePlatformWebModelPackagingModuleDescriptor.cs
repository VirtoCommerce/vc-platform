using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Client.Model
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class VirtoCommercePlatformWebModelPackagingModuleDescriptor :  IEquatable<VirtoCommercePlatformWebModelPackagingModuleDescriptor>
    {
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
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
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
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            return this.Equals(obj as VirtoCommercePlatformWebModelPackagingModuleDescriptor);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformWebModelPackagingModuleDescriptor instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePlatformWebModelPackagingModuleDescriptor to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformWebModelPackagingModuleDescriptor other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                ) && 
                (
                    this.Version == other.Version ||
                    this.Version != null &&
                    this.Version.Equals(other.Version)
                ) && 
                (
                    this.PlatformVersion == other.PlatformVersion ||
                    this.PlatformVersion != null &&
                    this.PlatformVersion.Equals(other.PlatformVersion)
                ) && 
                (
                    this.Title == other.Title ||
                    this.Title != null &&
                    this.Title.Equals(other.Title)
                ) && 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
                ) && 
                (
                    this.Authors == other.Authors ||
                    this.Authors != null &&
                    this.Authors.SequenceEqual(other.Authors)
                ) && 
                (
                    this.Owners == other.Owners ||
                    this.Owners != null &&
                    this.Owners.SequenceEqual(other.Owners)
                ) && 
                (
                    this.LicenseUrl == other.LicenseUrl ||
                    this.LicenseUrl != null &&
                    this.LicenseUrl.Equals(other.LicenseUrl)
                ) && 
                (
                    this.ProjectUrl == other.ProjectUrl ||
                    this.ProjectUrl != null &&
                    this.ProjectUrl.Equals(other.ProjectUrl)
                ) && 
                (
                    this.IconUrl == other.IconUrl ||
                    this.IconUrl != null &&
                    this.IconUrl.Equals(other.IconUrl)
                ) && 
                (
                    this.RequireLicenseAcceptance == other.RequireLicenseAcceptance ||
                    this.RequireLicenseAcceptance != null &&
                    this.RequireLicenseAcceptance.Equals(other.RequireLicenseAcceptance)
                ) && 
                (
                    this.ReleaseNotes == other.ReleaseNotes ||
                    this.ReleaseNotes != null &&
                    this.ReleaseNotes.Equals(other.ReleaseNotes)
                ) && 
                (
                    this.Copyright == other.Copyright ||
                    this.Copyright != null &&
                    this.Copyright.Equals(other.Copyright)
                ) && 
                (
                    this.Tags == other.Tags ||
                    this.Tags != null &&
                    this.Tags.Equals(other.Tags)
                ) && 
                (
                    this.Dependencies == other.Dependencies ||
                    this.Dependencies != null &&
                    this.Dependencies.SequenceEqual(other.Dependencies)
                ) && 
                (
                    this.ValidationErrors == other.ValidationErrors ||
                    this.ValidationErrors != null &&
                    this.ValidationErrors.SequenceEqual(other.ValidationErrors)
                ) && 
                (
                    this.IsRemovable == other.IsRemovable ||
                    this.IsRemovable != null &&
                    this.IsRemovable.Equals(other.IsRemovable)
                ) && 
                (
                    this.FileName == other.FileName ||
                    this.FileName != null &&
                    this.FileName.Equals(other.FileName)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

                if (this.Version != null)
                    hash = hash * 59 + this.Version.GetHashCode();

                if (this.PlatformVersion != null)
                    hash = hash * 59 + this.PlatformVersion.GetHashCode();

                if (this.Title != null)
                    hash = hash * 59 + this.Title.GetHashCode();

                if (this.Description != null)
                    hash = hash * 59 + this.Description.GetHashCode();

                if (this.Authors != null)
                    hash = hash * 59 + this.Authors.GetHashCode();

                if (this.Owners != null)
                    hash = hash * 59 + this.Owners.GetHashCode();

                if (this.LicenseUrl != null)
                    hash = hash * 59 + this.LicenseUrl.GetHashCode();

                if (this.ProjectUrl != null)
                    hash = hash * 59 + this.ProjectUrl.GetHashCode();

                if (this.IconUrl != null)
                    hash = hash * 59 + this.IconUrl.GetHashCode();

                if (this.RequireLicenseAcceptance != null)
                    hash = hash * 59 + this.RequireLicenseAcceptance.GetHashCode();

                if (this.ReleaseNotes != null)
                    hash = hash * 59 + this.ReleaseNotes.GetHashCode();

                if (this.Copyright != null)
                    hash = hash * 59 + this.Copyright.GetHashCode();

                if (this.Tags != null)
                    hash = hash * 59 + this.Tags.GetHashCode();

                if (this.Dependencies != null)
                    hash = hash * 59 + this.Dependencies.GetHashCode();

                if (this.ValidationErrors != null)
                    hash = hash * 59 + this.ValidationErrors.GetHashCode();

                if (this.IsRemovable != null)
                    hash = hash * 59 + this.IsRemovable.GetHashCode();

                if (this.FileName != null)
                    hash = hash * 59 + this.FileName.GetHashCode();

                return hash;
            }
        }

    }
}
