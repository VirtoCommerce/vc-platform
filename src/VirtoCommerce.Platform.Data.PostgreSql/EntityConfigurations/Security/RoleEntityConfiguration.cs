using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.PostgreSql.Extensions;

namespace VirtoCommerce.Platform.Data.PostgreSql.EntityConfigurations.Security;

public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(x => x.Name).UseCaseInsensitiveCollation();
    }
}
