using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Replace the default EF value generation  for string primary keys from Guid.ToString() with hyphens to Guid.ToString("N")
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder GenerateCompactGuidForKeys(this DbContextOptionsBuilder builder)
        {
            builder.ReplaceService<IValueGeneratorSelector, CustomRelationalValueGeneratorSelector>();
            return builder;
        }
    }
}
