using System.Runtime.Serialization;

namespace VirtoCommerce.Foundation.Catalogs.Search
{
    [DataContract]
    public class ChildCategoryFilter
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Outline { get; set; }
    }
}