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
    public partial class VirtoCommercePlatformCoreAssetBlobFolder :  IEquatable<VirtoCommercePlatformCoreAssetBlobFolder>
    {
        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Url
        /// </summary>
        [DataMember(Name="url", EmitDefaultValue=false)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or Sets RelativeUrl
        /// </summary>
        [DataMember(Name="relativeUrl", EmitDefaultValue=false)]
        public string RelativeUrl { get; set; }

        /// <summary>
        /// Gets or Sets ParentUrl
        /// </summary>
        [DataMember(Name="parentUrl", EmitDefaultValue=false)]
        public string ParentUrl { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommercePlatformCoreAssetBlobFolder {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Url: ").Append(Url).Append("\n");
            sb.Append("  RelativeUrl: ").Append(RelativeUrl).Append("\n");
            sb.Append("  ParentUrl: ").Append(ParentUrl).Append("\n");
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
            return this.Equals(obj as VirtoCommercePlatformCoreAssetBlobFolder);
        }

        /// <summary>
        /// Returns true if VirtoCommercePlatformCoreAssetBlobFolder instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePlatformCoreAssetBlobFolder to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePlatformCoreAssetBlobFolder other)
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
                    this.Url == other.Url ||
                    this.Url != null &&
                    this.Url.Equals(other.Url)
                ) && 
                (
                    this.RelativeUrl == other.RelativeUrl ||
                    this.RelativeUrl != null &&
                    this.RelativeUrl.Equals(other.RelativeUrl)
                ) && 
                (
                    this.ParentUrl == other.ParentUrl ||
                    this.ParentUrl != null &&
                    this.ParentUrl.Equals(other.ParentUrl)
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

                if (this.Url != null)
                    hash = hash * 59 + this.Url.GetHashCode();

                if (this.RelativeUrl != null)
                    hash = hash * 59 + this.RelativeUrl.GetHashCode();

                if (this.ParentUrl != null)
                    hash = hash * 59 + this.ParentUrl.GetHashCode();

                return hash;
            }
        }

    }
}
