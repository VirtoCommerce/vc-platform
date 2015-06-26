using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AvaTaxCalcREST
{
    using System;

    public enum DocType
    {
        SalesOrder,
        SalesInvoice,
        ReturnOrder,
        ReturnInvoice,
        PurchaseOrder,
        PurchaseInvoice,
        ReverseChargeOrder,
        ReverseChargeInvoice
    }

    public enum DetailLevel
    {
        Tax,
        Document,
        Line,
        Diagnostic
    }

    public enum SystemCustomerUsageType
    {
        L, // "Other",
        A, // "Federal government",
        B, // "State government",
        C, // "Tribe / Status Indian / Indian Band",
        D, // "Foreign diplomat",
        E, // "Charitable or benevolent organization",
        F, // "Regligious or educational organization",
        G, // "Resale",
        H, // "Commercial agricultural production",
        I, // "Industrial production / manufacturer",
        J, // "Direct pay permit",
        K, // "Direct Mail",
        N, // "Local Government",
        P, // "Commercial Aquaculture",
        Q, // "Commercial Fishery",
        R // "Non-resident"
    }

    [Serializable]
    public class GetTaxRequest
    {
        // Required for tax calculation
        public string DocDate { get; set; }

        public string CustomerCode { get; set; }

        public ICollection<Address> Addresses { get; set; }

        public ICollection<Line> Lines { get; set; }

        // Best Practice for tax calculation
        public string Client { get; set; }

        public string DocCode { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DocType DocType { get; set; }

        public string CompanyCode { get; set; }

        public bool Commit { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DetailLevel DetailLevel { get; set; }

        // Use where appropriate to the situation
        public string CustomerUsageType { get; set; }

        public string ExemptionNo { get; set; }

        public decimal Discount { get; set; }

        public string BusinessIdentificationNo { get; set; }

        public TaxOverrideDef TaxOverride { get; set; }

        public string CurrencyCode { get; set; }

        // Optional
        public string PurchaseOrderNo { get; set; }

        public string PaymentDate { get; set; }

        public string PosLaneCode { get; set; }

        public string ReferenceCode { get; set; }
    }
}
