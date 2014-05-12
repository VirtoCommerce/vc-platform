namespace VirtoCommerce.Foundation.Catalogs.Search
{
    using System.Runtime.Serialization;
    using VirtoCommerce.Foundation.Search;

    [DataContract]
    public class CategoryFilter : ISearchFilter
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public CategoryFilterValue[] Values
        {
            get;set;
        }
    }
}
