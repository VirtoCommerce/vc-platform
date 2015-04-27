namespace VirtoCommerce.ApiClient.DataContracts.Marketing
{
    public class Property
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public PropertyValueType ValueType { get; set; }
    }
}