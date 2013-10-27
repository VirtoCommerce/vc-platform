using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Reviews.Model;
using VirtoCommerce.Foundation.Reviews.Repositories;
using VirtoCommerce.Web.Client.Extensions.Filters;
using VirtoCommerce.Web.Client.Globalization;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Virto.Helpers;

namespace VirtoCommerce.Web.Controllers.Api
{
    /// <summary>
    /// Class ReviewController.
    /// </summary>
	[LocalizeWebApi]
    public class ReviewController : ApiController
    {
        /// <summary>
        /// The _order client
        /// </summary>
        private readonly OrderClient _orderClient;
        /// <summary>
        /// The _repository
        /// </summary>
        private readonly IReviewRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewController"/> class.
        /// </summary>
        public ReviewController()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewController"/> class.
        /// </summary>
        /// <param name="reviewRepository">The review repository.</param>
        /// <param name="orderClient">The order client.</param>
        public ReviewController(IReviewRepository reviewRepository, OrderClient orderClient)
        {
            _repository = reviewRepository;
            _orderClient = orderClient;
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>IQueryable{MReview}.</returns>
        [Queryable]
        [HttpGet]
        public IQueryable<MReview> Get()
        {
            return _repository.Reviews.Where(r => r.Status == 2)
                .Select(r => new MReview
                {
                    ItemId = r.ItemId,
                    Id = r.ReviewId,
                    PositiveFeedbackCount = r.TotalPositiveFeedbackCount,
                    NegativeFeedbackCount = r.TotalNegativeFeedbackCount,
                    AbuseCount = r.TotalAbuseCount,
                    Rating = r.OverallRating,
                    RatingComment = r.Title,
                    IsVerifiedBuyer = r.IsVerifiedBuyer,
                    ReviewText =
                        r.ReviewFieldValues.Select(
                            rf =>
                            new MReviewField
                                {
                                    Name = rf.Name,
                                    Text = rf.Value,
                                    Id = rf.ReviewFieldValueId
                                }).FirstOrDefault(),
                    CreatedDateTime = r.Created,
                    Comments =
                        r.ReviewComments.Where(
                            rc => rc.Status == 2)
                         .OrderByDescending(rc => rc.Created)
                         .Take(3)
                         .Select(rc => new MReviewComment
                             {
                                 Id = rc.ReviewCommentId,
                                 PositiveFeedbackCount = rc.TotalPositiveFeedbackCount,
                                 NegativeFeedbackCount = rc.TotalNegativeFeedbackCount,
                                 AbuseCount = rc.TotalAbuseCount,
                                 Comment = rc.Comment,
                                 CreatedDateTime = rc.Created,
                                 Reviewer = new MReviewer
                                     {
                                         Address = rc.AuthorLocation,
                                         Id = rc.AuthorId,
                                         NickName = rc.AuthorName
                                     }
                             }),
                    TotalComments = r.ReviewComments.Count(rc => rc.Status == (int)ReviewStatus.Approved),
                    Reviewer = new MReviewer
                        {
                            Address = r.AuthorLocation,
                            Id = r.AuthorId,
                            NickName = r.AuthorName
                        }
                });
        }

        /// <summary>
        /// Gets the comments.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IQueryable{MReviewComment}.</returns>
        [Queryable]
        [HttpGet]
        public IQueryable<MReviewComment> GetComments(string id)
        {
            return _repository.ReviewComments
                              .Where(r => r.ReviewId == id && r.Status == (int)ReviewStatus.Approved)
                              .Select(rc => new MReviewComment
                                  {
                                      Id = rc.ReviewCommentId,
                                      PositiveFeedbackCount = rc.TotalPositiveFeedbackCount,
                                      NegativeFeedbackCount = rc.TotalNegativeFeedbackCount,
                                      AbuseCount = rc.TotalAbuseCount,
                                      Comment = rc.Comment,
                                      CreatedDateTime = rc.Created,
                                      Reviewer = new MReviewer
                                          {
                                              Address = rc.AuthorLocation,
                                              Id = rc.AuthorId,
                                              NickName = rc.AuthorName
                                          }
                                  });
        }

        /// <summary>
        /// Gets the review totals.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Object.</returns>
        [HttpGet]
        public object GetReviewTotals(string id)
        {
            var result = new ReviewTotals();
            var reviews = _repository.Reviews.Where(r => r.ItemId == id && r.Status == (int)ReviewStatus.Approved);

            result.TotalReviews = reviews.Count();
            if (result.TotalReviews > 0)
            {
                result.AverageRating = Math.Round(reviews.Average(r => r.OverallRating), 1);
            }

            return result;
        }

        /// <summary>
        /// Votes the product review.
        /// </summary>
        /// <param name="voteParams">The vote parameters.</param>
        /// <returns>HttpResponseMessage.</returns>
        [ActionName("vote")]
        [HttpPost]
        public HttpResponseMessage VoteProductReview(VoteParameters voteParams)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            dynamic entity = voteParams.IsReview
                                 ? (StorageEntity)
                                   _repository.Reviews.FirstOrDefault(r => r.ReviewId == voteParams.Id)
                                 : _repository.ReviewComments.FirstOrDefault(r => r.ReviewCommentId == voteParams.Id);

            if (entity == null)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound);
                response.Content = new StringContent("Entity with given id not found".Localize());
                return response;
            }

