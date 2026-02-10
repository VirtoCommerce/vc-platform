using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EntityFrameworkCore.Triggers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Extensions;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Security.Model;
using static VirtoCommerce.Platform.Data.Infrastructure.DbContextBase;

namespace VirtoCommerce.Platform.Security.Repositories
{
    public class SecurityDbContext : IdentityDbContext<ApplicationUser, Role, string>
    {
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options)
            : base(options)
        {
        }

        protected SecurityDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<DateTime>().HaveConversion<UtcDateTimeConverter>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserApiKeyEntity>().ToAuditableEntityTable("UserApiKey");
            builder.Entity<UserApiKeyEntity>().Property(x => x.ApiKey).HasMaxLength(Length128);
            builder.Entity<UserApiKeyEntity>().HasIndex(x => new { x.ApiKey }).IsUnique();

            builder.Entity<UserPasswordHistoryEntity>().ToAuditableEntityTable("AspNetUserPasswordsHistory");
            builder.Entity<UserPasswordHistoryEntity>().HasIndex(x => new { x.UserId });
            builder.Entity<UserPasswordHistoryEntity>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(uh => uh.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().Ignore(x => x.Password);
            builder.Entity<ApplicationUser>().Ignore(x => x.Roles);
            builder.Entity<ApplicationUser>().Ignore(x => x.Logins);
            builder.Entity<ApplicationUser>().Property(x => x.Id).HasMaxLength(IdLength).ValueGeneratedOnAdd();
            builder.Entity<ApplicationUser>().Property(x => x.UserType).HasMaxLength(Length64);
            builder.Entity<ApplicationUser>().Property(x => x.Status).HasMaxLength(Length64);
            builder.Entity<ApplicationUser>().Property(x => x.PhotoUrl).HasMaxLength(UrlLength);
            builder.Entity<ApplicationUser>().Property(x => x.StoreId).HasMaxLength(IdLength);
            builder.Entity<ApplicationUser>().Property(x => x.MemberId).HasMaxLength(IdLength);

            builder.Entity<Role>().Ignore(x => x.Permissions);
            builder.Entity<Role>().Property(x => x.Id).HasMaxLength(IdLength).ValueGeneratedOnAdd();

            builder.Entity<IdentityUserClaim<string>>().Property(x => x.UserId).HasMaxLength(IdLength);

            builder.Entity<IdentityRoleClaim<string>>().Property(x => x.RoleId).HasMaxLength(IdLength);

            builder.Entity<IdentityUserLogin<string>>().Property(x => x.UserId).HasMaxLength(IdLength);
            builder.Entity<IdentityUserLogin<string>>().Property(x => x.LoginProvider).HasMaxLength(Length128);
            builder.Entity<IdentityUserLogin<string>>().Property(x => x.ProviderKey).HasMaxLength(Length128);

            builder.Entity<IdentityUserToken<string>>().Property(x => x.UserId).HasMaxLength(IdLength);
            builder.Entity<IdentityUserToken<string>>().Property(x => x.LoginProvider).HasMaxLength(Length128);
            builder.Entity<IdentityUserToken<string>>().Property(x => x.Name).HasMaxLength(Length128);

            builder.Entity<IdentityUserRole<string>>().Property(x => x.UserId).HasMaxLength(IdLength);
            builder.Entity<IdentityUserRole<string>>().Property(x => x.RoleId).HasMaxLength(IdLength);
            builder.Entity<IdentityUserRole<string>>(userRole =>
            {
                userRole.HasOne<Role>()
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);

                userRole.HasOne<ApplicationUser>()
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId);
            });

            builder.Entity<ServerCertificateEntity>().ToEntityTable(nameof(ServerCertificate));
            builder.Entity<ServerCertificateEntity>().Property(x => x.PrivateKeyCertPassword).HasMaxLength(Length128);

            // Allows configuration for an entity type for different database types.
            // Applies configuration from all <see cref="IEntityTypeConfiguration{TEntity}" in VirtoCommerce.Platform.Data.XXX project. /> 
            switch (Database.ProviderName)
            {
                case "Pomelo.EntityFrameworkCore.MySql":
                    builder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.Platform.Data.MySql"),
                        IsSecurityDBContextEntity);
                    break;
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    builder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.Platform.Data.PostgreSql"),
                        IsSecurityDBContextEntity);
                    break;
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    builder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.Platform.Data.SqlServer"),
                        IsSecurityDBContextEntity);
                    break;
            }
        }

        #region override Save*** methods to catch save events in Triggers, otherwise ApplicationUser not be catched because SecurityDbContext can't inherit DbContextWithTriggers
        public override int SaveChanges()
        {
            return this.SaveChangesWithTriggers(base.SaveChanges, acceptAllChangesOnSuccess: true);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return this.SaveChangesWithTriggers(base.SaveChanges, acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, true, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, acceptAllChangesOnSuccess, cancellationToken);
        }
        #endregion

        private static bool IsSecurityDBContextEntity(Type type)
        {
            return type.Namespace.EndsWith(".EntityConfigurations.Security");
        }
    }
}
