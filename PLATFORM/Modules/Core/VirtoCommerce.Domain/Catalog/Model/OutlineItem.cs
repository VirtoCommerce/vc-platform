using System.Collections.Generic;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.Domain.Catalog.Model
{
    /// <summary>
    /// Represent outline one part
    /// </summary>
    public class OutlineItem : ISeoSupport
    {
        /// <summary>
        /// outline item object id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Mean that outline item have virtual parent
        /// </summary>
        public bool IsLinkTarget { get; set; }
        /// <summary>
        /// outline item object type
        /// </summary>
        public string SeoObjectType { get; set; }
        /// <summary>
        /// All seo infos for  object
        /// </summary>
        public ICollection<SeoInfo> SeoInfos { get; set; }

        public override string ToString()
        {
            return (IsLinkTarget ? "*" : "") + Id;
        }
    }
}
