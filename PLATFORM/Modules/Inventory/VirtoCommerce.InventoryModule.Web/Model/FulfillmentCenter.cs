using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.InventoryModule.Web.Model
{
	public class FulfillmentCenter : AuditableEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
        public int MaxReleasesPerPickBatch { get; set; }
        public int PickDelay { get; set; }
		public string DaytimePhoneNumber { get; set; }
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string City { get; set; }
		
        public string StateProvince { get; set; }
        
        /// <summary>
        /// ISO 3166-1 alpha-3
        /// </summary>
        public string CountryCode { get; set; }
		public string CountryName { get; set; }
		public string PostalCode { get; set; }
	}
}
