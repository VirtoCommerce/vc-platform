using System;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class MappingItem
    {
        public MappingItem()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string EntityColumnName { get; set; }
        public string CsvColumnName { get; set; }
        public bool IsSystemProperty { get; set; }
        public bool IsRequired { get; set; }
        public string CustomValue { get; set; }
        public string DisplayName { get; set; }
        public string StringFormat { get; set; }
        public string Locale { get; set; }

        public string ImportJobId { get; set; }
    }
}
