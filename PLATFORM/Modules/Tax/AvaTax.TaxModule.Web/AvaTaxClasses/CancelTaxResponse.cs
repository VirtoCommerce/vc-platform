namespace AvaTaxCalcREST
{
    using System;

    [Serializable]
    public class CancelTaxResponse
    {
        public CancelTaxResult CancelTaxResult { get; set; }

        public SeverityLevel ResultCode { get; set; }

        public Message[] Messages { get; set; }
    }
}
