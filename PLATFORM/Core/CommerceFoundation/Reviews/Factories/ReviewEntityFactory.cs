using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Reviews.Model;
using VirtoCommerce.Foundation.Reviews.Model.Management;

namespace VirtoCommerce.Foundation.Reviews.Factories
{
	public class ReviewEntityFactory : FactoryBase, IReviewEntityFactory
	{
		public ReviewEntityFactory()
		{
			RegisterStorageType(typeof(Review), "Review");
			RegisterStorageType(typeof(ReviewComment), "ReviewComment");
			RegisterStorageType(typeof(ReviewFieldValue), "ReviewFieldValue");
			RegisterStorageType(typeof(MediaContent), "MediaContent");
			RegisterStorageType(typeof(ReportAbuseElement), "ReportAbuseElement");
			RegisterStorageType(typeof(ReportHelpfulElement), "ReportHelpfulElement");
			RegisterStorageType(typeof(ReviewFieldSchema), "ReviewFieldSchema");
			RegisterStorageType(typeof(ReviewSchema), "ReviewSchema");
			RegisterStorageType(typeof(Subscription), "Subscription");
			RegisterStorageType(typeof(UserBlacklist), "UserBlacklist");
		}
	}
}
