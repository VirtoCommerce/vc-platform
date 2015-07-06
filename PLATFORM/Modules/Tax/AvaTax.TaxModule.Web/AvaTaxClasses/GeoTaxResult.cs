using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AvaTaxCalcREST
{
    using System;

    [Serializable]
    public class GeoTaxResult // Result of tax/get verb GET
    {
        public decimal Rate { get; set; }

        public decimal Tax { get; set; }

        public TaxDetail[] TaxDetails { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SeverityLevel ResultCode { get; set; }

        public Message[] Messages { get; set; }
    }
}
