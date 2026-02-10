using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.PostgreSql.Extensions;

namespace VirtoCommerce.Platform.Data.PostgreSql.EntityConfigurations.Security;

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.UserName).UseCollation(CaseInsensitive.CollationName);
        builder.Property(x => x.Email).UseCollation(CaseInsensitive.CollationName);
    }
}
