using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Reviews.Model;

namespace VirtoCommerce.Foundation.Reviews.Repositories
{
	public interface IReviewRepository : IRepository
	{
		IQueryable<Review> Reviews { get; }
		IQueryable<ReviewComment> ReviewComments { get; }
		IQueryable<ReviewFieldValue> ReviewFieldValues { get; }
		IQueryable<MediaContent> MediaContents { get; }

		IQueryable<ReportAbuseElement> ReportAbuseElements{ get; }
		IQueryable<ReportHelpfulElement> ReportHelpfulElements{ get; }
	
	}
}
