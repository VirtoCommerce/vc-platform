using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.StoreModule.Web.Model
{
    public class TaxProvider
    {
        /// <summary>
		/// Inner unique method code
		/// </summary>
		public string Code { get; set; }

        /// <summary>
        /// Display name of shipping method
        /// </summary>
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Absolute logo url of shipping method, can be used in UI
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// If true - method can be available on storefront
        /// </summary>
        public bool IsActive { get; set; }

        public int Priority { get; set; }

        public ICollection<Setting> Settings { get; set; }
    }
}