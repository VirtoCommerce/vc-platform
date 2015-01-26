using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiWebClient.Extensions.Filters;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Controllers.Api
{
    /// <summary>
    /// Class ReviewController.
    /// </summary>
    [LocalizeWebApi]
    public class ReviewController : ApiController
    {

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <param name="id">The item identifier.</param>
        /// <returns>
        /// IQueryable{MReview}.
        /// </returns>
        [EnableQuery]
        [HttpGet]
        public async Task<IQueryable<MReview>> Get(string id)
        {
            var reviewClient = ClientContext.Clients.CreateReviewsClient();
            var reviews = await reviewClient.GetReviewsAsync(id);

            return reviews.Items.Select(r => new MReview
                {
                    ItemId = id,
                    Id = r.Id,
                    PositiveFeedbackCount = r.PositiveFeedbackCount,
                    NegativeFeedbackCount = r.NegativeFeedbackCount,
                    AbuseCount = r.AbuseCount,
                    Rating = r.Rating,
                    RatingComment = r.RatingComment,
                    IsVerifiedBuyer = false,//r.IsVerifiedBuyer,
                    ReviewText = r.ReviewText,
                    CreatedDateTime = r.Created,
                    Comments =
                        r.Comments
                         .OrderByDescending(rc => rc.CreatedDateTime)
                         .Take(3)
                         .Select(rc => new MReviewComment
                             {
                                 Id = rc.Id,
                                 PositiveFeedbackCount = rc.PositiveFeedbackCount,
                                 NegativeFeedbackCount = rc.NegativeFeedbackCount,
                                 AbuseCount = rc.AbuseCount,
                                 Comment = rc.Comment,
                                 CreatedDateTime = rc.CreatedDateTime,
                                 Reviewer = new MReviewer
                                     {
                                         Address = "No address",
                                         Id = "",
                                         NickName = rc.Author
                                     }
                             }),
                    TotalComments = r.Comments.Count(),
                    Reviewer = new MReviewer
                        {
                            Address = r.AuthorLocation,
                            Id = r.AuthorId,
                            NickName = r.AuthorName
                        }
                }).AsQueryable();
                
        }

        /// <summary>
        /// Gets the comments.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IQueryable{MReviewComment}.</returns>
        [EnableQuery]
        [HttpGet]
        public IQueryable<MReviewComment> GetComments(string id)
        {
            //return _reviewClient.GetReviewComments(id)
            //                  .Select(rc => new MReviewComment
            //                      {
            //                          Id = rc.ReviewCommentId,
            //                          PositiveFeedbackCount = rc.TotalPositiveFeedbackCount,
            //                          NegativeFeedbackCount = rc.TotalNegativeFeedbackCount,
            //                          AbuseCount = rc.TotalAbuseCount,
            //                          Comment = rc.Comment,
            //                          CreatedDateTime = rc.Created,
            //                          Reviewer = new MReviewer
            //                              {
            //                                  Address = rc.AuthorLocation,
            //                                  Id = rc.AuthorId,
            //                                  NickName = rc.AuthorName
            //                              }
            //                      }).AsQueryable();

            return null;
        }

        /// <summary>
        /// Gets the review totals.
        /// </summary>
        /// <param name="id">The item identifier.</param>
        /// <returns>
        /// ReviewTotals querable
        /// </returns>
        [HttpGet]
        [EnableQuery(MaxNodeCount = 500)]
        public async Task<ReviewTotals> GetReviewTotals(string id)
        {
            var reviewClient = ClientContext.Clients.CreateReviewsClient();
            var reviews = await reviewClient.GetReviewsAsync(id);

            return new ReviewTotals
            {
                TotalReviews = reviews.TotalCount,
                AverageRating = reviews.Items.Any() ? Math.Round(reviews.Items.Average(r => r.Rating), 1) : 0,
                ItemId = id
            };

        }

        /// <summary>
        /// Votes the product review.
        /// </summary>
        /// <param name="voteParams">The vote parameters.</param>
        /// <returns>HttpResponseMessage.</returns>
        [ActionName("vote")]
        [HttpPost]
        public IHttpActionResult VoteProductReview(VoteParameters voteParams)
        {
            return Ok();
        }

        /// <summary>
        /// Adds the review comment.
        /// </summary>
        /// <param name="commentParams">The comment parameters.</param>
        /// <returns>HttpResponseMessage.</returns>
        [ActionName("comment")]
        [HttpPut]
        [ModelValidationFilter]
        public IHttpActionResult AddReviewComment(AddCommentParameters commentParams)
        {
            return Ok();
        }

        /// <summary>
        /// Adds the review.
        /// </summary>
        /// <param name="review">The review.</param>
        /// <returns>HttpResponseMessage.</returns>
        [ActionName("addreview")]
        [HttpPut]
        [Authorize]
        [ModelValidationFilter]
        public IHttpActionResult AddReview(MReview review)
        {
            return Ok();
        }

        //api/review/reportabuse
        /// <summary>
        /// Reports the abuse.
        /// </summary>
        /// <param name="abuse">The abuse.</param>
        /// <returns>HttpResponseMessage.</returns>
        [ActionName("reportabuse")]
        [HttpPut]
        [ModelValidationFilter]
        public IHttpActionResult ReportAbuse(MReportAbuse abuse)
        {
            return Ok();
        }
    }
}