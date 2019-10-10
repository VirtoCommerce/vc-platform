using System;

namespace VirtoCommerce.Platform.Core.Swagger
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SwaggerSchemaIdAttribute : Attribute
    {
        public string Id { get; private set; }

        public SwaggerSchemaIdAttribute(string id)
        {
            Id = id;
        }
    }
}
