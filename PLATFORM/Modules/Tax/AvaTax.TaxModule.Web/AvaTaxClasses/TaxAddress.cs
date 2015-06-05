namespace AvaTaxCalcREST
{
    using System;

    [Serializable]
    public class TaxAddress // Result object
    {
        public string Address { get; set; }

        public string AddressCode { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string TaxRegionId { get; set; }

        public string JurisCode { get; set; }
    }
}
