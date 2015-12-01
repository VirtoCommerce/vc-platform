using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.Client.Model
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class VirtoCommerceMerchandisingModuleWebModelPrice : IEquatable<VirtoCommerceMerchandisingModuleWebModelPrice>
    {
        
        /// <summary>
        /// Gets or sets the value of original price
        /// </summary>
        /// <value>Gets or sets the value of original price</value>
        [DataMember(Name="list", EmitDefaultValue=false)]
        public double? List { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of minimum catalog item quantity for current price
        /// </summary>
        /// <value>Gets or sets the value of minimum catalog item quantity for current price</value>
        [DataMember(Name="minQuantity", EmitDefaultValue=false)]
        public int? MinQuantity { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of pricelist id
        /// </summary>
        /// <value>Gets or sets the value of pricelist id</value>
        [DataMember(Name="pricelistId", EmitDefaultValue=false)]
        public string PricelistId { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of catalog item id
        /// </summary>
        /// <value>Gets or sets the value of catalog item id</value>
        [DataMember(Name="productId", EmitDefaultValue=false)]
        public string ProductId { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value for sale price (include static discount amount)
        /// </summary>
        /// <value>Gets or sets the value for sale price (include static discount amount)</value>
        [DataMember(Name="sale", EmitDefaultValue=false)]
        public double? Sale { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of price currency
        /// </summary>
        /// <value>Gets or sets the value of price currency</value>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelPrice {\n");
            sb.Append("  List: ").Append(List).Append("\n");
            sb.Append("  MinQuantity: ").Append(MinQuantity).Append("\n");
            sb.Append("  PricelistId: ").Append(PricelistId).Append("\n");
            sb.Append("  ProductId: ").Append(ProductId).Append("\n");
            sb.Append("  Sale: ").Append(Sale).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelPrice);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelPrice instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelPrice to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelPrice other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.List == other.List ||
                    this.List != null &&
                    this.List.Equals(other.List)
                ) && 
                (
                    this.MinQuantity == other.MinQuantity ||
                    this.MinQuantity != null &&
                    this.MinQuantity.Equals(other.MinQuantity)
                ) && 
                (
                    this.PricelistId == other.PricelistId ||
                    this.PricelistId != null &&
                    this.PricelistId.Equals(other.PricelistId)
                ) && 
                (
                    this.ProductId == other.ProductId ||
                    this.ProductId != null &&
                    this.ProductId.Equals(other.ProductId)
                ) && 
                (
                    this.Sale == other.Sale ||
                    this.Sale != null &&
                    this.Sale.Equals(other.Sale)
                ) && 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
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
                
                if (this.List != null)
                    hash = hash * 57 + this.List.GetHashCode();
                
                if (this.MinQuantity != null)
                    hash = hash * 57 + this.MinQuantity.GetHashCode();
                
                if (this.PricelistId != null)
                    hash = hash * 57 + this.PricelistId.GetHashCode();
                
                if (this.ProductId != null)
                    hash = hash * 57 + this.ProductId.GetHashCode();
                
                if (this.Sale != null)
                    hash = hash * 57 + this.Sale.GetHashCode();
                
                if (this.Currency != null)
                    hash = hash * 57 + this.Currency.GetHashCode();
                
                return hash;
            }
        }

    }


}
