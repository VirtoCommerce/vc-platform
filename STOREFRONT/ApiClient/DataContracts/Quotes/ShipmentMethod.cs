namespace VirtoCommerce.ApiClient.DataContracts.Quotes
{
    public class ShipmentMethod
    {
        public string ShipmentMethodCode { get; set; }

        public string OptionName { get; set; }

        public string Name { get; set; }

        public string LogoUrl { get; set; }

        public string Currency { get; set; }

        public decimal Price { get; set; }
    }
}