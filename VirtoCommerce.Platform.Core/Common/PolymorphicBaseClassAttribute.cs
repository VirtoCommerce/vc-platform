using System;

namespace VirtoCommerce.Platform.Core.Common
{
    /// <summary>
    /// Class marked with this attribute is marked as a base class for OpenAPI polymorphism
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PolymorphicBaseClassAttribute : Attribute
    {
        /// <summary>
        /// The property name used to store type information, in camelCase. By default: "type".
        /// </summary>
        public string DiscriminatorPropertyName { get; set; } = "type";
    }
}
