using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using VirtoCommerce.Platform.Core.Extensions;
namespace VirtoCommerce.Platform.Data.Infrastructure
{
    public class CustomRelationalValueGeneratorSelector : RelationalValueGeneratorSelector
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RelationalValueGeneratorSelector" /> class.
        /// </summary>
        /// <param name="dependencies"> Parameter object containing dependencies for this service. </param>
        public CustomRelationalValueGeneratorSelector(ValueGeneratorSelectorDependencies dependencies)
            : base(dependencies)
        {
        }

        /// <summary>
        ///     Creates a new value generator for the given property.
        /// </summary>
        /// <param name="property"> The property to get the value generator for. </param>
        /// <param name="entityType">
        ///     The entity type that the value generator will be used for. When called on inherited properties on derived entity types,
        ///     this entity type may be different from the declared entity type on <paramref name="property" />
        /// </param>
        /// <returns> The newly created value generator. </returns>
        public override ValueGenerator Create(IProperty property, IEntityType entityType)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }

            if (property.ValueGenerated != ValueGenerated.Never)
            {
                var propertyType = property.ClrType.UnwrapNullableType().UnwrapEnumType();
                if ( propertyType == typeof(string))
                {
                    //Generate temporary value if GetDefaultValueSql is set
                    return new StringCompactGuidValueGenerator(property.GetDefaultValueSql() != null);
                }

            }
            return base.Create(property, entityType);
        }
    }
}
