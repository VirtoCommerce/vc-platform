namespace VirtoCommerce.Foundation.Data.Security.Identity
{
	public class IdentitySampleDatabaseInitializer : IdentityDatabaseInitializer
	{
		protected override void Seed(SecurityDbContext context)
		{
			base.Seed(context);

			context.Users.Add(new ApplicationUser
			{
				Id = "2",
				UserName = "test",
				PasswordHash = "AHQSmKnSLYrzj9vtdDWWnUXojjpmuDW2cHvWloGL9UL3TC9UCfBmbIuR2YCyg4BpNg==",
				SecurityStamp = string.Empty,
				EmailConfirmed = true,
				LockoutEnabled = true,
			});

			context.SaveChanges();
		}
	}
}
