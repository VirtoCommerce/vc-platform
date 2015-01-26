using VirtoCommerce.Foundation.Data.Infrastructure;

namespace VirtoCommerce.Foundation.Data.Reviews
{
	public class SqlReviewSampleDatabaseInitializer : SetupDatabaseInitializer<EFReviewRepository, Migrations.Configuration>
	{
		protected override void Seed(EFReviewRepository context)
		{
			CreateReviews(context);
			base.Seed(context);
		}

		private void CreateReviews(EFReviewRepository context)
		{
			RunCommand(context, "Review.sql", "Reviews");
			RunCommand(context, "ReviewComment.sql", "Reviews");
			RunCommand(context, "ReviewFieldValue.sql", "Reviews");
		}
	}
}
