using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Reviews.Model;
using System.ServiceModel;

namespace VirtoCommerce.Foundation.Reviews.Services
{
	[ServiceContract]
	public interface IReviewService
	{
		[OperationContract]
		void ReportAbuse(string reviewId, string authorId, string authorName, string authorLocation);
		[OperationContract]
		void ReportHelpful(string reviewId, bool isHelpful, string authorId, string authorName, string authorLocation);
		[OperationContract]
		Review[] GetTopReviews();
		[OperationContract]
		double GetItemOverallRating(string itemId);
	}
}
