using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Catalog.Model
{
    public class Catalog : Entity
    {
        public string Name { get; set; }
        public bool Virtual { get; set; }
        public ICollection<CatalogLanguage> Languages { get; set; }
		public ICollection<PropertyValue> PropertyValues { get; set; }
    }
}