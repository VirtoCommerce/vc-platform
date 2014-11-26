using System.Collections.Generic;
using VirtoCommerce.CatalogModule.Model;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class ImportJob
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CatalogId { get; set; }
        public string TemplatePath { get; set; }
        public int MaxErrorsCount { get; set; }
        public int ImportStep { get; set; }
        public int ImportCount { get; set; }
        public int StartIndex { get; set; }
        public string ColumnDelimiter { get; set; }
        public string EntityImporter { get; set; } // product, sku, bundle, package, dynamickit, category, association, price, customer, inventory, itemrelation
        public string PropertySetId { get; set; }
        public ICollection<MappingItem> PropertiesMap { get; set; }
    }
}
