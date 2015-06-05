namespace AvaTaxCalcREST
{
    using System;

    [Serializable]
    public class GeoTaxResult // Result of tax/get verb GET
    {
        public decimal Rate { get; set; }

        public decimal Tax { get; set; }

        public TaxDetail[] TaxDetails { get; set; }

        public SeverityLevel ResultCode { get; set; }

        public Message[] Messages { get; set; }
    }
}
