using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AvaTaxCalcREST
{
    using System;

    [Serializable]
    public class GetTaxResult // Result of tax/get verb POST
    {
        public string DocCode { get; set; }

        public DateTime DocDate { get; set; }

        public DateTime TimeStamp { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TotalDiscount { get; set; }

        public decimal TotalExemption { get; set; }

        public decimal TotalTaxable { get; set; }

        public decimal TotalTax { get; set; }

        public decimal TotalTaxCalculated { get; set; }

        public DateTime TaxDate { get; set; }

        public ICollection<TaxLine> TaxLines { get; set; }

        public ICollection<TaxLine> TaxSummary { get; set; }

        public ICollection<TaxAddress> TaxAddresses { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SeverityLevel ResultCode { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
