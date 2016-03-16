using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public interface IHasProperties
    {
        ICollection<CatalogProperty> Properties { get; }
    }
}
