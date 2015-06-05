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
        public CancelCode CancelCode { get; set; }

        public DocType DocType { get; set; } // Note that the only *meaningful* values for this property here are SalesInvoice, ReturnInvoice, PurchaseInvoice.

        // The document needs to be identified by either DocCode/CompanyCode (recommended) OR DocId (not recommended).
        public string CompanyCode { get; set; }

        public string DocCode { get; set; }
    }
}
