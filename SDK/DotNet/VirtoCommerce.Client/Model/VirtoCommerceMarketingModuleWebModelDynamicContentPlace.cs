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
    /// Represent presentation placeholders for dynamic content publication
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceMarketingModuleWebModelDynamicContentPlace :  IEquatable<VirtoCommerceMarketingModuleWebModelDynamicContentPlace>
    {
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
        /// Gets or Sets FolderId
        /// </summary>
        [DataMember(Name="folderId", EmitDefaultValue=false)]
        public string FolderId { get; set; }

        /// <summary>
        /// all parent folders ids concatenated (1;21;344)
        /// </summary>
        /// <value>all parent folders ids concatenated (1;21;344)</value>
        [DataMember(Name="outline", EmitDefaultValue=false)]
        public string Outline { get; set; }

        /// <summary>
        /// all parent folders names concatenated (Root\\Child\\Child2)
        /// </summary>
        /// <value>all parent folders names concatenated (Root\\Child\\Child2)</value>
        [DataMember(Name="path", EmitDefaultValue=false)]
        public string Path { get; set; }

        /// <summary>
        /// Gets or Sets ImageUrl
        /// </summary>
        [DataMember(Name="imageUrl", EmitDefaultValue=false)]
        public string ImageUrl { get; set; }

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
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMarketingModuleWebModelDynamicContentPlace {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  FolderId: ").Append(FolderId).Append("\n");
            sb.Append("  Outline: ").Append(Outline).Append("\n");
            sb.Append("  Path: ").Append(Path).Append("\n");
            sb.Append("  ImageUrl: ").Append(ImageUrl).Append("\n");
            sb.Append("  CreatedDate: ").Append(CreatedDate).Append("\n");
            sb.Append("  ModifiedDate: ").Append(ModifiedDate).Append("\n");
            sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
            sb.Append("  ModifiedBy: ").Append(ModifiedBy).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
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
            return this.Equals(obj as VirtoCommerceMarketingModuleWebModelDynamicContentPlace);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMarketingModuleWebModelDynamicContentPlace instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceMarketingModuleWebModelDynamicContentPlace to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMarketingModuleWebModelDynamicContentPlace other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
                ) && 
                (
                    this.FolderId == other.FolderId ||
                    this.FolderId != null &&
                    this.FolderId.Equals(other.FolderId)
                ) && 
                (
                    this.Outline == other.Outline ||
                    this.Outline != null &&
                    this.Outline.Equals(other.Outline)
                ) && 
                (
                    this.Path == other.Path ||
                    this.Path != null &&
                    this.Path.Equals(other.Path)
                ) && 
                (
                    this.ImageUrl == other.ImageUrl ||
                    this.ImageUrl != null &&
                    this.ImageUrl.Equals(other.ImageUrl)
                ) && 
                (
                    this.CreatedDate == other.CreatedDate ||
                    this.CreatedDate != null &&
                    this.CreatedDate.Equals(other.CreatedDate)
                ) && 
                (
                    this.ModifiedDate == other.ModifiedDate ||
                    this.ModifiedDate != null &&
                    this.ModifiedDate.Equals(other.ModifiedDate)
                ) && 
                (
                    this.CreatedBy == other.CreatedBy ||
                    this.CreatedBy != null &&
                    this.CreatedBy.Equals(other.CreatedBy)
                ) && 
                (
                    this.ModifiedBy == other.ModifiedBy ||
                    this.ModifiedBy != null &&
                    this.ModifiedBy.Equals(other.ModifiedBy)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
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

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.Description != null)
                    hash = hash * 59 + this.Description.GetHashCode();

                if (this.FolderId != null)
                    hash = hash * 59 + this.FolderId.GetHashCode();

                if (this.Outline != null)
                    hash = hash * 59 + this.Outline.GetHashCode();

                if (this.Path != null)
                    hash = hash * 59 + this.Path.GetHashCode();

                if (this.ImageUrl != null)
                    hash = hash * 59 + this.ImageUrl.GetHashCode();

                if (this.CreatedDate != null)
                    hash = hash * 59 + this.CreatedDate.GetHashCode();

                if (this.ModifiedDate != null)
                    hash = hash * 59 + this.ModifiedDate.GetHashCode();

                if (this.CreatedBy != null)
                    hash = hash * 59 + this.CreatedBy.GetHashCode();

                if (this.ModifiedBy != null)
                    hash = hash * 59 + this.ModifiedBy.GetHashCode();

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

                return hash;
            }
        }

    }
}
