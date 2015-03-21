namespace VirtoCommerce.Foundation.Data.Reviews
{
	public class SqlReviewSampleDatabaseInitializer : SqlReviewDatabaseInitializer
	{
		protected override void Seed(EFReviewRepository context)
		{
			CreateReviews(context);
			base.Seed(context);
		}

		private void CreateReviews(EFReviewRepository context)
		{
			ExecuteSqlScriptFile(context, "Review.sql", "Reviews");
			ExecuteSqlScriptFile(context, "ReviewComment.sql", "Reviews");
			ExecuteSqlScriptFile(context, "ReviewFieldValue.sql", "Reviews");
		}
	}
}
