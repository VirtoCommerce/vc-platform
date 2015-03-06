using System;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class ReviewComment : IFeedbackCounterSupport
    {
        #region Public Properties

        public int AbuseCount { get; set; }
        public string Author { get; set; }
        public string Comment { get; set; }

        public DateTime? CreatedDateTime { get; set; }
        public string Id { get; set; }

        public int NegativeFeedbackCount { get; set; }
        public int PositiveFeedbackCount { get; set; }

        #endregion
    }
}
