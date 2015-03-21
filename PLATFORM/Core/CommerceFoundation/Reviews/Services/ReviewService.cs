using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Reviews.Repositories;
using VirtoCommerce.Foundation.Reviews.Model;

namespace VirtoCommerce.Foundation.Reviews.Services
{
	public class ReviewService : IReviewService
	{
		private IReviewRepository _repository;
		public ReviewService(IReviewRepository repository)
		{
			_repository = repository;
		}

		#region IReviewService Members

		public void ReportAbuse(string reviewId, string authorId, string authorName, string authorLocation)
		{
			// Validate Incoming parameters
			if (string.IsNullOrEmpty(reviewId))
				throw new ArgumentNullException("reviewId");
			if (string.IsNullOrEmpty(authorId))
				throw new ArgumentNullException("authorId");
			if (string.IsNullOrEmpty(authorName))
				throw new ArgumentNullException("authorName");
			if (string.IsNullOrEmpty(authorLocation))
				throw new ArgumentNullException("authorLocation");

			var review = FindReviewById(reviewId);
			if (review != null)
			{
				// Skip duplicated Abuse
				var abuseElement = FindReportAbuseByAuthor(reviewId, authorId);
				if (abuseElement != null)
				{
					return;
				}

				// Post Abuse
				var newAbuseElement = new ReportAbuseElement();
				newAbuseElement.ReviewId = reviewId;
				//newAbuseElement.ReportAbuseElementId = Guid.NewGuid().ToString();
				newAbuseElement.AuthorId = authorId;
                //newAbuseElement.AuthorFN = authorName;
                //newAbuseElement.AuthorLocation = authorLocation;
                //newAbuseElement.Status = "Pending";

				_repository.Add(newAbuseElement);
				_repository.UnitOfWork.Commit();
			}
		}

		public void ReportHelpful(string reviewId, bool isHelpful, string authorId, string authorName, string authorLocation)
		{
			// Validate Incoming parameters
			if (string.IsNullOrEmpty(reviewId))
				throw new ArgumentNullException("reviewId");
			if (string.IsNullOrEmpty(authorId))
				throw new ArgumentNullException("authorId");
			if (string.IsNullOrEmpty(authorName))
				throw new ArgumentNullException("authorName");
			if (string.IsNullOrEmpty(authorLocation))
				throw new ArgumentNullException("authorLocation");

			var review = FindReviewById(reviewId);
			if (review != null)
			{
				// Skip duplicated Helpful
				var helpfulElement = FindReportHelpfulByAuthor(reviewId, authorId);
				if (helpfulElement != null)
				{
					return;
				}

				// Post Helpfull
				var newHelpfulElement = new ReportHelpfulElement();
				newHelpfulElement.ReviewId = reviewId;
				//newHelpfulElement.ReportHelpfulElementId = Guid.NewGuid().ToString();
				newHelpfulElement.AuthorId = authorId;
                //newHelpfulElement.AuthorFN = authorName;
                //newHelpfulElement.AuthorLocation = authorLocation;
                //newHelpfulElement.Status = "Pending";
				newHelpfulElement.IsHelpful = isHelpful;

				_repository.Add(newHelpfulElement);
				_repository.UnitOfWork.Commit();
			}
		}

		public Model.Review[] GetTopReviews()
		{
			return _repository.Reviews.Take(20).ToArray();
		}

		public double GetItemOverallRating(string itemId)
		{
			var result = _repository.Reviews.Where(x => x.ItemId == itemId && x.Status == (int)ReviewStatus.Approved).First();

			return result.OverallRating;
		}

		#endregion

		private ReportAbuseElement FindReportAbuseByAuthor(string reviewId, string authorId)
		{
			if (reviewId == null)
				throw new ArgumentNullException("reviewId");
			if (authorId == null)
				throw new ArgumentNullException("authorId");

			var results = (from r in _repository.ReportAbuseElements where r.ReviewId == reviewId && r.AuthorId == authorId select r).FirstOrDefault();
			return results;
		}

		private ReportHelpfulElement FindReportHelpfulByAuthor(string reviewId, string authorId)
		{
			if (reviewId == null)
				throw new ArgumentNullException("reviewId");
			if (authorId == null)
				throw new ArgumentNullException("authorId");

			var results = (from r in _repository.ReportHelpfulElements where r.ReviewId == reviewId && r.AuthorId == authorId select r).FirstOrDefault();
			return results;
		}


		private Review FindReviewById(string reviewId)
		{
			if (reviewId == null)
				throw new ArgumentNullException("reviewId");

			var results = (from r in _repository.Reviews where r.ReviewId == reviewId select r).FirstOrDefault();
			return results;
		}

		
		private ReportAbuseElement FindReportAbuseElementByAuthor(string reviewId, string authorId)
		{
			if (reviewId == null)
				throw new ArgumentNullException("reviewId");
			if (authorId == null)
				throw new ArgumentNullException("authorId");

			var results = (from r in _repository.ReportAbuseElements where r.ReviewId == reviewId && r.AuthorId == authorId select r).FirstOrDefault();
			return results;
		}
	}
}
