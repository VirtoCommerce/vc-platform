using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
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

        protected override ValueGenerator FindForType(IProperty property, ITypeBase typeBase, Type clrType)
        {
            if (property.ValueGenerated != ValueGenerated.Never)
            {
                if (clrType == typeof(string))
                {
                    return new StringCompactGuidValueGenerator(property.GetDefaultValueSql() != null);
                }
            }

            return base.FindForType(property, typeBase, clrType);
        }
    }
}
