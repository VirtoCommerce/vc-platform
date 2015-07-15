using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicProperty : AuditableEntity
    {
        public string Name { get; set; }
        public string ObjectType { get; set; }
        public bool IsArray { get; set; }
        public bool IsDictionary { get; set; }
        public bool IsMultilingual { get; set; }
        public bool IsRequired { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DynamicPropertyValueType ValueType { get; set; }

        public DynamicPropertyName[] DisplayNames { get; set; }

        public DynamicProperty Clone()
        {
            return new DynamicProperty
            {
                Id = Id,
                CreatedDate = CreatedDate,
                ModifiedDate = ModifiedDate,
                CreatedBy = CreatedBy,
                ModifiedBy = ModifiedBy,
                Name = Name,
                ObjectType = ObjectType,
                IsArray = IsArray,
                IsDictionary = IsDictionary,
                IsMultilingual = IsMultilingual,
                IsRequired = IsRequired,
                ValueType = ValueType,
                DisplayNames = DisplayNames == null ? null : DisplayNames.Select(n => n.Clone()).ToArray(),
            };
        }
    }
}
