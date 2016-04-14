using System.Collections.Generic;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.Domain.Catalog.Model
{
    /// <summary>
    /// Represents one outline element: catalog, category or product.
    /// </summary>
    public class OutlineItem : ISeoSupport
    {
        #region ISeoSupport Members

        /// <summary>
        /// Object id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Object type
        /// </summary>
        public string SeoObjectType { get; set; }

        /// <summary>
        /// All SEO records for the object
        /// </summary>
        public ICollection<SeoInfo> SeoInfos { get; set; }

        #endregion

        /// <summary>
        /// True when this object is linked to the virtual parent.
        /// </summary>
        public bool HasVirtualParent { get; set; }

        public override string ToString()
        {
            return (HasVirtualParent ? "*" : "") + Id;
        }
    }
}
