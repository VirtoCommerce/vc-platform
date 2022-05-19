using System.Threading;
using System.Threading.Tasks;
using EntityFrameworkCore.Triggers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model;

namespace VirtoCommerce.Platform.Security.Repositories
{
    public class SecurityDbContext : IdentityDbContext<ApplicationUser, Role, string>
    {
        private const int stringShort = 64;
        private const int stringMedium = 128;
        private const int stringLong = 2048;

        public SecurityDbContext(DbContextOptions<SecurityDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserApiKeyEntity>().ToTable("UserApiKey").HasKey(x => x.Id);
            builder.Entity<UserApiKeyEntity>().Property(x => x.Id).HasMaxLength(stringMedium).ValueGeneratedOnAdd();
            builder.Entity<UserApiKeyEntity>().Property(x => x.ApiKey).HasMaxLength(stringMedium);
            builder.Entity<UserApiKeyEntity>().HasIndex(x => new { x.ApiKey }).IsUnique();
            builder.Entity<UserApiKeyEntity>().Property(x => x.CreatedBy).HasMaxLength(stringShort);
            builder.Entity<UserApiKeyEntity>().Property(x => x.ModifiedBy).HasMaxLength(stringShort);

            builder.Entity<UserPasswordHistoryEntity>().ToTable("AspNetUserPasswordsHistory").HasKey(x => x.Id);
            builder.Entity<UserPasswordHistoryEntity>().Property(x => x.Id).HasMaxLength(stringMedium).ValueGeneratedOnAdd();
            builder.Entity<UserPasswordHistoryEntity>().HasIndex(x => new { x.UserId });
            builder.Entity<UserPasswordHistoryEntity>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(uh => uh.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<IdentityUserRole<string>>(userRole =>
            {
                userRole.HasOne<Role>()
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);

                userRole.HasOne<ApplicationUser>()
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId);
            });

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<Role>().Ignore(x => x.Permissions);
            builder.Entity<ApplicationUser>().Ignore(x => x.Password);
            builder.Entity<ApplicationUser>().Ignore(x => x.Roles);
            builder.Entity<ApplicationUser>().Ignore(x => x.LockoutEndDateUtc);
            builder.Entity<ApplicationUser>().Ignore(x => x.Permissions);
            builder.Entity<ApplicationUser>().Ignore(x => x.Logins);
            builder.Entity<ApplicationUser>().Ignore(x => x.UserState);
            builder.Entity<ApplicationUser>().Property(x => x.UserType).HasMaxLength(stringShort);
            builder.Entity<ApplicationUser>().Property(x => x.Status).HasMaxLength(stringShort);
            builder.Entity<ApplicationUser>().Property(x => x.PhotoUrl).HasMaxLength(stringLong);
            builder.Entity<ApplicationUser>().Property(x => x.Id).HasMaxLength(stringMedium).ValueGeneratedOnAdd();
            builder.Entity<ApplicationUser>().Property(x => x.StoreId).HasMaxLength(stringMedium);
            builder.Entity<ApplicationUser>().Property(x => x.MemberId).HasMaxLength(stringMedium);

            builder.Entity<Role>().Property(x => x.Id).HasMaxLength(stringMedium).ValueGeneratedOnAdd();
            builder.Entity<IdentityUserClaim<string>>().Property(x => x.UserId).HasMaxLength(stringMedium);
            builder.Entity<IdentityUserLogin<string>>().Property(x => x.UserId).HasMaxLength(stringMedium);
            builder.Entity<IdentityUserLogin<string>>().Property(x => x.LoginProvider).HasMaxLength(stringMedium);
            builder.Entity<IdentityUserLogin<string>>().Property(x => x.ProviderKey).HasMaxLength(stringMedium);
            builder.Entity<IdentityUserRole<string>>().Property(x => x.UserId).HasMaxLength(stringMedium);
            builder.Entity<IdentityUserRole<string>>().Property(x => x.RoleId).HasMaxLength(stringMedium);
            builder.Entity<IdentityRoleClaim<string>>().Property(x => x.RoleId).HasMaxLength(stringMedium);
            builder.Entity<IdentityUserToken<string>>().Property(x => x.UserId).HasMaxLength(stringMedium);

            builder.Entity<ServerCertificateEntity>().ToTable(nameof(ServerCertificate)).HasKey(x => x.Id);
            builder.Entity<ServerCertificateEntity>().Property(x => x.Id).HasMaxLength(stringMedium).ValueGeneratedOnAdd();
            builder.Entity<ServerCertificateEntity>().Property(x => x.PublicCertBase64);
            builder.Entity<ServerCertificateEntity>().Property(x => x.PrivateKeyCertBase64);
            builder.Entity<ServerCertificateEntity>().Property(x => x.PrivateKeyCertPassword).HasMaxLength(stringMedium);

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
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, true, cancellationToken);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, acceptAllChangesOnSuccess, cancellationToken);
        }
        #endregion
    }
}
