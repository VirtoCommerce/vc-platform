using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenIddict.EntityFrameworkCore.Models;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Security.Model;

namespace VirtoCommerce.Platform.Data.SqlServer;
public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable(tb => tb.UseSqlOutputClause(false));
    }
}

public class UserApiKeyEntityConfiguration : IEntityTypeConfiguration<UserApiKeyEntity>
{
    public void Configure(EntityTypeBuilder<UserApiKeyEntity> builder)
    {
        builder.ToTable(tb => tb.UseSqlOutputClause(false));
    }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(tb => tb.UseSqlOutputClause(false));
    }
}

public class UserPasswordHistoryEntityConfiguration : IEntityTypeConfiguration<UserPasswordHistoryEntity>
{
    public void Configure(EntityTypeBuilder<UserPasswordHistoryEntity> builder)
    {
        builder.ToTable(tb => tb.UseSqlOutputClause(false));
    }
}

public class ServerCertificateEntityConfiguration : IEntityTypeConfiguration<ServerCertificateEntity>
{
    public void Configure(EntityTypeBuilder<ServerCertificateEntity> builder)
    {
        builder.ToTable(tb => tb.UseSqlOutputClause(false));
    }
}

public class IdentityRoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        builder.ToTable(tb => tb.UseSqlOutputClause(false));
    }
}
public class IdentityUserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
    {
        builder.ToTable(tb => tb.UseSqlOutputClause(false));
    }
}

public class IdentityUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
    {
        builder.ToTable(tb => tb.UseSqlOutputClause(false));
    }
}

public class IdentityUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
    {
        builder.ToTable(tb => tb.UseSqlOutputClause(false));
    }
}
public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.ToTable(tb => tb.UseSqlOutputClause(false));
    }
}

public class OpenIddictTokenConfiguration : IEntityTypeConfiguration<OpenIddictEntityFrameworkCoreToken>
{
    public void Configure(EntityTypeBuilder<OpenIddictEntityFrameworkCoreToken> builder)
    {
        builder.ToTable(tb => tb.UseSqlOutputClause(false));
    }
}

public class OpenIddictApplicationConfiguration : IEntityTypeConfiguration<OpenIddictEntityFrameworkCoreApplication>
{
    public void Configure(EntityTypeBuilder<OpenIddictEntityFrameworkCoreApplication> builder)
    {
        builder.ToTable(tb => tb.UseSqlOutputClause(false));
    }
}

public class OpenIddictScopeConfiguration : IEntityTypeConfiguration<OpenIddictEntityFrameworkCoreScope>
{
    public void Configure(EntityTypeBuilder<OpenIddictEntityFrameworkCoreScope> builder)
    {
        builder.ToTable(tb => tb.UseSqlOutputClause(false));
    }
}

public class OpenIddictAuthorizationConfiguration : IEntityTypeConfiguration<OpenIddictEntityFrameworkCoreAuthorization>
{
    public void Configure(EntityTypeBuilder<OpenIddictEntityFrameworkCoreAuthorization> builder)
    {
        builder.ToTable(tb => tb.UseSqlOutputClause(false));
    }
}
