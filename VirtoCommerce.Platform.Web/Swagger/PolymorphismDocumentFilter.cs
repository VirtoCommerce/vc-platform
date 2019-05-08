using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Description;
using Swashbuckle.Swagger;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public class PolymorphismDocumentFilter : IDocumentFilter
    {
        private readonly bool _useFullTypeNames;

        public PolymorphismDocumentFilter(bool useFullTypeNames)
        {
            _useFullTypeNames = useFullTypeNames;
        }

        [CLSCompliant(false)]
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            RegisterSubClasses(schemaRegistry, apiExplorer);
        }

        private void RegisterSubClasses(SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            foreach (var type in apiExplorer.ApiDescriptions.Select(x => x.ResponseType()).Where(x => x != null).Distinct())
            {
                var schema = GetTypeSchema(schemaRegistry, type, false);

                // IApiExplorer contains types from all api controllers, so some of them could be not presented in specific module schemaRegistry.
                if (schema != null)
                {
                    // Find if type is registered in AbstractTypeFactory with descendants and TypeInfo have Discriminator filled
                    var polymorphicBaseTypeInfoType = typeof(PolymorphicBaseTypeInfo<>).MakeGenericType(type);
                    var polymorphicBaseTypeInfoInstance = Activator.CreateInstance(polymorphicBaseTypeInfoType);
                    var derivedTypesPropertyGetter = polymorphicBaseTypeInfoType.GetProperty("DerivedTypes", BindingFlags.Instance | BindingFlags.Public).GetGetMethod();
                    var derivedTypes = (derivedTypesPropertyGetter.Invoke(polymorphicBaseTypeInfoInstance, null) as IEnumerable<Type>).ToArray();
                    var discriminatorPropertyGetter = polymorphicBaseTypeInfoType.GetProperty("Discriminator", BindingFlags.Instance | BindingFlags.Public).GetGetMethod();
                    var discriminator = discriminatorPropertyGetter.Invoke(polymorphicBaseTypeInfoInstance, null) as string;

                    // Polymorphism registration required if we have at least one TypeInfo in AbstractTypeFactory and Discriminator is set
                    if (derivedTypes.Length > 0 && !string.IsNullOrEmpty(discriminator))
                    {
                        foreach (var derivedType in derivedTypes)
                        {
                            var derivedTypeSchema = GetTypeSchema(schemaRegistry, derivedType, false);

                            // Make sure all derivedTypes are in schemaRegistry
                            if (derivedTypeSchema == null)
                            {
                                derivedTypeSchema = schemaRegistry.GetOrRegister(derivedType);
                            }

                            AddInheritanceToDerivedTypeSchema(derivedTypeSchema, type);
                        }

                        AddDiscriminatorToBaseType(schemaRegistry, type, discriminator);
                    }
                }
            }
        }

        private Schema GetTypeSchema(SchemaRegistry schemaRegistry, Type type, bool throwOnEmpty)
        {
            Schema result = null;
            var typeName = _useFullTypeNames ? type.FullName : type.Name;

            // IApiExplorer contains types from all api controllers, so some of them could be not presented in specific module schemaRegistry.
            if (schemaRegistry.Definitions.ContainsKey(typeName))
            {
                result = schemaRegistry.Definitions[typeName];
            }

            if (throwOnEmpty && result == null)
            {
                throw new KeyNotFoundException($"Derived type \"{type.FullName}\" does not exist in SchemaRegistry.");
            }

            return result;
        }

        private void AddDiscriminatorToBaseType(SchemaRegistry schemaRegistry, Type baseType, string discriminator)
        {
            // Need to make first discriminator character lower to avoid properties duplication because of case, as all properties in OpenApi spec are in camelCase
            discriminator = char.ToLowerInvariant(discriminator[0]) + discriminator.Substring(1);

            var baseTypeSchema = GetTypeSchema(schemaRegistry, baseType, true);

            // Set up a discriminator property (it must be required)
            baseTypeSchema.discriminator = discriminator;
            baseTypeSchema.required = new List<string> { discriminator };

            if (!baseTypeSchema.properties.ContainsKey(discriminator))
            {
                baseTypeSchema.properties.Add(discriminator, new Schema { type = "string" });
            }
        }

        private void AddInheritanceToDerivedTypeSchema(Schema derivedTypeSchema, Type baseType)
        {
            var clonedSchema = new Schema
            {
                properties = derivedTypeSchema.properties,
                type = derivedTypeSchema.type,
                required = derivedTypeSchema.required
            };

            var baseTypeName = _useFullTypeNames ? baseType.FullName : baseType.FriendlyId();

            var parentSchema = new Schema { @ref = "#/definitions/" + baseTypeName };

            derivedTypeSchema.allOf = new List<Schema> { parentSchema, clonedSchema };

            //reset properties for they are included in allOf, should be null but code does not handle it
            derivedTypeSchema.properties = new Dictionary<string, Schema>();
        }

        // This private class is used to simplify querying from AbstractTypeFactory<T>.AllTypeInfos properties (no need to implement Linq queries using reflection)
        private class PolymorphicBaseTypeInfo<T>
        {
            public string Discriminator { get => AbstractTypeFactory<T>.AllTypeInfos.FirstOrDefault()?.Discriminator; }
            public IEnumerable<Type> DerivedTypes { get => AbstractTypeFactory<T>.AllTypeInfos.Select(x => x.Type) ?? Enumerable.Empty<Type>(); }
        }
    }
}
