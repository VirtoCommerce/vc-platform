using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.QuoteModule.Web.Model
{
    public class QuoteRequestTotals
    {
        /// <summary>
        /// Grand total subtotal + shipping - discount + tax
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// Items proposal tier quantity * proposal price
        /// </summary>
        public decimal SubTotal { get; set; }
        public decimal ShippingTotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal TaxTotal { get; set; }
    }
}