using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AvaTaxCalcREST
{
    using System;

    [Serializable]
    public class CancelTaxResult
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public SeverityLevel ResultCode { get; set; }

        public string TransactionId { get; set; }

        public string DocId { get; set; }

        public Message[] Messages { get; set; }
    }
}
