namespace VirtoCommerce.Foundation.Catalogs.Search
{
    using System.Runtime.Serialization;
    using System.Text;

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

        public string CacheKey
        {
            get
            {
                var key = new StringBuilder();
                key.Append("_cf:" + Key);
                foreach (var field in this.Values)
                {
                    key.Append("_cf:" + field.Id);
                }
                return key.ToString();
            }
        }
    }
}
