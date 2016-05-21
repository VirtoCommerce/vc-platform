using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.Platform.Data.Security.Identity
{
    public class IdentityDatabaseInitializer : SetupDatabaseInitializer<SecurityDbContext, Migrations.Configuration>
    {
        protected override void Seed(SecurityDbContext context)
        {
            base.Seed(context);

            context.Users.Add(new ApplicationUser
            {
                Id = "1eb2fa8ac6574541afdb525833dadb46",
                UserName = "admin",
                PasswordHash = "AHQSmKnSLYrzj9vtdDWWnUXojjpmuDW2cHvWloGL9UL3TC9UCfBmbIuR2YCyg4BpNg==",
                SecurityStamp = string.Empty,
                EmailConfirmed = true,
                LockoutEnabled = true,
            });

            context.Users.Add(new ApplicationUser
            {
                Id = "9b605a3096ba4cc8bc0b8d80c397c59f",
                UserName = "frontend",
                PasswordHash = "AHQSmKnSLYrzj9vtdDWWnUXojjpmuDW2cHvWloGL9UL3TC9UCfBmbIuR2YCyg4BpNg==",
                SecurityStamp = string.Empty,
                EmailConfirmed = true,
                LockoutEnabled = true,
            });

            context.SaveChanges();
        }
    }
}
