using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Catalog.Model;
namespace VirtoCommerce.CatalogModule.Web.Model
{
    /// <summary>
    /// The actual property value assigned to concrete merchandising entity. 
    /// </summary>
    public class PropertyValue
    {
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the name of the property that this value belongs to.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
		public string PropertyName { get; set; }
        /// <summary>
        /// Gets or sets the id of the property that this value belongs to.
        /// </summary>
        /// <value>
        /// The  property id.
        /// </value>
        public string PropertyId { get; set; }
        /// <summary>
        /// Gets or sets the language of this property value.
        /// </summary>
        /// <value>
        /// The language code.
        /// </value>
		public string LanguageCode { get; set; }
        /// <summary>
        /// Gets or sets the value of this dictionary value in default language.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
		public string Alias { get; set; }
        /// <summary>
        /// Gets or sets the type of the value.
        /// </summary>
        /// <value>
        /// The type of the value.
        /// </value>
		[JsonConverter(typeof(StringEnumConverter))]
		public PropertyValueType ValueType { get; set; }
        /// <summary>
        /// Gets or sets the value id in case this value is for property which supports dictionary values.
        /// </summary>
        /// <value>
        /// The value identifier.
        /// </value>
		public string ValueId { get; set; }
        /// <summary>
        /// Gets or sets the actual value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// System flag used to mark that object was inherited from other
        /// </summary>
        public bool IsInherited { get; set; }
    }
}