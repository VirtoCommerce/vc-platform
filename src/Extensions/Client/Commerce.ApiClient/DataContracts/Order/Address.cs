namespace VirtoCommerce.ApiClient.DataContracts.Order
{
    public class Address
    {
        public string Id { get; set; }

        public string AddressType { get; set; }

        public string Organization { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string CustomerOrderId { get; set; }

        public string ShipmentId { get; set; }

        public string PaymentInId { get; set; }
    }
}