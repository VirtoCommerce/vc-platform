using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AvaTaxCalcREST
{
    using System;

    public enum CancelCode
    {
        Unspecified,
        PostFailed,
        DocDeleted,
        DocVoided,
        AdjustmentCancelled
    }

    [Serializable]
    public class CancelTaxRequest
    {
        // Required for CancelTax operation
        [JsonConverter(typeof(StringEnumConverter))]
        public CancelCode CancelCode { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DocType DocType { get; set; } // Note that the only *meaningful* values for this property here are SalesInvoice, ReturnInvoice, PurchaseInvoice.

        // The document needs to be identified by either DocCode/CompanyCode (recommended) OR DocId (not recommended).
        public string CompanyCode { get; set; }

        public string DocCode { get; set; }
    }
}
