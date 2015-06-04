using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VirtoCommerce.Foundation.Data.Security.Identity
{
	public class SecurityDbContext : IdentityDbContext<ApplicationUser>
	{
		public SecurityDbContext()
			: this("VirtoCommerce")
		{
		}

		public SecurityDbContext(string nameOrConnectionString)
			: base(nameOrConnectionString, false)
		{
		}

		static SecurityDbContext()
		{
			// Set the database intializer which is run once during application start
			Database.SetInitializer(new SecurityDatabaseInitializer());
		}

		public static SecurityDbContext Create()
		{
			return new SecurityDbContext();
		}
	}

	public class SecurityDatabaseInitializer : DropCreateDatabaseIfModelChanges<SecurityDbContext>
	{
		protected override void Seed(SecurityDbContext context)
		{
			context.Users.Add(new ApplicationUser
			{
				Id = "1",
				Email = "john.doe@gmail.com",
				EmailConfirmed = true,
				UserName = "admin",
				PasswordHash = "AHQSmKnSLYrzj9vtdDWWnUXojjpmuDW2cHvWloGL9UL3TC9UCfBmbIuR2YCyg4BpNg==",
				SecurityStamp = "",
				LockoutEnabled = true
			});
			base.Seed(context);
		}
	}
}