            if (entity.AuthorId == UserHelper.CustomerSession.CustomerId)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound);
                response.Content =
                    new StringContent(string.Format("You cannot vote for your own {0}".Localize(),
                                                    voteParams.IsReview ? "review" : "comment"));
                return response;
            }

            if (_repository.ReportHelpfulElements.Any(rh => rh.AuthorId == UserHelper.CustomerSession.CustomerId &&
                                                            (voteParams.IsReview && rh.ReviewId == voteParams.Id ||
                                                             rh.CommentId == voteParams.Id)))
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound);
                response.Content =
                    new StringContent(string.Format("You may only submit one vote per {0}".Localize(),
                                                    voteParams.IsReview ? "review." : "comment."));
                return response;
            }

            if (voteParams.Like)
            {
                entity.TotalPositiveFeedbackCount++;
            }
            else
            {
                entity.TotalNegativeFeedbackCount++;
            }

            try
            {
                var sEntity = (StorageEntity)entity;
                _repository.Update(sEntity);

                var helpful = new ReportHelpfulElement
                    {
                        AuthorId = UserHelper.CustomerSession.CustomerId,
                        IsHelpful = voteParams.Like,
                        ReviewId = voteParams.IsReview ? voteParams.Id : null,
                        CommentId = voteParams.IsReview ? null : voteParams.Id
                    };

                _repository.Add(helpful);
                _repository.UnitOfWork.Commit();
            }
            catch (Exception)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound);
                response.Content = new StringContent("Error while saving data to database.".Localize());
                return response;
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Adds the review comment.
        /// </summary>
        /// <param name="commentParams">The comment parameters.</param>
        /// <returns>HttpResponseMessage.</returns>
        [ActionName("comment")]
        [HttpPut]
        [ModelValidationFilter]
        public HttpResponseMessage AddReviewComment(AddCommentParameters commentParams)
        {
            if (ModelState.IsValid)
            {
                var productreview = _repository.Reviews.FirstOrDefault(r => r.ReviewId == commentParams.Id);

                if (productreview == null)
                {
                    var response = Request.CreateResponse(HttpStatusCode.NotFound);
                    response.Content = new StringContent("Review with given id not found".Localize());
                    return response;
                }

                try
                {
                    var reviewComment = new ReviewComment
                        {
                            AuthorId = UserHelper.CustomerSession.CustomerId,
                            AuthorLocation = commentParams.AuthorLocation,
                            AuthorName = commentParams.AuthorName,
                            Comment = commentParams.Comment,
                            ReviewId = productreview.ReviewId,
                            ReviewCommentId = Guid.NewGuid().ToString(),
                            Status = (int)ReviewStatus.Pending
                        };

                    productreview.ReviewComments.Add(reviewComment);

                    _repository.Update(productreview);
                    _repository.UnitOfWork.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK, new MReviewComment
                        {
                            Comment = reviewComment.Comment,
                            CreatedDateTime = reviewComment.Created,
                            Id = reviewComment.ReviewCommentId,
                            Reviewer = new MReviewer
                                {
                                    Address = reviewComment.AuthorLocation,
                                    Id = reviewComment.AuthorId,
                                    NickName = reviewComment.AuthorName
                                }
                        });
                }
                catch
                {
                    var response = Request.CreateResponse(HttpStatusCode.NotFound);
                    response.Content = new StringContent("Error while saving data to database.".Localize());
                    return response;
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
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
        public HttpResponseMessage AddReview(MReview review)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            try
            {
                var isVerified = false;

                try
                {
                    var orders = _orderClient.GetAllCustomerOrders(UserHelper.CustomerSession.CustomerId,
                                                                   UserHelper.StoreClient.GetCurrentStore().StoreId);

                    if (orders != null)
                    {
                        isVerified = orders.ExpandAll().Where(o => o.Status.Contains("Completed"))
                                           .SelectMany(o => o.OrderForms)
                                           .SelectMany(of => of.LineItems)
                                           .Any(li => li.CatalogItemId == review.ItemId
                                                      ||
                                                      !string.IsNullOrEmpty(li.ParentCatalogItemId) &&
                                                      li.ParentCatalogItemId == review.ItemId);
                    }
                }
                catch
                {
                    //not verified
                }

                var dbReviewreview = new Review
                    {
                        AuthorId = UserHelper.CustomerSession.CustomerId,
                        AuthorLocation = review.Reviewer.Address,
                        AuthorName = review.Reviewer.NickName,
                        ItemId = review.ItemId,
                        ItemUrl = review.ItemId,
                        OverallRating = review.Rating,
                        Title = review.RatingComment,
                        IsVerifiedBuyer = isVerified,
                        Status = (int)ReviewStatus.Pending
                    };

                dbReviewreview.ReviewFieldValues.Add(new ReviewFieldValue
                    {
                        Name = "Review",
                        Value = review.ReviewText.Text,
                        ReviewFieldValueId = Guid.NewGuid().ToString()
                    });

                _repository.Add(dbReviewreview);
                _repository.UnitOfWork.Commit();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound);
                response.Content = new StringContent("Error while saving data to database.".Localize() + " " + ex.Message);
                return response;
            }
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
        public HttpResponseMessage ReportAbuse(MReportAbuse abuse)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var abuseEl = new ReportAbuseElement
                        {
                            AuthorId = UserHelper.CustomerSession.CustomerId,
                            Comment = abuse.Comment,
                            ReviewId = abuse.IsReview ? abuse.Id : null,
                            CommentId = abuse.IsReview ? null : abuse.Id,
                            Email = abuse.Email,
                            Reason = abuse.Reason
                        };

                    _repository.Add(abuseEl);
                    _repository.UnitOfWork.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch
                {
                    var response = Request.CreateResponse(HttpStatusCode.NotFound);
                    response.Content = new StringContent("Error while saving data to database.".Localize());
                    return response;
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}