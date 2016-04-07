using System.Collections.Generic;

namespace VirtoCommerce.Domain.Catalog.Model
{
    public interface IHasOutlines
    {
        ICollection<Outline> Outlines { get; set; }
    }
}
