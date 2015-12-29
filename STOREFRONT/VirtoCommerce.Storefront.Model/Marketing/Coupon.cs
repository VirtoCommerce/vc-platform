using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Marketing
{
    /// <summary>
    /// Represents coupon object
    /// </summary>
    public class Coupon
    {
        /// <summary>
        /// Gets or sets coupon code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets coupon description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the sign that coupon was applied successfully
        /// </summary>
        public bool AppliedSuccessfully { get; set; }

        /// <summary>
        /// Gets or sets coupon error code
        /// </summary>
        public string ErrorCode { get; set; }
    }
}