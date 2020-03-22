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
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserApiKeyEntity>().ToTable("UserApiKey").HasKey(x => x.Id);
            builder.Entity<UserApiKeyEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            builder.Entity<UserApiKeyEntity>().Property(x => x.ApiKey).HasMaxLength(128);
            builder.Entity<UserApiKeyEntity>().HasIndex(x => new { x.ApiKey }).IsUnique();
            builder.Entity<UserApiKeyEntity>().Property(x => x.CreatedBy).HasMaxLength(64);
            builder.Entity<UserApiKeyEntity>().Property(x => x.ModifiedBy).HasMaxLength(64);


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
            builder.Entity<ApplicationUser>().Property(x => x.UserType).HasMaxLength(64);
            builder.Entity<ApplicationUser>().Property(x => x.PhotoUrl).HasMaxLength(2048);
            builder.Entity<ApplicationUser>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            builder.Entity<ApplicationUser>().Property(x => x.StoreId).HasMaxLength(128);
            builder.Entity<ApplicationUser>().Property(x => x.MemberId).HasMaxLength(128);
            builder.Entity<Role>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            builder.Entity<IdentityUserClaim<string>>().Property(x => x.UserId).HasMaxLength(128);
            builder.Entity<IdentityUserLogin<string>>().Property(x => x.UserId).HasMaxLength(128);
            builder.Entity<IdentityUserLogin<string>>().Property(x => x.LoginProvider).HasMaxLength(128);
            builder.Entity<IdentityUserLogin<string>>().Property(x => x.ProviderKey).HasMaxLength(128);
            builder.Entity<IdentityUserRole<string>>().Property(x => x.UserId).HasMaxLength(128);
            builder.Entity<IdentityUserRole<string>>().Property(x => x.RoleId).HasMaxLength(128);
            builder.Entity<IdentityRoleClaim<string>>().Property(x => x.RoleId).HasMaxLength(128);
            builder.Entity<IdentityUserToken<string>>().Property(x => x.UserId).HasMaxLength(128);
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
