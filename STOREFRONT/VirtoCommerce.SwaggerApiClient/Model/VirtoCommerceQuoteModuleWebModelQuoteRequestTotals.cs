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
  public class VirtoCommerceQuoteModuleWebModelQuoteRequestTotals {
    
    /// <summary>
    /// Original subtotal tier quantity * sale price
    /// </summary>
    /// <value>Original subtotal tier quantity * sale price</value>
    [DataMember(Name="originalSubTotalExlTax", EmitDefaultValue=false)]
    public double? OriginalSubTotalExlTax { get; set; }

    
    /// <summary>
    /// Items proposal tier quantity * proposal price
    /// </summary>
    /// <value>Items proposal tier quantity * proposal price</value>
    [DataMember(Name="subTotalExlTax", EmitDefaultValue=false)]
    public double? SubTotalExlTax { get; set; }

    
    /// <summary>
    /// Gets or Sets ShippingTotal
    /// </summary>
    [DataMember(Name="shippingTotal", EmitDefaultValue=false)]
    public double? ShippingTotal { get; set; }

    
    /// <summary>
    /// Gets or Sets DiscountTotal
    /// </summary>
    [DataMember(Name="discountTotal", EmitDefaultValue=false)]
    public double? DiscountTotal { get; set; }

    
    /// <summary>
    /// Gets or Sets TaxTotal
    /// </summary>
    [DataMember(Name="taxTotal", EmitDefaultValue=false)]
    public double? TaxTotal { get; set; }

    
    /// <summary>
    /// Adjustment SubTotalOriginalExlTax -  SubTotalExlTax
    /// </summary>
    /// <value>Adjustment SubTotalOriginalExlTax -  SubTotalExlTax</value>
    [DataMember(Name="adjustmentQuoteExlTax", EmitDefaultValue=false)]
    public double? AdjustmentQuoteExlTax { get; set; }

    
    /// <summary>
    /// Grand total SubTotalExlTax + shipping - discount
    /// </summary>
    /// <value>Grand total SubTotalExlTax + shipping - discount</value>
    [DataMember(Name="grandTotalExlTax", EmitDefaultValue=false)]
    public double? GrandTotalExlTax { get; set; }

    
    /// <summary>
    /// Grand total subtotal + shipping - discount + tax
    /// </summary>
    /// <value>Grand total subtotal + shipping - discount + tax</value>
    [DataMember(Name="grandTotalInclTax", EmitDefaultValue=false)]
    public double? GrandTotalInclTax { get; set; }

    

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VirtoCommerceQuoteModuleWebModelQuoteRequestTotals {\n");
      
      sb.Append("  OriginalSubTotalExlTax: ").Append(OriginalSubTotalExlTax).Append("\n");
      
      sb.Append("  SubTotalExlTax: ").Append(SubTotalExlTax).Append("\n");
      
      sb.Append("  ShippingTotal: ").Append(ShippingTotal).Append("\n");
      
      sb.Append("  DiscountTotal: ").Append(DiscountTotal).Append("\n");
      
      sb.Append("  TaxTotal: ").Append(TaxTotal).Append("\n");
      
      sb.Append("  AdjustmentQuoteExlTax: ").Append(AdjustmentQuoteExlTax).Append("\n");
      
      sb.Append("  GrandTotalExlTax: ").Append(GrandTotalExlTax).Append("\n");
      
      sb.Append("  GrandTotalInclTax: ").Append(GrandTotalInclTax).Append("\n");
      
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
