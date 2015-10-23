using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Represent store - main ecommerce aggregate unit
    /// </summary>
    public class Store
    {
        public Store()
        {
            Languages = new List<string>();
            Currencies = new List<string>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Url of store storefront
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Secure url of store, must use https protocol, required
        /// </summary>
        public string SecureUrl { get; set; }

        /// <summary>
        /// State of store (open, closing, maintenance)
        /// </summary>
        public string StoreState { get; set; }

        public string TimeZone { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        /// <summary>
        /// Default Language culture name  of store ( example en-US )
        /// </summary>
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// All supported lanuagages
        /// </summary>
        public ICollection<string> Languages { get; set; }

        /// <summary>
        /// Default currency of store. Use ISO 4217 currency codes
        /// </summary>
        public string DefaultCurrency { get; set; }
        /// <summary>
        /// List of supported additional currencies
        /// </summary>
        public ICollection<string> Currencies { get; set; }

        /// <summary>
        /// Product catalog id assigned to store
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// Contact email of store
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Administrator contact email of store
        /// </summary>
        public string AdminEmail { get; set; }

        /// <summary>
        /// Store theme name
        /// </summary>
        public string ThemeName { get; set; }
  
        public ICollection<DynamicProperty> DynamicProperties { get; set; }

        public ICollection<SeoInfo> SeoInfos { get; set; }
    }
}