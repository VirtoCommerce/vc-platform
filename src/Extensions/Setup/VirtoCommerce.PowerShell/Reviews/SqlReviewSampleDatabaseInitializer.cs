using System;
using System.Collections.Generic;
using VirtoCommerce.PowerShell.DatabaseSetup;
using VirtoCommerce.Foundation.Reviews.Model;
using VirtoCommerce.Foundation.Reviews.Model.Management;
using VirtoCommerce.Foundation.Data.Reviews.Migrations;
using VirtoCommerce.Foundation.Data.Reviews;
using System.Reflection;
using System.IO;

namespace VirtoCommerce.PowerShell.Reviews
{
    public class SqlReviewSampleDatabaseInitializer : SetupDatabaseInitializer<EFReviewRepository, Configuration>
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
