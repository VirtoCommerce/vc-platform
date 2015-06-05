namespace AvaTaxCalcREST
{
    using System;

    [Serializable]
    public class Line
    {
        public string LineNo { get; set; } // Required

        public string DestinationCode { get; set; } // Required

        public string OriginCode { get; set; } // Required

        public string ItemCode { get; set; } // Required

        public decimal Qty { get; set; } // Required

        public decimal Amount { get; set; } // Required

        public string TaxCode { get; set; } // Best practice

        public string CustomerUsageType { get; set; }

        public TaxOverrideDef TaxOverride { get; set; }

        public string Description { get; set; } // Best Practice

        public bool Discounted { get; set; }

        public bool TaxIncluded { get; set; }
        
        public string BusinessIdentificationNo { get; set; }

        public string Ref1 { get; set; }

        public string Ref2 { get; set; }
    }
}
