namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public interface IFeedbackCounterSupport
    {
        #region Public Properties

        int AbuseCount { get; set; }
        int NegativeFeedbackCount { get; set; }
        int PositiveFeedbackCount { get; set; }

        #endregion
    }
}
