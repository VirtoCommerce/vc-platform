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
    public partial class VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext :  IEquatable<VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext>
    {
        /// <summary>
        /// Gets or Sets StoreId
        /// </summary>
        [DataMember(Name="storeId", EmitDefaultValue=false)]
        public string StoreId { get; set; }

        /// <summary>
        /// Gets or Sets PlaceName
        /// </summary>
        [DataMember(Name="placeName", EmitDefaultValue=false)]
        public string PlaceName { get; set; }

        /// <summary>
        /// Gets or Sets Tags
        /// </summary>
        [DataMember(Name="tags", EmitDefaultValue=false)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Gets or Sets ToDate
        /// </summary>
        [DataMember(Name="toDate", EmitDefaultValue=false)]
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Gets or Sets ContextObject
        /// </summary>
        [DataMember(Name="contextObject", EmitDefaultValue=false)]
        public Object ContextObject { get; set; }

        /// <summary>
        /// Gets or Sets GeoCity
        /// </summary>
        [DataMember(Name="geoCity", EmitDefaultValue=false)]
        public string GeoCity { get; set; }

        /// <summary>
        /// Gets or Sets GeoState
        /// </summary>
        [DataMember(Name="geoState", EmitDefaultValue=false)]
        public string GeoState { get; set; }

        /// <summary>
        /// Gets or Sets GeoCountry
        /// </summary>
        [DataMember(Name="geoCountry", EmitDefaultValue=false)]
        public string GeoCountry { get; set; }

        /// <summary>
        /// Gets or Sets GeoContinent
        /// </summary>
        [DataMember(Name="geoContinent", EmitDefaultValue=false)]
        public string GeoContinent { get; set; }

        /// <summary>
        /// Gets or Sets GeoZipCode
        /// </summary>
        [DataMember(Name="geoZipCode", EmitDefaultValue=false)]
        public string GeoZipCode { get; set; }

        /// <summary>
        /// Gets or Sets GeoConnectionType
        /// </summary>
        [DataMember(Name="geoConnectionType", EmitDefaultValue=false)]
        public string GeoConnectionType { get; set; }

        /// <summary>
        /// Gets or Sets GeoTimeZone
        /// </summary>
        [DataMember(Name="geoTimeZone", EmitDefaultValue=false)]
        public string GeoTimeZone { get; set; }

        /// <summary>
        /// Gets or Sets GeoIpRoutingType
        /// </summary>
        [DataMember(Name="geoIpRoutingType", EmitDefaultValue=false)]
        public string GeoIpRoutingType { get; set; }

        /// <summary>
        /// Gets or Sets GeoIspSecondLevel
        /// </summary>
        [DataMember(Name="geoIspSecondLevel", EmitDefaultValue=false)]
        public string GeoIspSecondLevel { get; set; }

        /// <summary>
        /// Gets or Sets GeoIspTopLevel
        /// </summary>
        [DataMember(Name="geoIspTopLevel", EmitDefaultValue=false)]
        public string GeoIspTopLevel { get; set; }

        /// <summary>
        /// Gets or Sets ShopperAge
        /// </summary>
        [DataMember(Name="shopperAge", EmitDefaultValue=false)]
        public int? ShopperAge { get; set; }

        /// <summary>
        /// Gets or Sets ShopperGender
        /// </summary>
        [DataMember(Name="shopperGender", EmitDefaultValue=false)]
        public string ShopperGender { get; set; }

        /// <summary>
        /// Gets or Sets Language
        /// </summary>
        [DataMember(Name="language", EmitDefaultValue=false)]
        public string Language { get; set; }

        /// <summary>
        /// Gets or Sets ShopperSearchedPhraseInStore
        /// </summary>
        [DataMember(Name="shopperSearchedPhraseInStore", EmitDefaultValue=false)]
        public string ShopperSearchedPhraseInStore { get; set; }

        /// <summary>
        /// Gets or Sets ShopperSearchedPhraseOnInternet
        /// </summary>
        [DataMember(Name="shopperSearchedPhraseOnInternet", EmitDefaultValue=false)]
        public string ShopperSearchedPhraseOnInternet { get; set; }

        /// <summary>
        /// Gets or Sets CurrentUrl
        /// </summary>
        [DataMember(Name="currentUrl", EmitDefaultValue=false)]
        public string CurrentUrl { get; set; }

        /// <summary>
        /// Gets or Sets ReferredUrl
        /// </summary>
        [DataMember(Name="referredUrl", EmitDefaultValue=false)]
        public string ReferredUrl { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext {\n");
            sb.Append("  StoreId: ").Append(StoreId).Append("\n");
            sb.Append("  PlaceName: ").Append(PlaceName).Append("\n");
            sb.Append("  Tags: ").Append(Tags).Append("\n");
            sb.Append("  ToDate: ").Append(ToDate).Append("\n");
            sb.Append("  ContextObject: ").Append(ContextObject).Append("\n");
            sb.Append("  GeoCity: ").Append(GeoCity).Append("\n");
            sb.Append("  GeoState: ").Append(GeoState).Append("\n");
            sb.Append("  GeoCountry: ").Append(GeoCountry).Append("\n");
            sb.Append("  GeoContinent: ").Append(GeoContinent).Append("\n");
            sb.Append("  GeoZipCode: ").Append(GeoZipCode).Append("\n");
            sb.Append("  GeoConnectionType: ").Append(GeoConnectionType).Append("\n");
            sb.Append("  GeoTimeZone: ").Append(GeoTimeZone).Append("\n");
            sb.Append("  GeoIpRoutingType: ").Append(GeoIpRoutingType).Append("\n");
            sb.Append("  GeoIspSecondLevel: ").Append(GeoIspSecondLevel).Append("\n");
            sb.Append("  GeoIspTopLevel: ").Append(GeoIspTopLevel).Append("\n");
            sb.Append("  ShopperAge: ").Append(ShopperAge).Append("\n");
            sb.Append("  ShopperGender: ").Append(ShopperGender).Append("\n");
            sb.Append("  Language: ").Append(Language).Append("\n");
            sb.Append("  ShopperSearchedPhraseInStore: ").Append(ShopperSearchedPhraseInStore).Append("\n");
            sb.Append("  ShopperSearchedPhraseOnInternet: ").Append(ShopperSearchedPhraseOnInternet).Append("\n");
            sb.Append("  CurrentUrl: ").Append(CurrentUrl).Append("\n");
            sb.Append("  ReferredUrl: ").Append(ReferredUrl).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.StoreId == other.StoreId ||
                    this.StoreId != null &&
                    this.StoreId.Equals(other.StoreId)
                ) && 
                (
                    this.PlaceName == other.PlaceName ||
                    this.PlaceName != null &&
                    this.PlaceName.Equals(other.PlaceName)
                ) && 
                (
                    this.Tags == other.Tags ||
                    this.Tags != null &&
                    this.Tags.SequenceEqual(other.Tags)
                ) && 
                (
                    this.ToDate == other.ToDate ||
                    this.ToDate != null &&
                    this.ToDate.Equals(other.ToDate)
                ) && 
                (
                    this.ContextObject == other.ContextObject ||
                    this.ContextObject != null &&
                    this.ContextObject.Equals(other.ContextObject)
                ) && 
                (
                    this.GeoCity == other.GeoCity ||
                    this.GeoCity != null &&
                    this.GeoCity.Equals(other.GeoCity)
                ) && 
                (
                    this.GeoState == other.GeoState ||
                    this.GeoState != null &&
                    this.GeoState.Equals(other.GeoState)
                ) && 
                (
                    this.GeoCountry == other.GeoCountry ||
                    this.GeoCountry != null &&
                    this.GeoCountry.Equals(other.GeoCountry)
                ) && 
                (
                    this.GeoContinent == other.GeoContinent ||
                    this.GeoContinent != null &&
                    this.GeoContinent.Equals(other.GeoContinent)
                ) && 
                (
                    this.GeoZipCode == other.GeoZipCode ||
                    this.GeoZipCode != null &&
                    this.GeoZipCode.Equals(other.GeoZipCode)
                ) && 
                (
                    this.GeoConnectionType == other.GeoConnectionType ||
                    this.GeoConnectionType != null &&
                    this.GeoConnectionType.Equals(other.GeoConnectionType)
                ) && 
                (
                    this.GeoTimeZone == other.GeoTimeZone ||
                    this.GeoTimeZone != null &&
                    this.GeoTimeZone.Equals(other.GeoTimeZone)
                ) && 
                (
                    this.GeoIpRoutingType == other.GeoIpRoutingType ||
                    this.GeoIpRoutingType != null &&
                    this.GeoIpRoutingType.Equals(other.GeoIpRoutingType)
                ) && 
                (
                    this.GeoIspSecondLevel == other.GeoIspSecondLevel ||
                    this.GeoIspSecondLevel != null &&
                    this.GeoIspSecondLevel.Equals(other.GeoIspSecondLevel)
                ) && 
                (
                    this.GeoIspTopLevel == other.GeoIspTopLevel ||
                    this.GeoIspTopLevel != null &&
                    this.GeoIspTopLevel.Equals(other.GeoIspTopLevel)
                ) && 
                (
                    this.ShopperAge == other.ShopperAge ||
                    this.ShopperAge != null &&
                    this.ShopperAge.Equals(other.ShopperAge)
                ) && 
                (
                    this.ShopperGender == other.ShopperGender ||
                    this.ShopperGender != null &&
                    this.ShopperGender.Equals(other.ShopperGender)
                ) && 
                (
                    this.Language == other.Language ||
                    this.Language != null &&
                    this.Language.Equals(other.Language)
                ) && 
                (
                    this.ShopperSearchedPhraseInStore == other.ShopperSearchedPhraseInStore ||
                    this.ShopperSearchedPhraseInStore != null &&
                    this.ShopperSearchedPhraseInStore.Equals(other.ShopperSearchedPhraseInStore)
                ) && 
                (
                    this.ShopperSearchedPhraseOnInternet == other.ShopperSearchedPhraseOnInternet ||
                    this.ShopperSearchedPhraseOnInternet != null &&
                    this.ShopperSearchedPhraseOnInternet.Equals(other.ShopperSearchedPhraseOnInternet)
                ) && 
                (
                    this.CurrentUrl == other.CurrentUrl ||
                    this.CurrentUrl != null &&
                    this.CurrentUrl.Equals(other.CurrentUrl)
                ) && 
                (
                    this.ReferredUrl == other.ReferredUrl ||
                    this.ReferredUrl != null &&
                    this.ReferredUrl.Equals(other.ReferredUrl)
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

                if (this.StoreId != null)
                    hash = hash * 59 + this.StoreId.GetHashCode();

                if (this.PlaceName != null)
                    hash = hash * 59 + this.PlaceName.GetHashCode();

                if (this.Tags != null)
                    hash = hash * 59 + this.Tags.GetHashCode();

                if (this.ToDate != null)
                    hash = hash * 59 + this.ToDate.GetHashCode();

                if (this.ContextObject != null)
                    hash = hash * 59 + this.ContextObject.GetHashCode();

                if (this.GeoCity != null)
                    hash = hash * 59 + this.GeoCity.GetHashCode();

                if (this.GeoState != null)
                    hash = hash * 59 + this.GeoState.GetHashCode();

                if (this.GeoCountry != null)
                    hash = hash * 59 + this.GeoCountry.GetHashCode();

                if (this.GeoContinent != null)
                    hash = hash * 59 + this.GeoContinent.GetHashCode();

                if (this.GeoZipCode != null)
                    hash = hash * 59 + this.GeoZipCode.GetHashCode();

                if (this.GeoConnectionType != null)
                    hash = hash * 59 + this.GeoConnectionType.GetHashCode();

                if (this.GeoTimeZone != null)
                    hash = hash * 59 + this.GeoTimeZone.GetHashCode();

                if (this.GeoIpRoutingType != null)
                    hash = hash * 59 + this.GeoIpRoutingType.GetHashCode();

                if (this.GeoIspSecondLevel != null)
                    hash = hash * 59 + this.GeoIspSecondLevel.GetHashCode();

                if (this.GeoIspTopLevel != null)
                    hash = hash * 59 + this.GeoIspTopLevel.GetHashCode();

                if (this.ShopperAge != null)
                    hash = hash * 59 + this.ShopperAge.GetHashCode();

                if (this.ShopperGender != null)
                    hash = hash * 59 + this.ShopperGender.GetHashCode();

                if (this.Language != null)
                    hash = hash * 59 + this.Language.GetHashCode();

                if (this.ShopperSearchedPhraseInStore != null)
                    hash = hash * 59 + this.ShopperSearchedPhraseInStore.GetHashCode();

                if (this.ShopperSearchedPhraseOnInternet != null)
                    hash = hash * 59 + this.ShopperSearchedPhraseOnInternet.GetHashCode();

                if (this.CurrentUrl != null)
                    hash = hash * 59 + this.CurrentUrl.GetHashCode();

                if (this.ReferredUrl != null)
                    hash = hash * 59 + this.ReferredUrl.GetHashCode();

                return hash;
            }
        }

    }
}
