using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicPropertyDictionaryItem : AuditableEntity
    {
        public string PropertyId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Item names for different languages.
        /// </summary>
        public DynamicPropertyDictionaryItemName[] DisplayNames { get; set; }
    }
}
