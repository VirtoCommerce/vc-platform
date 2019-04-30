using System;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.Swagger;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public class PolymorphismDocumentFilter : IDocumentFilter
    {
        private readonly Type[] _types;

        public PolymorphismDocumentFilter(Type[] types)
        {
            _types = types;
        }

        [CLSCompliant(false)]
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, System.Web.Http.Description.IApiExplorer apiExplorer)
        {
            foreach (var type in _types)
            {
                RegisterSubClasses(schemaRegistry, type);
            }
        }

        private static void RegisterSubClasses(SchemaRegistry schemaRegistry, Type abstractType)
        {
            var discriminatorName = "type";

            // Need to make first property character lower to avoid properties duplication because of case, as all properties in OpenApi spec are in camelCase
            discriminatorName = char.ToLowerInvariant(discriminatorName[0]) + discriminatorName.Substring(1);

            var typeName = schemaRegistry.Definitions.ContainsKey(abstractType.FullName) ? abstractType.FullName : abstractType.FriendlyId();
            var parentSchema = schemaRegistry.Definitions[typeName];

            //set up a discriminator property (it must be required)
            parentSchema.discriminator = discriminatorName;
            parentSchema.required = new List<string> { discriminatorName };

            if (!parentSchema.properties.ContainsKey(discriminatorName))
            {
                parentSchema.properties.Add(discriminatorName, new Schema { type = "string" });
            }

            //register all subclasses
            var derivedTypes = abstractType.Assembly
                                           .GetTypes()
                                           .Where(x => abstractType != x && abstractType.IsAssignableFrom(x));

            foreach (var item in derivedTypes)
            {
                schemaRegistry.GetOrRegister(item);
            }
        }
    }
}
