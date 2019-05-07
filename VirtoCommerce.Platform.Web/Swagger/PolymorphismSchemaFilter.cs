using System;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.Swagger;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public class PolymorphismSchemaFilter : ISchemaFilter
    {
        private readonly string _moduleName;
        private readonly IPolymorphismRegistrar _polymorphismRegistrar;
        private readonly Lazy<HashSet<Type>> _derivedTypes;
        private readonly bool _useFullTypeNames;

        public PolymorphismSchemaFilter(IPolymorphismRegistrar polymorphismRegistrar, string moduleName, bool useFullTypeNames)
        {
            _polymorphismRegistrar = polymorphismRegistrar ?? throw new ArgumentNullException(nameof(polymorphismRegistrar));
            _moduleName = moduleName;
            _derivedTypes = new Lazy<HashSet<Type>>(Init);
            _useFullTypeNames = useFullTypeNames;
        }

        private HashSet<Type> Init()
        {
            var result = new HashSet<Type>();

            result.AddRange(_polymorphismRegistrar.GetPolymorphicBaseTypes(_moduleName).SelectMany(x => x.DerivedTypes).Distinct());

            return result;
        }

        [CLSCompliant(false)]
        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            if (_derivedTypes.Value.Contains(type))
            {
                var clonedSchema = new Schema
                {
                    properties = schema.properties,
                    type = schema.type,
                    required = schema.required
                };

                var baseType = type.BaseType;
                var baseTypeName = _useFullTypeNames ? baseType.FullName : baseType.FriendlyId();

                var parentSchema = new Schema { @ref = "#/definitions/" + baseTypeName };

                schema.allOf = new List<Schema> { parentSchema, clonedSchema };

                //reset properties for they are included in allOf, should be null but code does not handle it
                schema.properties = new Dictionary<string, Schema>();
            }
        }
    }
}
