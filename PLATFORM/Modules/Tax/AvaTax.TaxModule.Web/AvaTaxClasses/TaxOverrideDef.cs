namespace AvaTaxCalcREST
{
    using System;

    [Serializable]
    public class TaxOverrideDef // Allows tax date, amount, or exempt status to be overridden.
    {
        public string TaxOverrideType { get; set; }

        public string TaxAmount { get; set; }

        public string TaxDate { get; set; }

        public string Reason { get; set; }
    }
}
