using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.PostgreSql.Extensions;

namespace VirtoCommerce.Platform.Data.PostgreSql.EntityConfigurations.Data;

public class OperationLogEntityConfiguration : IEntityTypeConfiguration<OperationLogEntity>
{
    public void Configure(EntityTypeBuilder<OperationLogEntity> builder)
    {
        builder.Property(x => x.ModifiedBy).UseCaseInsensitiveCollation();
        builder.Property(x => x.Detail).UseCaseInsensitiveCollation();
    }
}
