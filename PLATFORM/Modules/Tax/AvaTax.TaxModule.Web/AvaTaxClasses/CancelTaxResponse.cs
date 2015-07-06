using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AvaTaxCalcREST
{
    using System;

    [Serializable]
    public class CancelTaxResponse
    {
        public CancelTaxResult CancelTaxResult { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SeverityLevel ResultCode { get; set; }

        public Message[] Messages { get; set; }
    }
}
