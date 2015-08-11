using System;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class ReviewComment : IFeedbackCounterSupport
    {
        /// <summary>
        /// Gets or sets the value of review comment abuse reports count
        /// </summary>
        public int AbuseCount { get; set; }

        /// <summary>
        /// Gets or sets the value of review comment author name
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the value of review comment text content
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the value of review comment created date/time
        /// </summary>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the value of review comment id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the value of review comment negative feedback count
        /// </summary>
        public int NegativeFeedbackCount { get; set; }

        /// <summary>
        /// Gets or sets the value of review comment positive feedback count
        /// </summary>
        public int PositiveFeedbackCount { get; set; }
    }
}