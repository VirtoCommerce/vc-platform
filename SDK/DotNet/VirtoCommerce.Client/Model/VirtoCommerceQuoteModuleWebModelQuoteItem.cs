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
    public partial class VirtoCommerceQuoteModuleWebModelQuoteItem :  IEquatable<VirtoCommerceQuoteModuleWebModelQuoteItem>
    {
        /// <summary>
        /// Gets or Sets Currency
        /// </summary>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }

        /// <summary>
        /// Base catalog price
        /// </summary>
        /// <value>Base catalog price</value>
        [DataMember(Name="listPrice", EmitDefaultValue=false)]
        public double? ListPrice { get; set; }

        /// <summary>
        /// Sale price for buyer
        /// </summary>
        /// <value>Sale price for buyer</value>
        [DataMember(Name="salePrice", EmitDefaultValue=false)]
        public double? SalePrice { get; set; }

        /// <summary>
        /// Gets or Sets ProductId
        /// </summary>
        [DataMember(Name="productId", EmitDefaultValue=false)]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or Sets Product
        /// </summary>
        [DataMember(Name="product", EmitDefaultValue=false)]
        public VirtoCommerceDomainCatalogModelCatalogProduct Product { get; set; }

        /// <summary>
        /// Gets or Sets CatalogId
        /// </summary>
        [DataMember(Name="catalogId", EmitDefaultValue=false)]
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or Sets CategoryId
        /// </summary>
        [DataMember(Name="categoryId", EmitDefaultValue=false)]
        public string CategoryId { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Comment
        /// </summary>
        [DataMember(Name="comment", EmitDefaultValue=false)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or Sets ImageUrl
        /// </summary>
        [DataMember(Name="imageUrl", EmitDefaultValue=false)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or Sets Sku
        /// </summary>
        [DataMember(Name="sku", EmitDefaultValue=false)]
        public string Sku { get; set; }

        /// <summary>
        /// Gets or Sets TaxType
        /// </summary>
        [DataMember(Name="taxType", EmitDefaultValue=false)]
        public string TaxType { get; set; }

        /// <summary>
        /// Selected proposal tier price
        /// </summary>
        /// <value>Selected proposal tier price</value>
        [DataMember(Name="selectedTierPrice", EmitDefaultValue=false)]
        public VirtoCommerceQuoteModuleWebModelTierPrice SelectedTierPrice { get; set; }

        /// <summary>
        /// Proposal tier prices
        /// </summary>
        /// <value>Proposal tier prices</value>
        [DataMember(Name="proposalPrices", EmitDefaultValue=false)]
        public List<VirtoCommerceQuoteModuleWebModelTierPrice> ProposalPrices { get; set; }

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
            sb.Append("class VirtoCommerceQuoteModuleWebModelQuoteItem {\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  ListPrice: ").Append(ListPrice).Append("\n");
            sb.Append("  SalePrice: ").Append(SalePrice).Append("\n");
            sb.Append("  ProductId: ").Append(ProductId).Append("\n");
            sb.Append("  Product: ").Append(Product).Append("\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Comment: ").Append(Comment).Append("\n");
            sb.Append("  ImageUrl: ").Append(ImageUrl).Append("\n");
            sb.Append("  Sku: ").Append(Sku).Append("\n");
            sb.Append("  TaxType: ").Append(TaxType).Append("\n");
            sb.Append("  SelectedTierPrice: ").Append(SelectedTierPrice).Append("\n");
            sb.Append("  ProposalPrices: ").Append(ProposalPrices).Append("\n");
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
            return this.Equals(obj as VirtoCommerceQuoteModuleWebModelQuoteItem);
        }

        /// <summary>
        /// Returns true if VirtoCommerceQuoteModuleWebModelQuoteItem instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceQuoteModuleWebModelQuoteItem to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceQuoteModuleWebModelQuoteItem other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
                ) && 
                (
                    this.ListPrice == other.ListPrice ||
                    this.ListPrice != null &&
                    this.ListPrice.Equals(other.ListPrice)
                ) && 
                (
                    this.SalePrice == other.SalePrice ||
                    this.SalePrice != null &&
                    this.SalePrice.Equals(other.SalePrice)
                ) && 
                (
                    this.ProductId == other.ProductId ||
                    this.ProductId != null &&
                    this.ProductId.Equals(other.ProductId)
                ) && 
                (
                    this.Product == other.Product ||
                    this.Product != null &&
                    this.Product.Equals(other.Product)
                ) && 
                (
                    this.CatalogId == other.CatalogId ||
                    this.CatalogId != null &&
                    this.CatalogId.Equals(other.CatalogId)
                ) && 
                (
                    this.CategoryId == other.CategoryId ||
                    this.CategoryId != null &&
                    this.CategoryId.Equals(other.CategoryId)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Comment == other.Comment ||
                    this.Comment != null &&
                    this.Comment.Equals(other.Comment)
                ) && 
                (
                    this.ImageUrl == other.ImageUrl ||
                    this.ImageUrl != null &&
                    this.ImageUrl.Equals(other.ImageUrl)
                ) && 
                (
                    this.Sku == other.Sku ||
                    this.Sku != null &&
                    this.Sku.Equals(other.Sku)
                ) && 
                (
                    this.TaxType == other.TaxType ||
                    this.TaxType != null &&
                    this.TaxType.Equals(other.TaxType)
                ) && 
                (
                    this.SelectedTierPrice == other.SelectedTierPrice ||
                    this.SelectedTierPrice != null &&
                    this.SelectedTierPrice.Equals(other.SelectedTierPrice)
                ) && 
                (
                    this.ProposalPrices == other.ProposalPrices ||
                    this.ProposalPrices != null &&
                    this.ProposalPrices.SequenceEqual(other.ProposalPrices)
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

                if (this.Currency != null)
                    hash = hash * 59 + this.Currency.GetHashCode();

                if (this.ListPrice != null)
                    hash = hash * 59 + this.ListPrice.GetHashCode();

                if (this.SalePrice != null)
                    hash = hash * 59 + this.SalePrice.GetHashCode();

                if (this.ProductId != null)
                    hash = hash * 59 + this.ProductId.GetHashCode();

                if (this.Product != null)
                    hash = hash * 59 + this.Product.GetHashCode();

                if (this.CatalogId != null)
                    hash = hash * 59 + this.CatalogId.GetHashCode();

                if (this.CategoryId != null)
                    hash = hash * 59 + this.CategoryId.GetHashCode();

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.Comment != null)
                    hash = hash * 59 + this.Comment.GetHashCode();

                if (this.ImageUrl != null)
                    hash = hash * 59 + this.ImageUrl.GetHashCode();

                if (this.Sku != null)
                    hash = hash * 59 + this.Sku.GetHashCode();

                if (this.TaxType != null)
                    hash = hash * 59 + this.TaxType.GetHashCode();

                if (this.SelectedTierPrice != null)
                    hash = hash * 59 + this.SelectedTierPrice.GetHashCode();

                if (this.ProposalPrices != null)
                    hash = hash * 59 + this.ProposalPrices.GetHashCode();

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
