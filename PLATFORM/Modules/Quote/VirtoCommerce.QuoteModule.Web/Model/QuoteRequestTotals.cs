using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.QuoteModule.Web.Model
{
    public class QuoteRequestTotals
    {
        /// <summary>
        /// Original subtotal tier quantity * sale price 
        /// </summary>
        public decimal OriginalSubTotalExlTax { get; set; }
        /// <summary>
        /// Items proposal tier quantity * proposal price
        /// </summary>
        public decimal SubTotalExlTax { get; set; }
        public decimal ShippingTotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal TaxTotal { get; set; }

        /// <summary>
        /// Adjustment SubTotalOriginalExlTax -  SubTotalExlTax
        /// </summary>
        public decimal AdjustmentQuoteExlTax { get; set; }

        /// <summary>
        /// Grand total SubTotalExlTax + shipping - discount
        /// </summary>
        public decimal GrandTotalExlTax { get; set; }
        /// <summary>
        /// Grand total subtotal + shipping - discount + tax
        /// </summary>
        public decimal GrandTotalInclTax { get; set; }
    }
}