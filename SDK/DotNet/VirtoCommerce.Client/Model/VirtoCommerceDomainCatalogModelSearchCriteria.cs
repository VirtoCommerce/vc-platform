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
    public partial class VirtoCommerceDomainCatalogModelSearchCriteria :  IEquatable<VirtoCommerceDomainCatalogModelSearchCriteria>
    {
        /// <summary>
        /// Gets or Sets StoreId
        /// </summary>
        [DataMember(Name="storeId", EmitDefaultValue=false)]
        public string StoreId { get; set; }

        /// <summary>
        /// Gets or Sets ResponseGroup
        /// </summary>
        [DataMember(Name="responseGroup", EmitDefaultValue=false)]
        public string ResponseGroup { get; set; }

        /// <summary>
        /// Gets or Sets Keyword
        /// </summary>
        [DataMember(Name="keyword", EmitDefaultValue=false)]
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or Sets SearchInChildren
        /// </summary>
        [DataMember(Name="searchInChildren", EmitDefaultValue=false)]
        public bool? SearchInChildren { get; set; }

        /// <summary>
        /// Gets or Sets SearchInVariations
        /// </summary>
        [DataMember(Name="searchInVariations", EmitDefaultValue=false)]
        public bool? SearchInVariations { get; set; }

        /// <summary>
        /// Gets or Sets CategoryId
        /// </summary>
        [DataMember(Name="categoryId", EmitDefaultValue=false)]
        public string CategoryId { get; set; }

        /// <summary>
        /// Gets or Sets CategoryIds
        /// </summary>
        [DataMember(Name="categoryIds", EmitDefaultValue=false)]
        public List<string> CategoryIds { get; set; }

        /// <summary>
        /// Gets or Sets CatalogId
        /// </summary>
        [DataMember(Name="catalogId", EmitDefaultValue=false)]
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or Sets CatalogIds
        /// </summary>
        [DataMember(Name="catalogIds", EmitDefaultValue=false)]
        public List<string> CatalogIds { get; set; }

        /// <summary>
        /// Gets or Sets LanguageCode
        /// </summary>
        [DataMember(Name="languageCode", EmitDefaultValue=false)]
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [DataMember(Name="code", EmitDefaultValue=false)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or Sets Sort
        /// </summary>
        [DataMember(Name="sort", EmitDefaultValue=false)]
        public string Sort { get; set; }

        /// <summary>
        /// Gets or Sets SortInfos
        /// </summary>
        [DataMember(Name="sortInfos", EmitDefaultValue=false)]
        public List<VirtoCommercePlatformCoreCommonSortInfo> SortInfos { get; set; }

        /// <summary>
        /// Gets or Sets HideDirectLinkedCategories
        /// </summary>
        [DataMember(Name="hideDirectLinkedCategories", EmitDefaultValue=false)]
        public bool? HideDirectLinkedCategories { get; set; }

        /// <summary>
        /// Gets or Sets PropertyValues
        /// </summary>
        [DataMember(Name="propertyValues", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelPropertyValue> PropertyValues { get; set; }

        /// <summary>
        /// Gets or Sets Currency
        /// </summary>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or Sets StartPrice
        /// </summary>
        [DataMember(Name="startPrice", EmitDefaultValue=false)]
        public double? StartPrice { get; set; }

        /// <summary>
        /// Gets or Sets EndPrice
        /// </summary>
        [DataMember(Name="endPrice", EmitDefaultValue=false)]
        public double? EndPrice { get; set; }

        /// <summary>
        /// Gets or Sets Skip
        /// </summary>
        [DataMember(Name="skip", EmitDefaultValue=false)]
        public int? Skip { get; set; }

        /// <summary>
        /// Gets or Sets Take
        /// </summary>
        [DataMember(Name="take", EmitDefaultValue=false)]
        public int? Take { get; set; }

        /// <summary>
        /// Gets or Sets IndexDate
        /// </summary>
        [DataMember(Name="indexDate", EmitDefaultValue=false)]
        public DateTime? IndexDate { get; set; }

        /// <summary>
        /// Gets or Sets PricelistId
        /// </summary>
        [DataMember(Name="pricelistId", EmitDefaultValue=false)]
        public string PricelistId { get; set; }

        /// <summary>
        /// Gets or Sets PricelistIds
        /// </summary>
        [DataMember(Name="pricelistIds", EmitDefaultValue=false)]
        public List<string> PricelistIds { get; set; }

        /// <summary>
        /// Gets or Sets Terms
        /// </summary>
        [DataMember(Name="terms", EmitDefaultValue=false)]
        public List<string> Terms { get; set; }

        /// <summary>
        /// Gets or Sets Facets
        /// </summary>
        [DataMember(Name="facets", EmitDefaultValue=false)]
        public List<string> Facets { get; set; }

        /// <summary>
        /// Gets or Sets Outline
        /// </summary>
        [DataMember(Name="outline", EmitDefaultValue=false)]
        public string Outline { get; set; }

        /// <summary>
        /// Gets or Sets WithHidden
        /// </summary>
        [DataMember(Name="withHidden", EmitDefaultValue=false)]
        public bool? WithHidden { get; set; }

        /// <summary>
        /// Gets or Sets OnlyBuyable
        /// </summary>
        [DataMember(Name="onlyBuyable", EmitDefaultValue=false)]
        public bool? OnlyBuyable { get; set; }

        /// <summary>
        /// Gets or Sets OnlyWithTrackingInventory
        /// </summary>
        [DataMember(Name="onlyWithTrackingInventory", EmitDefaultValue=false)]
        public bool? OnlyWithTrackingInventory { get; set; }

        /// <summary>
        /// Gets or Sets ProductType
        /// </summary>
        [DataMember(Name="productType", EmitDefaultValue=false)]
        public string ProductType { get; set; }

        /// <summary>
        /// Gets or Sets ProductTypes
        /// </summary>
        [DataMember(Name="productTypes", EmitDefaultValue=false)]
        public List<string> ProductTypes { get; set; }

        /// <summary>
        /// Gets or Sets StartDateFrom
        /// </summary>
        [DataMember(Name="startDateFrom", EmitDefaultValue=false)]
        public DateTime? StartDateFrom { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceDomainCatalogModelSearchCriteria {\n");
            sb.Append("  StoreId: ").Append(StoreId).Append("\n");
            sb.Append("  ResponseGroup: ").Append(ResponseGroup).Append("\n");
            sb.Append("  Keyword: ").Append(Keyword).Append("\n");
            sb.Append("  SearchInChildren: ").Append(SearchInChildren).Append("\n");
            sb.Append("  SearchInVariations: ").Append(SearchInVariations).Append("\n");
            sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
            sb.Append("  CategoryIds: ").Append(CategoryIds).Append("\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  CatalogIds: ").Append(CatalogIds).Append("\n");
            sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Sort: ").Append(Sort).Append("\n");
            sb.Append("  SortInfos: ").Append(SortInfos).Append("\n");
            sb.Append("  HideDirectLinkedCategories: ").Append(HideDirectLinkedCategories).Append("\n");
            sb.Append("  PropertyValues: ").Append(PropertyValues).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  StartPrice: ").Append(StartPrice).Append("\n");
            sb.Append("  EndPrice: ").Append(EndPrice).Append("\n");
            sb.Append("  Skip: ").Append(Skip).Append("\n");
            sb.Append("  Take: ").Append(Take).Append("\n");
            sb.Append("  IndexDate: ").Append(IndexDate).Append("\n");
            sb.Append("  PricelistId: ").Append(PricelistId).Append("\n");
            sb.Append("  PricelistIds: ").Append(PricelistIds).Append("\n");
            sb.Append("  Terms: ").Append(Terms).Append("\n");
            sb.Append("  Facets: ").Append(Facets).Append("\n");
            sb.Append("  Outline: ").Append(Outline).Append("\n");
            sb.Append("  WithHidden: ").Append(WithHidden).Append("\n");
            sb.Append("  OnlyBuyable: ").Append(OnlyBuyable).Append("\n");
            sb.Append("  OnlyWithTrackingInventory: ").Append(OnlyWithTrackingInventory).Append("\n");
            sb.Append("  ProductType: ").Append(ProductType).Append("\n");
            sb.Append("  ProductTypes: ").Append(ProductTypes).Append("\n");
            sb.Append("  StartDateFrom: ").Append(StartDateFrom).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainCatalogModelSearchCriteria);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainCatalogModelSearchCriteria instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainCatalogModelSearchCriteria to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainCatalogModelSearchCriteria other)
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
                    this.ResponseGroup == other.ResponseGroup ||
                    this.ResponseGroup != null &&
                    this.ResponseGroup.Equals(other.ResponseGroup)
                ) && 
                (
                    this.Keyword == other.Keyword ||
                    this.Keyword != null &&
                    this.Keyword.Equals(other.Keyword)
                ) && 
                (
                    this.SearchInChildren == other.SearchInChildren ||
                    this.SearchInChildren != null &&
                    this.SearchInChildren.Equals(other.SearchInChildren)
                ) && 
                (
                    this.SearchInVariations == other.SearchInVariations ||
                    this.SearchInVariations != null &&
                    this.SearchInVariations.Equals(other.SearchInVariations)
                ) && 
                (
                    this.CategoryId == other.CategoryId ||
                    this.CategoryId != null &&
                    this.CategoryId.Equals(other.CategoryId)
                ) && 
                (
                    this.CategoryIds == other.CategoryIds ||
                    this.CategoryIds != null &&
                    this.CategoryIds.SequenceEqual(other.CategoryIds)
                ) && 
                (
                    this.CatalogId == other.CatalogId ||
                    this.CatalogId != null &&
                    this.CatalogId.Equals(other.CatalogId)
                ) && 
                (
                    this.CatalogIds == other.CatalogIds ||
                    this.CatalogIds != null &&
                    this.CatalogIds.SequenceEqual(other.CatalogIds)
                ) && 
                (
                    this.LanguageCode == other.LanguageCode ||
                    this.LanguageCode != null &&
                    this.LanguageCode.Equals(other.LanguageCode)
                ) && 
                (
                    this.Code == other.Code ||
                    this.Code != null &&
                    this.Code.Equals(other.Code)
                ) && 
                (
                    this.Sort == other.Sort ||
                    this.Sort != null &&
                    this.Sort.Equals(other.Sort)
                ) && 
                (
                    this.SortInfos == other.SortInfos ||
                    this.SortInfos != null &&
                    this.SortInfos.SequenceEqual(other.SortInfos)
                ) && 
                (
                    this.HideDirectLinkedCategories == other.HideDirectLinkedCategories ||
                    this.HideDirectLinkedCategories != null &&
                    this.HideDirectLinkedCategories.Equals(other.HideDirectLinkedCategories)
                ) && 
                (
                    this.PropertyValues == other.PropertyValues ||
                    this.PropertyValues != null &&
                    this.PropertyValues.SequenceEqual(other.PropertyValues)
                ) && 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
                ) && 
                (
                    this.StartPrice == other.StartPrice ||
                    this.StartPrice != null &&
                    this.StartPrice.Equals(other.StartPrice)
                ) && 
                (
                    this.EndPrice == other.EndPrice ||
                    this.EndPrice != null &&
                    this.EndPrice.Equals(other.EndPrice)
                ) && 
                (
                    this.Skip == other.Skip ||
                    this.Skip != null &&
                    this.Skip.Equals(other.Skip)
                ) && 
                (
                    this.Take == other.Take ||
                    this.Take != null &&
                    this.Take.Equals(other.Take)
                ) && 
                (
                    this.IndexDate == other.IndexDate ||
                    this.IndexDate != null &&
                    this.IndexDate.Equals(other.IndexDate)
                ) && 
                (
                    this.PricelistId == other.PricelistId ||
                    this.PricelistId != null &&
                    this.PricelistId.Equals(other.PricelistId)
                ) && 
                (
                    this.PricelistIds == other.PricelistIds ||
                    this.PricelistIds != null &&
                    this.PricelistIds.SequenceEqual(other.PricelistIds)
                ) && 
                (
                    this.Terms == other.Terms ||
                    this.Terms != null &&
                    this.Terms.SequenceEqual(other.Terms)
                ) && 
                (
                    this.Facets == other.Facets ||
                    this.Facets != null &&
                    this.Facets.SequenceEqual(other.Facets)
                ) && 
                (
                    this.Outline == other.Outline ||
                    this.Outline != null &&
                    this.Outline.Equals(other.Outline)
                ) && 
                (
                    this.WithHidden == other.WithHidden ||
                    this.WithHidden != null &&
                    this.WithHidden.Equals(other.WithHidden)
                ) && 
                (
                    this.OnlyBuyable == other.OnlyBuyable ||
                    this.OnlyBuyable != null &&
                    this.OnlyBuyable.Equals(other.OnlyBuyable)
                ) && 
                (
                    this.OnlyWithTrackingInventory == other.OnlyWithTrackingInventory ||
                    this.OnlyWithTrackingInventory != null &&
                    this.OnlyWithTrackingInventory.Equals(other.OnlyWithTrackingInventory)
                ) && 
                (
                    this.ProductType == other.ProductType ||
                    this.ProductType != null &&
                    this.ProductType.Equals(other.ProductType)
                ) && 
                (
                    this.ProductTypes == other.ProductTypes ||
                    this.ProductTypes != null &&
                    this.ProductTypes.SequenceEqual(other.ProductTypes)
                ) && 
                (
                    this.StartDateFrom == other.StartDateFrom ||
                    this.StartDateFrom != null &&
                    this.StartDateFrom.Equals(other.StartDateFrom)
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

                if (this.ResponseGroup != null)
                    hash = hash * 59 + this.ResponseGroup.GetHashCode();

                if (this.Keyword != null)
                    hash = hash * 59 + this.Keyword.GetHashCode();

                if (this.SearchInChildren != null)
                    hash = hash * 59 + this.SearchInChildren.GetHashCode();

                if (this.SearchInVariations != null)
                    hash = hash * 59 + this.SearchInVariations.GetHashCode();

                if (this.CategoryId != null)
                    hash = hash * 59 + this.CategoryId.GetHashCode();

                if (this.CategoryIds != null)
                    hash = hash * 59 + this.CategoryIds.GetHashCode();

                if (this.CatalogId != null)
                    hash = hash * 59 + this.CatalogId.GetHashCode();

                if (this.CatalogIds != null)
                    hash = hash * 59 + this.CatalogIds.GetHashCode();

                if (this.LanguageCode != null)
                    hash = hash * 59 + this.LanguageCode.GetHashCode();

                if (this.Code != null)
                    hash = hash * 59 + this.Code.GetHashCode();

                if (this.Sort != null)
                    hash = hash * 59 + this.Sort.GetHashCode();

                if (this.SortInfos != null)
                    hash = hash * 59 + this.SortInfos.GetHashCode();

                if (this.HideDirectLinkedCategories != null)
                    hash = hash * 59 + this.HideDirectLinkedCategories.GetHashCode();

                if (this.PropertyValues != null)
                    hash = hash * 59 + this.PropertyValues.GetHashCode();

                if (this.Currency != null)
                    hash = hash * 59 + this.Currency.GetHashCode();

                if (this.StartPrice != null)
                    hash = hash * 59 + this.StartPrice.GetHashCode();

                if (this.EndPrice != null)
                    hash = hash * 59 + this.EndPrice.GetHashCode();

                if (this.Skip != null)
                    hash = hash * 59 + this.Skip.GetHashCode();

                if (this.Take != null)
                    hash = hash * 59 + this.Take.GetHashCode();

                if (this.IndexDate != null)
                    hash = hash * 59 + this.IndexDate.GetHashCode();

                if (this.PricelistId != null)
                    hash = hash * 59 + this.PricelistId.GetHashCode();

                if (this.PricelistIds != null)
                    hash = hash * 59 + this.PricelistIds.GetHashCode();

                if (this.Terms != null)
                    hash = hash * 59 + this.Terms.GetHashCode();

                if (this.Facets != null)
                    hash = hash * 59 + this.Facets.GetHashCode();

                if (this.Outline != null)
                    hash = hash * 59 + this.Outline.GetHashCode();

                if (this.WithHidden != null)
                    hash = hash * 59 + this.WithHidden.GetHashCode();

                if (this.OnlyBuyable != null)
                    hash = hash * 59 + this.OnlyBuyable.GetHashCode();

                if (this.OnlyWithTrackingInventory != null)
                    hash = hash * 59 + this.OnlyWithTrackingInventory.GetHashCode();

                if (this.ProductType != null)
                    hash = hash * 59 + this.ProductType.GetHashCode();

                if (this.ProductTypes != null)
                    hash = hash * 59 + this.ProductTypes.GetHashCode();

                if (this.StartDateFrom != null)
                    hash = hash * 59 + this.StartDateFrom.GetHashCode();

                return hash;
            }
        }

    }
}
