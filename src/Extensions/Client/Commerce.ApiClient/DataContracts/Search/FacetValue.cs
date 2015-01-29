namespace VirtoCommerce.ApiClient.DataContracts.Search
{
    public class FacetValue
    {
        public string Label { get; set; }
        public int Count { get; set; }
        public object Value { get; set; }
        public bool IsApplied { get; set; }
    }
}
