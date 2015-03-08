using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Reviews.Repositories;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.MerchandisingModule.Web.Model;
using foundation = VirtoCommerce.Foundation.Reviews.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    public class ReviewController : ApiController
    {
        #region Fields

        private readonly Func<IReviewRepository> _reviewRepositoryFactory;

        #endregion

        #region Constructors and Destructors

        public ReviewController(Func<IReviewRepository> reviewRepositoryFactory)
        {
            this._reviewRepositoryFactory = reviewRepositoryFactory;
        }

        #endregion

        #region Public Methods and Operators

        [HttpGet]
        [Route("~/api/mp/{language}/products/{productId}/reviews")]
        [ResponseType(typeof(ResponseCollection<Review>))]
        public IHttpActionResult GetAllProductReviews(string language, string productId)
        {
            var retVal = new ResponseCollection<Review>();
            using (var repository = this._reviewRepositoryFactory())
            {
                var reviews =
                    repository.Reviews.Where(
                        x =>
                            (string.IsNullOrEmpty(productId) || x.ItemId == productId)
                                && x.Status == (int)foundation.ReviewStatus.Approved)
                        .ExpandAll()
                        .ToArray();
                retVal.Items = reviews.Select(x => x.ToWebModel()).ToArray();
                retVal.TotalCount = retVal.Items.Count;
            }
            return this.Ok(retVal);
        }

        #endregion
    }
}
