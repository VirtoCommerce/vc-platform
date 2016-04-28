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
    public partial class VirtoCommercePlatformWebModelAssetAssetListItem :  IEquatable<VirtoCommercePlatformWebModelAssetAssetListItem>
    {
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets ContentType
        /// </summary>
        [DataMember(Name="contentType", EmitDefaultValue=false)]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or Sets Url
        /// </summary>
        [DataMember(Name="url", EmitDefaultValue=false)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or Sets Size
        /// </summary>
        [DataMember(Name="size", EmitDefaultValue=false)]
        public string Size { get; set; }

        /// <summary>
        /// Gets or Sets ParentUrl
        /// </summary>
        [DataMember(Name="parentUrl", EmitDefaultValue=false)]
        public string ParentUrl { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedDate
        /// </summary>
        [DataMember(Name="modifiedDate", EmitDefaultValue=false)]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or Sets CreatedDate
        /// </summary>
        [DataMember(Name="createdDate", EmitDefaultValue=false)]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommercePlatformWebModelAssetAssetListItem {\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  ContentType: ").Append(ContentType).Append("\n");
            sb.Append("  Url: ").Append(Url).Append("\n");
            sb.Append("  Size: ").Append(Size).Append("\n");
            sb.Append("  ParentUrl: ").Append(ParentUrl).Append("\n");
            sb.Append("  ModifiedDate: ").Append(ModifiedDate).Append("\n");
            sb.Append("  CreatedDate: ").Append(CreatedDate).Append("\n");
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
            return this.Equals(obj as VirtoCommercePlatformWebModelAssetAssetListItem);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformWebModelAssetAssetListItem instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePlatformWebModelAssetAssetListItem to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformWebModelAssetAssetListItem other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Type == other.Type ||
                    this.Type != null &&
                    this.Type.Equals(other.Type)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.ContentType == other.ContentType ||
                    this.ContentType != null &&
                    this.ContentType.Equals(other.ContentType)
                ) && 
                (
                    this.Url == other.Url ||
                    this.Url != null &&
                    this.Url.Equals(other.Url)
                ) && 
                (
                    this.Size == other.Size ||
                    this.Size != null &&
                    this.Size.Equals(other.Size)
                ) && 
                (
                    this.ParentUrl == other.ParentUrl ||
                    this.ParentUrl != null &&
                    this.ParentUrl.Equals(other.ParentUrl)
                ) && 
                (
                    this.ModifiedDate == other.ModifiedDate ||
                    this.ModifiedDate != null &&
                    this.ModifiedDate.Equals(other.ModifiedDate)
                ) && 
                (
                    this.CreatedDate == other.CreatedDate ||
                    this.CreatedDate != null &&
                    this.CreatedDate.Equals(other.CreatedDate)
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

                if (this.Type != null)
                    hash = hash * 59 + this.Type.GetHashCode();

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.ContentType != null)
                    hash = hash * 59 + this.ContentType.GetHashCode();

                if (this.Url != null)
                    hash = hash * 59 + this.Url.GetHashCode();

                if (this.Size != null)
                    hash = hash * 59 + this.Size.GetHashCode();

                if (this.ParentUrl != null)
                    hash = hash * 59 + this.ParentUrl.GetHashCode();

                if (this.ModifiedDate != null)
                    hash = hash * 59 + this.ModifiedDate.GetHashCode();

                if (this.CreatedDate != null)
                    hash = hash * 59 + this.CreatedDate.GetHashCode();

                return hash;
            }
        }

    }
}
