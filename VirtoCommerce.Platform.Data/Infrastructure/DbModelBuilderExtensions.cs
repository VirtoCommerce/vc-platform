using System.Data.Entity;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
    public static class DbModelBuilderExtensions
    {
        public static void Entity<T>(this DbModelBuilder modelBuilder, string tableName, string idColumnName)
            where T : Entity
        {
            modelBuilder.Entity<T>()
                .ToTable(tableName)
                .HasKey(x => x.Id)
                .Property(x => x.Id).HasColumnName(idColumnName).HasMaxLength(64);
        }
    }
}
