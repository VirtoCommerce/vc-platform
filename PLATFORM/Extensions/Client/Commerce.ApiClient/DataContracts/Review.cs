#region

using System;

#endregion

namespace VirtoCommerce.ApiClient.DataContracts
{

    #region

    #endregion

    public class Review
    {
        #region Public Properties

        public int AbuseCount { get; set; }

        public string AuthorId { get; set; }

        public string AuthorLocation { get; set; }

        public string AuthorName { get; set; }

        public ReviewComment[] Comments { get; set; }

        public DateTime? Created { get; set; }

        public string Id { get; set; }

        public int NegativeFeedbackCount { get; set; }

        public int PositiveFeedbackCount { get; set; }

        public int Rating { get; set; }

        public string RatingComment { get; set; }

        public string ReviewText { get; set; }

        public int TotalComments { get; set; }

        #endregion
    }
}
