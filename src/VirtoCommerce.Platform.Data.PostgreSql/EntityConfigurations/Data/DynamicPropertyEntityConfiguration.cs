using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.PostgreSql.Extensions;

namespace VirtoCommerce.Platform.Data.PostgreSql.EntityConfigurations.Data;

public class DynamicPropertyEntityConfiguration : IEntityTypeConfiguration<DynamicPropertyEntity>
{
    public void Configure(EntityTypeBuilder<DynamicPropertyEntity> builder)
    {
        builder.Property(x => x.Name).UseCaseInsensitiveCollation();
    }
}
