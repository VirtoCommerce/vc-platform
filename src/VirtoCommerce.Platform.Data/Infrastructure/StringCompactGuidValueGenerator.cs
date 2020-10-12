using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
    public class StringCompactGuidValueGenerator : ValueGenerator<string>
    {
        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public StringCompactGuidValueGenerator(bool generateTemporaryValues)
        {
            GeneratesTemporaryValues = generateTemporaryValues;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override bool GeneratesTemporaryValues { get; }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override string Next(EntityEntry entry)
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
