namespace VirtoCommerce.ApiClient.DataContracts.Cart
{
    public class Coupon
    {
        #region Public Properties

        public string CouponCode { get; set; }

        public string InvalidDescription { get; set; }

        public bool IsValid { get; set; }

        #endregion
    }
}
