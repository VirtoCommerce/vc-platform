using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace VirtoCommerce.Web.Models
{
	/// <summary>
	/// Class ReviewTotals.
	/// </summary>
    public class ReviewTotals
    {
		/// <summary>
		/// Gets or sets the total reviews.
		/// </summary>
		/// <value>The total reviews.</value>
        public int TotalReviews { get; set; }
		/// <summary>
		/// Gets or sets the average rating.
		/// </summary>
		/// <value>The average rating.</value>
        public double AverageRating { get; set; }

        public string ItemId { get; set; }
    }

	/// <summary>
	/// Class MReview.
	/// </summary>
    public class MReview
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="MReview"/> class.
		/// </summary>
        public MReview()
        {
            Reviewer = new MReviewer();
            ReviewText = new MReviewField();
        }

        //Just for filtering
		/// <summary>
		/// Gets or sets the item identifier.
		/// </summary>
		/// <value>The item identifier.</value>
        public string ItemId { get; set; }

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
        public string Id { get; set; }

		/// <summary>
		/// Gets or sets the rating.
		/// </summary>
		/// <value>The rating.</value>
        [Display(Name = "Your Rating")]
        [Range(1, 5, ErrorMessage = "Review rating must be set")]
        public int Rating { get; set; }

		/// <summary>
		/// Gets or sets the rating comment.
		/// </summary>
		/// <value>The rating comment.</value>
        [Required]
        [Display(Name = "Review Headline")]
        public string RatingComment { get; set; }

		/// <summary>
		/// Gets or sets the positive feedback count.
		/// </summary>
		/// <value>The positive feedback count.</value>
        public int PositiveFeedbackCount { get; set; }
		/// <summary>
		/// Gets or sets the negative feedback count.
		/// </summary>
		/// <value>The negative feedback count.</value>
        public int NegativeFeedbackCount { get; set; }
		/// <summary>
		/// Gets or sets the abuse count.
		/// </summary>
		/// <value>The abuse count.</value>
        public int AbuseCount { get; set; }

		/// <summary>
		/// Gets or sets the created date time.
		/// </summary>
		/// <value>The created date time.</value>
        public DateTime? CreatedDateTime { get; set; }
		/// <summary>
		/// Gets or sets the reviewer.
		/// </summary>
		/// <value>The reviewer.</value>
        public MReviewer Reviewer { get; set; }
		/// <summary>
		/// Gets or sets the review text.
		/// </summary>
		/// <value>The review text.</value>
        public MReviewField ReviewText { get; set; }
		/// <summary>
		/// Gets or sets the comments.
		/// </summary>
		/// <value>The comments.</value>
        public IEnumerable<MReviewComment> Comments { get; set; }
		/// <summary>
		/// Gets or sets the total comments.
		/// </summary>
		/// <value>The total comments.</value>
        public int TotalComments { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this instance is verified buyer.
		/// </summary>
		/// <value><c>true</c> if this instance is verified buyer; otherwise, <c>false</c>.</value>
        public bool IsVerifiedBuyer { get; set; }
    }

	/// <summary>
	/// Class MReviewComment.
	/// </summary>
    public class MReviewComment
    {
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
        public string Id { get; set; }
		/// <summary>
		/// Gets or sets the comment.
		/// </summary>
		/// <value>The comment.</value>
        public string Comment { get; set; }
		/// <summary>
		/// Gets or sets the positive feedback count.
		/// </summary>
		/// <value>The positive feedback count.</value>
        public int PositiveFeedbackCount { get; set; }
		/// <summary>
		/// Gets or sets the negative feedback count.
		/// </summary>
		/// <value>The negative feedback count.</value>
        public int NegativeFeedbackCount { get; set; }
		/// <summary>
		/// Gets or sets the abuse count.
		/// </summary>
		/// <value>The abuse count.</value>
        public int AbuseCount { get; set; }
		/// <summary>
		/// Gets or sets the created date time.
		/// </summary>
		/// <value>The created date time.</value>
        public DateTime? CreatedDateTime { get; set; }
		/// <summary>
		/// Gets or sets the reviewer.
		/// </summary>
		/// <value>The reviewer.</value>
        public MReviewer Reviewer { get; set; }
    }

	/// <summary>
	/// Class MReviewField.
	/// </summary>
    public class MReviewField
    {
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
        public string Id { get; set; }
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        public string Name { get; set; }

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
        [Required]
        [Display(Name = "Review")]
        public string Text { get; set; }
    }

	/// <summary>
	/// Class MReportAbuse.
	/// </summary>
    public class MReportAbuse
    {
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
        [Required]
        public string Id { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is review.
		/// </summary>
		/// <value><c>true</c> if this instance is review; otherwise, <c>false</c>.</value>
        public bool IsReview { get; set; }
		/// <summary>
		/// Gets or sets the comment.
		/// </summary>
		/// <value>The comment.</value>
        public string Comment { get; set; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>The email.</value>
        [Required]
        [Email]
        public string Email { get; set; }

		/// <summary>
		/// Gets or sets the reason.
		/// </summary>
		/// <value>The reason.</value>
        [Required]
        public string Reason { get; set; }
    }

	/// <summary>
	/// Class MReviewer.
	/// </summary>
    public class MReviewer
    {
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
        public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the nick.
		/// </summary>
		/// <value>The name of the nick.</value>
        [Required]
        [Display(Name = "Author")]
        public string NickName { get; set; }

		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		/// <value>The address.</value>
        [Required]
        [Display(Name = "Your Location")]
        public string Address { get; set; }
    }

	/// <summary>
	/// Class VoteParameters.
	/// </summary>
    public class VoteParameters
    {
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
        public string Id { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="VoteParameters"/> is like.
		/// </summary>
		/// <value><c>true</c> if like; otherwise, <c>false</c>.</value>
        public bool Like { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this instance is review.
		/// </summary>
		/// <value><c>true</c> if this instance is review; otherwise, <c>false</c>.</value>
        public bool IsReview { get; set; }
    }

	/// <summary>
	/// Class AddCommentParameters.
	/// </summary>
    public class AddCommentParameters
    {
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
        public string Id { get; set; }

		/// <summary>
		/// Gets or sets the comment.
		/// </summary>
		/// <value>The comment.</value>
        [Required]
        public string Comment { get; set; }

		/// <summary>
		/// Gets or sets the name of the author.
		/// </summary>
		/// <value>The name of the author.</value>
        [Required]
        public string AuthorName { get; set; }

		/// <summary>
		/// Gets or sets the author location.
		/// </summary>
		/// <value>The author location.</value>
        public string AuthorLocation { get; set; }
    }
}