using System.Linq;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Reviews;
using VirtoCommerce.Foundation.Reviews.Model;
using VirtoCommerce.Foundation.Reviews.Repositories;

namespace VirtoCommerce.Client
{
    public class ReviewClient
    {
        #region Cache Constants
        public const string ReviewsCacheKey = "R:{0}";
        public const string ReviewCommentsCacheKey = "RC:{0}";
        #endregion

        #region Private Variables
        private readonly bool _isEnabled;
        private readonly IReviewRepository _reviewRepository;
        private readonly ICacheRepository _cacheRepository;
        #endregion

        public ReviewClient(IReviewRepository reviewRepository, ICacheRepository cacheRepository)
        {
            _reviewRepository = reviewRepository;
            _cacheRepository = cacheRepository;
            _isEnabled = ReviewConfiguration.Instance.Cache.IsEnabled;
        }

        /// <summary>
        /// Gets the reviews.
        /// </summary>
        /// <returns></returns>
        public Review[] GetReviews(string itemId)
        {
            var query = _reviewRepository.Reviews.Where(r => 
                (string.IsNullOrEmpty(itemId) || r.ItemId == itemId)
                && r.Status == (int)ReviewStatus.Approved).ExpandAll();

            return CacheHelper.Get(
                CacheHelper.CreateCacheKey(Constants.ReviewsCachePrefix,string.Format(ReviewsCacheKey, string.IsNullOrEmpty(itemId) ? "all" : itemId)),
                query.ToArray, 
                ReviewConfiguration.Instance.Cache.ReviewsTimeout, 
                _isEnabled);
        }

        /// <summary>
        /// Gets the review comments.
        /// </summary>
        /// <param name="reviewId">The review identifier.</param>
        /// <returns></returns>
        public ReviewComment[] GetReviewComments(string reviewId)
        {
            var query =
                _reviewRepository.ReviewComments.Where(
                    r => r.ReviewId == reviewId && r.Status == (int) ReviewStatus.Approved);
            return CacheHelper.Get(
                CacheHelper.CreateCacheKey(Constants.ReviewsCachePrefix, string.Format(ReviewCommentsCacheKey, reviewId)),
                query.ToArray,
                ReviewConfiguration.Instance.Cache.ReviewsTimeout,
                _isEnabled);
        }

        CacheHelper _cacheHelper;
        public CacheHelper CacheHelper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }
    }
}
