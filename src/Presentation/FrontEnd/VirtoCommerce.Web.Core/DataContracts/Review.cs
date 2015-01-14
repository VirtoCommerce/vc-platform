using System;

namespace VirtoCommerce.Web.Core.DataContracts
{
    public class Review
    {
        public string Id { get; set; }

        public int Rating { get; set; }
        public string RatingComment { get; set; }

        public DateTime? Created { get; set; }

        public string AuthorName { get; set; }

        public string AuthorId { get; set; }

        public string AuthorLocation { get; set; }

        public string ReviewText { get; set; }

        public ReviewComment[] Comments { get; set; }

        public int TotalComments { get; set; }

        public int PositiveFeedbackCount { get; set; }
        public int NegativeFeedbackCount { get; set; }
        public int AbuseCount { get; set; }

    }
}
