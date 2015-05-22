namespace VirtoCommerce.ApiClient.DataContracts.Orders
{
    public class OperationProperty
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public string Locale { get; set; }

        public PropertyValueType ValueType { get; set; }
    }
}