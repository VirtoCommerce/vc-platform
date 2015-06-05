namespace AvaTaxCalcREST
{
    using System;

    [Serializable]
    public class TaxLine // Result object
    {
        public string LineNo { get; set; }

        public string TaxCode { get; set; }

        public bool Taxability { get; set; }

        public decimal Taxable { get; set; }

        public decimal Rate { get; set; }

        public decimal Tax { get; set; }

        public decimal Discount { get; set; }

        public decimal TaxCalculated { get; set; }

        public decimal Exemption { get; set; }

        public TaxDetail[] TaxDetails { get; set; }
    }
}
