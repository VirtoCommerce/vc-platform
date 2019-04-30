using System;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.Swagger;

namespace VirtoCommerce.Platform.Web.Swagger
{

    public class PolymorphismSchemaFilter : ISchemaFilter
    {
        private readonly Type[] _types;
        private readonly Lazy<HashSet<Type>> _derivedTypes;

        public PolymorphismSchemaFilter(Type[] types)
        {
            _types = types;
            _derivedTypes = new Lazy<HashSet<Type>>(Init);
        }

        private HashSet<Type> Init()
        {
            var result = new HashSet<Type>();

            var derivedTypes = _types.SelectMany(baseType =>
                baseType.Assembly
                    .GetTypes()
                    .Where(derivedType => baseType != derivedType && baseType.IsAssignableFrom(derivedType)));

            foreach (var item in derivedTypes)
            {
                result.Add(item);
            }

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
                var baseTypeName = schemaRegistry.Definitions.ContainsKey(baseType.FullName) ? baseType.FullName : baseType.FriendlyId();

                var parentSchema = new Schema { @ref = "#/definitions/" + baseTypeName };

                schema.allOf = new List<Schema> { parentSchema, clonedSchema };

                //reset properties for they are included in allOf, should be null but code does not handle it
                schema.properties = new Dictionary<string, Schema>();
            }
        }
    }
}
