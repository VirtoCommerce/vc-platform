using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CartModule.Web.Model
{
    public class Coupon : ValueObject<Coupon>
    {
        /// <summary>
        /// Gets or sets the value of coupon code
        /// </summary>
        public string CouponCode { get; set; }

        /// <summary>
        /// Gets or sets the value of description of invalid operation with coupon
        /// </summary>
        public string InvalidDescription { get; set; }
    }
}