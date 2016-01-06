using System;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    /// <summary>
    /// Main working context contains all data which could be used in presentation logic
    /// </summary>
    public class ApiWorkContext 
    {
        public ApiWorkContext()
        {
            //CurrentPriceListIds = new List<string>();
            //CurrentLinkLists = new List<MenuLinkList>();
        }
      
        public string StoreId { get; set; }
        public string CatalogId { get; set; }
        public string CategoryId { get; set; }
        public string ItemId { get; set; }
        
        private DateTime? _utcNow;
        /// <summary>
        /// Represent current storefront datetime in UTC (may be changes to test in future or past events)
        /// </summary>
        public DateTime StorefrontUtcNow
        {
            get
            {
                return _utcNow == null ? DateTime.UtcNow : _utcNow.Value;
            }
            set
            {
                _utcNow = value;
            }
        }
    }
}
