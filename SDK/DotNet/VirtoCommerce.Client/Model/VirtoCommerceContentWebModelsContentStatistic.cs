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
    /// Represent a summary content statistics
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceContentWebModelsContentStatistic :  IEquatable<VirtoCommerceContentWebModelsContentStatistic>
    {
        /// <summary>
        /// Gets or Sets ActiveThemeName
        /// </summary>
        [DataMember(Name="activeThemeName", EmitDefaultValue=false)]
        public string ActiveThemeName { get; set; }

        /// <summary>
        /// Gets or Sets ThemesCount
        /// </summary>
        [DataMember(Name="themesCount", EmitDefaultValue=false)]
        public int? ThemesCount { get; set; }

        /// <summary>
        /// Gets or Sets PagesCount
        /// </summary>
        [DataMember(Name="pagesCount", EmitDefaultValue=false)]
        public int? PagesCount { get; set; }

        /// <summary>
        /// Gets or Sets BlogsCount
        /// </summary>
        [DataMember(Name="blogsCount", EmitDefaultValue=false)]
        public int? BlogsCount { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceContentWebModelsContentStatistic {\n");
            sb.Append("  ActiveThemeName: ").Append(ActiveThemeName).Append("\n");
            sb.Append("  ThemesCount: ").Append(ThemesCount).Append("\n");
            sb.Append("  PagesCount: ").Append(PagesCount).Append("\n");
            sb.Append("  BlogsCount: ").Append(BlogsCount).Append("\n");
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
            return this.Equals(obj as VirtoCommerceContentWebModelsContentStatistic);
        }

        /// <summary>
        /// Returns true if VirtoCommerceContentWebModelsContentStatistic instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceContentWebModelsContentStatistic to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceContentWebModelsContentStatistic other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.ActiveThemeName == other.ActiveThemeName ||
                    this.ActiveThemeName != null &&
                    this.ActiveThemeName.Equals(other.ActiveThemeName)
                ) && 
                (
                    this.ThemesCount == other.ThemesCount ||
                    this.ThemesCount != null &&
                    this.ThemesCount.Equals(other.ThemesCount)
                ) && 
                (
                    this.PagesCount == other.PagesCount ||
                    this.PagesCount != null &&
                    this.PagesCount.Equals(other.PagesCount)
                ) && 
                (
                    this.BlogsCount == other.BlogsCount ||
                    this.BlogsCount != null &&
                    this.BlogsCount.Equals(other.BlogsCount)
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

                if (this.ActiveThemeName != null)
                    hash = hash * 59 + this.ActiveThemeName.GetHashCode();

                if (this.ThemesCount != null)
                    hash = hash * 59 + this.ThemesCount.GetHashCode();

                if (this.PagesCount != null)
                    hash = hash * 59 + this.PagesCount.GetHashCode();

                if (this.BlogsCount != null)
                    hash = hash * 59 + this.BlogsCount.GetHashCode();

                return hash;
            }
        }

    }
}
