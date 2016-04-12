using System.Collections.Generic;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.Domain.Catalog.Model
{
    public class OutlineItem : ISeoSupport
    {
        public string Id { get; set; }
        public bool IsLinkTarget { get; set; }
        public string SeoObjectType { get; set; }
        public ICollection<SeoInfo> SeoInfos { get; set; }

        public override string ToString()
        {
            return (IsLinkTarget ? "*" : "") + Id;
        }
    }
}
