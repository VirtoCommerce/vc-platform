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
    public partial class VirtoCommerceMarketingModuleWebModelMarketingSearchResult :  IEquatable<VirtoCommerceMarketingModuleWebModelMarketingSearchResult>
    {
        /// <summary>
        /// Gets or Sets TotalCount
        /// </summary>
        [DataMember(Name="totalCount", EmitDefaultValue=false)]
        public int? TotalCount { get; set; }

        /// <summary>
        /// Gets or Sets Promotions
        /// </summary>
        [DataMember(Name="promotions", EmitDefaultValue=false)]
        public List<VirtoCommerceMarketingModuleWebModelPromotion> Promotions { get; set; }

        /// <summary>
        /// Gets or Sets ContentPlaces
        /// </summary>
        [DataMember(Name="contentPlaces", EmitDefaultValue=false)]
        public List<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> ContentPlaces { get; set; }

        /// <summary>
        /// Gets or Sets ContentItems
        /// </summary>
        [DataMember(Name="contentItems", EmitDefaultValue=false)]
        public List<VirtoCommerceMarketingModuleWebModelDynamicContentItem> ContentItems { get; set; }

        /// <summary>
        /// Gets or Sets ContentPublications
        /// </summary>
        [DataMember(Name="contentPublications", EmitDefaultValue=false)]
        public List<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> ContentPublications { get; set; }

        /// <summary>
        /// Gets or Sets ContentFolders
        /// </summary>
        [DataMember(Name="contentFolders", EmitDefaultValue=false)]
        public List<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> ContentFolders { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMarketingModuleWebModelMarketingSearchResult {\n");
            sb.Append("  TotalCount: ").Append(TotalCount).Append("\n");
            sb.Append("  Promotions: ").Append(Promotions).Append("\n");
            sb.Append("  ContentPlaces: ").Append(ContentPlaces).Append("\n");
            sb.Append("  ContentItems: ").Append(ContentItems).Append("\n");
            sb.Append("  ContentPublications: ").Append(ContentPublications).Append("\n");
            sb.Append("  ContentFolders: ").Append(ContentFolders).Append("\n");
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
            return this.Equals(obj as VirtoCommerceMarketingModuleWebModelMarketingSearchResult);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMarketingModuleWebModelMarketingSearchResult instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceMarketingModuleWebModelMarketingSearchResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMarketingModuleWebModelMarketingSearchResult other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.TotalCount == other.TotalCount ||
                    this.TotalCount != null &&
                    this.TotalCount.Equals(other.TotalCount)
                ) && 
                (
                    this.Promotions == other.Promotions ||
                    this.Promotions != null &&
                    this.Promotions.SequenceEqual(other.Promotions)
                ) && 
                (
                    this.ContentPlaces == other.ContentPlaces ||
                    this.ContentPlaces != null &&
                    this.ContentPlaces.SequenceEqual(other.ContentPlaces)
                ) && 
                (
                    this.ContentItems == other.ContentItems ||
                    this.ContentItems != null &&
                    this.ContentItems.SequenceEqual(other.ContentItems)
                ) && 
                (
                    this.ContentPublications == other.ContentPublications ||
                    this.ContentPublications != null &&
                    this.ContentPublications.SequenceEqual(other.ContentPublications)
                ) && 
                (
                    this.ContentFolders == other.ContentFolders ||
                    this.ContentFolders != null &&
                    this.ContentFolders.SequenceEqual(other.ContentFolders)
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

                if (this.TotalCount != null)
                    hash = hash * 59 + this.TotalCount.GetHashCode();

                if (this.Promotions != null)
                    hash = hash * 59 + this.Promotions.GetHashCode();

                if (this.ContentPlaces != null)
                    hash = hash * 59 + this.ContentPlaces.GetHashCode();

                if (this.ContentItems != null)
                    hash = hash * 59 + this.ContentItems.GetHashCode();

                if (this.ContentPublications != null)
                    hash = hash * 59 + this.ContentPublications.GetHashCode();

                if (this.ContentFolders != null)
                    hash = hash * 59 + this.ContentFolders.GetHashCode();

                return hash;
            }
        }

    }
}
