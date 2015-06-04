namespace VirtoCommerce.ApiClient.DataContracts.Cart
{
    public enum AddressType
    {
        Billing = 1,
        Shipping = 2,
        BillingAndShipping = Billing | Shipping
    }
}