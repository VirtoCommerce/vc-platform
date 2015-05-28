using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class CsvImportMappingItem : ValueObject<CsvImportMappingItem>
    {
        public string EntityColumnName { get; set; }
        public string CsvColumnName { get; set; }
        public bool IsSystemProperty { get; set; }
        public bool IsRequired { get; set; }
        public string CustomValue { get; set; }
        public string StringFormat { get; set; }
        public string Locale { get; set; }
    }
}
