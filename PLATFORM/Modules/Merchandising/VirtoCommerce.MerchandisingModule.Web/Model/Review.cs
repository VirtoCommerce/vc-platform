using System;
using System.Collections.Generic;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Review : IFeedbackCounterSupport
    {
        /// <summary>
        /// Gets or sets the value of review abuse reports count
        /// </summary>
        public int AbuseCount { get; set; }

        /// <summary>
        /// Gets or sets the value of review author id
        /// </summary>
        public string AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the value of review author location
        /// </summary>
        public string AuthorLocation { get; set; }

        /// <summary>
        /// Gets or sets the value of Review author name
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets the collection of review comments
        /// </summary>
        /// <value>
        /// Collection of ReviewComment objects
        /// </value>
        public ICollection<ReviewComment> Comments { get; set; }

        /// <summary>
        /// Gets or sets the value of review created date/time
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        /// Gets or sets the value of review id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the value of review negative feedback count
        /// </summary>
        public int NegativeFeedbackCount { get; set; }

        /// <summary>
        /// Gets or sets the value of review positive feedback count
        /// </summary>
        public int PositiveFeedbackCount { get; set; }

        /// <summary>
        /// Gets or sets the value of review rating
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the value of review rating comment
        /// </summary>
        public string RatingComment { get; set; }

        /// <summary>
        /// Gets or sets the value of review text content
        /// </summary>
        public string ReviewText { get; set; }

        /// <summary>
        /// Gets or sets the value of review total comments count
        /// </summary>
        public int TotalComments { get; set; }
    }
}