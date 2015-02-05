namespace VirtoCommerce.ApiClient.DataContracts.Cart
{
	public class Coupon
	{
		public string CouponCode { get; set; }
		public bool IsValid { get; set; }
		public string InvalidDescription { get; set; }
	}
}
