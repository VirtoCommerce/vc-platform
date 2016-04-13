using System.Collections.Generic;

namespace VirtoCommerce.Domain.Catalog.Model
{
    public class Outline
    {
        public ICollection<OutlineItem> Items { get; set; }

        public override string ToString()
        {
            return Items == null ? null : string.Join("/", Items);
        }
    }
}
