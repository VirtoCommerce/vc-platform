using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtoCommerce.Platform.Core.Common;
using static VirtoCommerce.Platform.Data.Infrastructure.DbContextBase;

namespace VirtoCommerce.Platform.Data.Extensions;

public static class ModelBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> ToAuditableEntityTable<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, string name)
        where TEntity : AuditableEntity
    {
        entityTypeBuilder.ToEntityTable(name);
        entityTypeBuilder.Property(x => x.CreatedBy).HasMaxLength(UserNameLength);
        entityTypeBuilder.Property(x => x.ModifiedBy).HasMaxLength(UserNameLength);

        return entityTypeBuilder;
    }

    public static EntityTypeBuilder<TEntity> ToEntityTable<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, string name)
        where TEntity : Entity
    {
        entityTypeBuilder.ToTable(name).HasKey(x => x.Id);
        entityTypeBuilder.Property(x => x.Id).HasMaxLength(IdLength).ValueGeneratedOnAdd();

        return entityTypeBuilder;
    }
}
