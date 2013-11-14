
namespace VirtoCommerce.Web.Models
{
	/// <summary>
	/// Class AddressCartModel.
	/// </summary>
	public class AddressCartModel
	{
		/// <summary>
		/// Gets or sets the region identifier.
		/// </summary>
		/// <value>The region identifier.</value>
		public string RegionId { get; set; }
		/// <summary>
		/// Gets or sets the estimate postcode.
		/// </summary>
		/// <value>The estimate postcode.</value>
		public string EstimatePostcode { get; set; }
		/// <summary>
		/// Gets or sets the state province.
		/// </summary>
		/// <value>The state province.</value>
		public string StateProvince { get; set; }
		/// <summary>
		/// Gets or sets the address state province identifier.
		/// </summary>
		/// <value>The address state province identifier.</value>
		public string AddressStateProvinceId { get; set; }
	}
}
