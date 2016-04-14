using System.Collections.Generic;

namespace VirtoCommerce.Domain.Catalog.Model
{
    /// <summary>
    /// Represent full outline path from children to parent
    /// </summary>
    public class Outline
    {
        /// <summary>
        /// Outline parts
        /// </summary>
        public ICollection<OutlineItem> Items { get; set; }

        public override string ToString()
        {
            return Items == null ? null : string.Join("/", Items);
        }
    }
}
