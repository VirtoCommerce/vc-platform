namespace VirtoCommerce.ApiClient.DataContracts.Cart
{
    public class PaymentMethod
    {
        public string GatewayCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string IconUrl { get; set; }

        public int Priority { get; set; }

        public string Type { get; set; }

        public string Group { get; set; }

        public bool IsAvailableForPartial { get; set; }
    }
}