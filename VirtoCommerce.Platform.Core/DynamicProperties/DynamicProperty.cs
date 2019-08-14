using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicProperty : AuditableEntity
    {
        public string Name { get; set; }
        /// <summary>
        /// dynamic property description
        /// </summary>
        public string Description { get; set; }
        public string ObjectType { get; set; }
        /// <summary>
        /// Defines whether a property supports multiple values.
        /// </summary>
        public bool IsArray { get; set; }
        /// <summary>
        /// Dictionary has a predefined set of values. User can select one or more of them and cannot enter arbitrary values.
        /// </summary>
        public bool IsDictionary { get; set; }
        /// <summary>
        /// For multilingual properties user can enter different values for each of registered languages.
        /// </summary>
        public bool IsMultilingual { get; set; }
        public bool IsRequired { get; set; }
        public int? DisplayOrder { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DynamicPropertyValueType ValueType { get; set; }

        /// <summary>
        /// Property names for different languages.
        /// </summary>
        public DynamicPropertyName[] DisplayNames { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", Name ?? "n/a");
        }

        public virtual DynamicProperty Clone()
        {
            var result = MemberwiseClone() as DynamicProperty;
            if (DisplayNames != null)
            {
                result.DisplayNames = DisplayNames.Select(x => x.Clone() as DynamicPropertyName).ToArray();
            }
            return result;

        }
    }
}
