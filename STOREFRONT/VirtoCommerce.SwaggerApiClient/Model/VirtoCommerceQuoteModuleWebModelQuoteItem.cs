using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.SwaggerApiClient.Model {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class VirtoCommerceQuoteModuleWebModelQuoteItem {
    
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
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
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
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}


}
