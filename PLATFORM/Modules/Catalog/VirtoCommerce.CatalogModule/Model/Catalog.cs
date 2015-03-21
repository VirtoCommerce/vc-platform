using System.Collections.Generic;

namespace VirtoCommerce.CatalogModule.Model
{
    public class Catalog
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Virtual { get; set; }
        public ICollection<CatalogLanguage> Languages { get; set; }
    }
}