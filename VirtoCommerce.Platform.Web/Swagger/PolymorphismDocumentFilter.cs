using System;
using System.Collections.Generic;
using Swashbuckle.Swagger;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public class PolymorphismDocumentFilter : IDocumentFilter
    {
        private readonly string _moduleName;
        private readonly IPolymorphismRegistrar _polymorphismRegistrar;
        private readonly bool _useFullTypeNames;

        public PolymorphismDocumentFilter(IPolymorphismRegistrar polymorphismRegistrar, string moduleName, bool useFullTypeNames)
        {
            _polymorphismRegistrar = polymorphismRegistrar ?? throw new ArgumentNullException(nameof(polymorphismRegistrar));
            _moduleName = moduleName;
            _useFullTypeNames = useFullTypeNames;
        }

        [CLSCompliant(false)]
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, System.Web.Http.Description.IApiExplorer apiExplorer)
        {
            foreach (var polymorphicBaseTypeInfo in _polymorphismRegistrar.GetPolymorphicBaseTypes(_moduleName))
            {
                RegisterSubClasses(schemaRegistry, polymorphicBaseTypeInfo);
            }
        }

        private void RegisterSubClasses(SchemaRegistry schemaRegistry, IPolymorphicBaseTypeInfo polymorphicBaseTypeInfo)
        {
            var abstractType = polymorphicBaseTypeInfo.Type;
            var discriminatorName = polymorphicBaseTypeInfo.DiscriminatorName;

            // Need to make first property character lower to avoid properties duplication because of case, as all properties in OpenApi spec are in camelCase
            discriminatorName = char.ToLowerInvariant(discriminatorName[0]) + discriminatorName.Substring(1);

            var typeName = _useFullTypeNames ? abstractType.FullName : abstractType.FriendlyId();
            var parentSchema = schemaRegistry.Definitions[typeName];

            //set up a discriminator property (it must be required)
            parentSchema.discriminator = discriminatorName;
            parentSchema.required = new List<string> { discriminatorName };

            if (!parentSchema.properties.ContainsKey(discriminatorName))
            {
                parentSchema.properties.Add(discriminatorName, new Schema { type = "string" });
            }

            //register all subclasses
            var derivedTypes = polymorphicBaseTypeInfo.DerivedTypes;

            foreach (var item in derivedTypes)
            {
                schemaRegistry.GetOrRegister(item);
            }
        }
    }
}
