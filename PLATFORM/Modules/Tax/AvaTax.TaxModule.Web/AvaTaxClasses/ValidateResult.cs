using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AvaTaxCalcREST
{
    using System;

    // Request for address/validate is parsed into the URI query parameters.
    [Serializable]
    public class ValidateResult
    {
        public Address Address { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SeverityLevel ResultCode { get; set; }

        public Message[] Messages { get; set; }
    }
}
