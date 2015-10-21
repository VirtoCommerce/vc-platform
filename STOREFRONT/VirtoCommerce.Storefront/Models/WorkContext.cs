using System;
using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Models
{
    /// <summary>
    /// Main working context contains all data which could be used in presentation logic
    /// </summary>
    public class WorkContext : IDisposable
    {
        public WorkContext()
        {
            AllStores = new List<Store>();
        }
        /// <summary>
        /// Current customer
        /// </summary>
        public Customer Customer { get; set; }
        /// <summary>
        /// Language culture name format (e.g. en-US)
        /// </summary>
        public string CurrentLanguage { get; set; }
        /// <summary>
        /// Currency code in ISO 4217 format (e.g. USD)
        /// </summary>
        public string CurrentCurrency { get; set; }

        public Store CurrentStore { get; set; }

        /// <summary>
        /// List of all supported stores
        /// </summary>
        public ICollection<Store> AllStores { get; set; }

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        #endregion
    }
}
